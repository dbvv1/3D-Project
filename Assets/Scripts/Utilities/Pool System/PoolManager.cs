using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//简单对象池
public class PoolManager : Singleton<PoolManager>
{
    [SerializeField, Header("预制体")] private List<GameObjectAssets> assetsList = new List<GameObjectAssets>();
    public Transform poolObjectParent;

    //名字对应对象的预制体  用于对象池的动态扩充
    private Dictionary<string, GameObject[]> nameToPrefab = new();

    //实际的对象池
    private Dictionary<string, Queue<GameObject>> pools = new ();

    //对象池的使用信息
    private Dictionary<string, PoolUseInfo> poolUseInfos = new();

    protected override void Awake()
    {
        base.Awake();
        InitPool();
    }

    private void Start()
    {
        // 默认每五分钟清理一次对象池  （后续可变更为不同种类场景不同的清理时间）
        InvokeRepeating(nameof(ClearExcessObjects), 300f, 300f);
    }


    private void InitPool()
    {
        if (assetsList.Count == 0) return;

        //遍历外面配置的资源 进行初始化
        for (int i = 0; i < assetsList.Count; i++)
        {
            //检查列表元素的内容是否已经在池子里面了，没有的话就创建一个
            if (!pools.ContainsKey(assetsList[i].assetsName))
            {
                pools.Add(assetsList[i].assetsName, new Queue<GameObject>());
                nameToPrefab.Add(assetsList[i].assetsName, assetsList[i].prefab);
                poolUseInfos.Add(assetsList[i].assetsName, new PoolUseInfo(assetsList[i].count, 0,assetsList[i].count));
            }
            //处理assetsList列表中包含了重复对象的情况
            else
            {
                poolUseInfos[assetsList[i].assetsName].totalCount += assetsList[i].count;
                poolUseInfos[assetsList[i].assetsName].initCount += assetsList[i].count;
            }
            //创建完毕后，遍历这个对象的总数，比如总算5，那么就创建5个，然后存进字典
            for (int j = 0; j < assetsList[i].count; j++)
            {
                GameObject tempGameObject = Instantiate(assetsList[i].prefab[Random.Range(0, assetsList[i].prefab.Length)], poolObjectParent, true);
                tempGameObject.transform.position = Vector3.zero;
                tempGameObject.transform.rotation = Quaternion.identity;
                pools[assetsList[i].assetsName].Enqueue(tempGameObject);
                tempGameObject.SetActive(false);
            }
        }
    }

    //只是想获取一个对象，但不需要立即对其进行操作（如设置位置或旋转）时使用
    public GameObject TakeGameObject(string objectName)
    {
        if (!pools.ContainsKey(objectName)) return null;

        if (pools[objectName].Count == 0)
            ExpansionPoolCapacity(objectName);

        GameObject dequeueObject = pools[objectName].Dequeue();
        dequeueObject.SetActive(true);
        poolUseInfos[objectName].useCount++;
        return dequeueObject;
    }

    //立即设置其位置和旋转，然后调用其SpawnObject方法。因为这个方法已经对获取的对象进行了操作，所以它不需要返回这个对象。
    public void TakeGameObject(string objectName, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(objectName)) return;

        if (pools[objectName].Count == 0)
            ExpansionPoolCapacity(objectName);

        GameObject dequeueObject = pools[objectName].Dequeue();
        dequeueObject.SetActive(true);
        dequeueObject.transform.position = position;
        dequeueObject.transform.rotation = rotation;
        poolUseInfos[objectName].useCount++;
        dequeueObject.GetComponent<IPool>().SettingObject();
    }

    //立即设置其位置、旋转和用户，然后调用其SpawnObject方法。同样，因为这个方法已经对获取的对象进行了操作.
    public void TakeGameObject(string objectName, Vector3 position, Quaternion rotation, Transform user)
    {
        if (!pools.ContainsKey(objectName)) return;

        if (pools[objectName].Count == 0)
            ExpansionPoolCapacity(objectName);

        GameObject dequeueObject = pools[objectName].Dequeue();
        dequeueObject.SetActive(true);
        dequeueObject.transform.position = position;
        dequeueObject.transform.rotation = rotation;
        poolUseInfos[objectName].useCount++;
        dequeueObject.GetComponent<IPool>().SettingObject(user);
    }

    public void RecycleGameObject(GameObject gameObject,string objectName)
    {
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
        poolUseInfos[objectName].useCount--;
        pools[objectName].Enqueue(gameObject);

    }

    //在对象池中已经没有该种类的对象的时候， 根据对象名 Instantiate 出一个新的对象并且分配使用 实现动态扩容
    private void ExpansionPoolCapacity(string objectName)
    {
        //每次扩容1.5倍的容量
        int expandCount = Mathf.CeilToInt(poolUseInfos[objectName].totalCount * 1.5f) - poolUseInfos[objectName].totalCount;
        if (expandCount == 0) expandCount = 1; //防止初始数量为0时无法扩充
        poolUseInfos[objectName].totalCount += expandCount;
        for (int i = 0; i < expandCount; i++)
        {
            var newGo = Instantiate(nameToPrefab[objectName][Random.Range(0, nameToPrefab[objectName].Length)], poolObjectParent, true);
            newGo.SetActive(false);
            pools[objectName].Enqueue(newGo);
        }
        
    }
    
    //清理对象池中过多的对象：既可以定时调用，也可以手动调用
    public void ClearExcessObjects()
    {
        StopAllCoroutines();
        StartCoroutine(ClearPools());
    }

    private IEnumerator ClearPools()
    {
        foreach (var poolInfo in poolUseInfos)
        {
            //当某种对象未使用的对象超过了总对象数目的一半时，进行清理工作
            if (poolInfo.Value.NeedClearObject())
            {
                //每次减容到原先总容量的 2/3 即去除掉 1/3 数量的对象
                int clearCount = (int)(poolInfo.Value.totalCount * 0.33f);
                if (poolInfo.Value.totalCount - clearCount < poolInfo.Value.initCount)
                    clearCount = poolInfo.Value.totalCount - poolInfo.Value.initCount;
                poolInfo.Value.totalCount -= clearCount;
                for (int i = 0; i < clearCount; i++)
                {
                    var clearObject = pools[poolInfo.Key].Dequeue();
                    clearObject.SetActive(false);
                    Destroy(clearObject);
                }
            }
            //每帧处理一种对象，放置对象池中对象种类过多，一次性处理造成的卡顿
            yield return null;
        }
       
    }


    [System.Serializable]
    private class GameObjectAssets
    {
        public string assetsName;
        public int count;
        public GameObject[] prefab;
    }

    private class PoolUseInfo
    {
        public PoolUseInfo(int totalCount, int useCount,int initCount)
        {
            this.totalCount = totalCount;
            this.useCount = useCount;
            this.initCount = initCount;
        }

        public bool NeedClearObject()
        {
            return (totalCount - useCount) * 2 >= totalCount && totalCount > initCount;
        }
        
        public int totalCount;
        public int useCount;
        public int initCount;
    }

}
