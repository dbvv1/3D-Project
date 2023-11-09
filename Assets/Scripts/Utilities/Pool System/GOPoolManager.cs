using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//简单对象池
public class GOPoolManager : Singleton<GOPoolManager>
{
    [SerializeField, Header("预制体")] private List<GameObjectAssets> assetsList = new List<GameObjectAssets>();
    [SerializeField] private Transform poolObjectParent;

    //名字对应对象的预制体  用于对象池的动态扩充
    private Dictionary<string, GameObject[]> nameToPrefab = new();

    //实际的对象池
    private Dictionary<string, Queue<GameObject>> pools = new ();

    protected override void Awake()
    {
        base.Awake();
        InitPool();
    }

    private void Start()
    {

    }

    private void InitPool()
    {
        if (assetsList.Count == 0) return;

        //遍历外面配置的资源 进行初始化
        for (int i = 0; i < assetsList.Count; i++)
        {
            //检查列表第一个元素的内容是否已经在池子里面了，没有的话就创建一个
            if (!pools.ContainsKey(assetsList[i].assetsName))
            {
                pools.Add(assetsList[i].assetsName, new Queue<GameObject>());
                nameToPrefab.Add(assetsList[i].assetsName, assetsList[i].prefab);
                if (assetsList[i].prefab.Length == 0) return;

                //创建完毕后，遍历这个对象的总数，比如总算5，那么就创建5个，然后存进字典
                for (int j = 0; j < assetsList[i].count; j++)
                {
                    GameObject temp_Gameobject = Instantiate(assetsList[i].prefab[Random.Range(0, assetsList[i].prefab.Length)]);
                    temp_Gameobject.transform.SetParent(poolObjectParent);
                    temp_Gameobject.transform.position = Vector3.zero;
                    temp_Gameobject.transform.rotation = Quaternion.identity;
                    pools[assetsList[i].assetsName].Enqueue(temp_Gameobject);
                    temp_Gameobject.SetActive(false);
                }
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
        return dequeueObject;
    }

    //立即设置其位置和旋转，然后调用其SpawnObject方法。因为这个方法已经对获取的对象进行了操作，所以它不需要返回这个对象。
    public void TakeGameobject(string objectName, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(objectName)) return;

        if (pools[objectName].Count == 0)
            ExpansionPoolCapacity(objectName);

        GameObject dequeueObject = pools[objectName].Dequeue();
        dequeueObject.SetActive(true);
        dequeueObject.transform.position = position;
        dequeueObject.transform.rotation = rotation;
        dequeueObject.GetComponent<IPool>().SettingObject();
    }

    //立即设置其位置、旋转和用户，然后调用其SpawnObject方法。同样，因为这个方法已经对获取的对象进行了操作.
    public void TakeGameobject(string objectName, Vector3 position, Quaternion rotation, Transform user)
    {
        if (!pools.ContainsKey(objectName)) return;

        if (pools[objectName].Count == 0)
            ExpansionPoolCapacity(objectName);

        GameObject dequeueObject = pools[objectName].Dequeue();
        dequeueObject.SetActive(true);
        dequeueObject.transform.position = position;
        dequeueObject.transform.rotation = rotation;
        dequeueObject.GetComponent<IPool>().SettingObject(user);
    }

    public void RecycleGameObject(GameObject gameObject,string objectName)
    {
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
        pools[objectName].Enqueue(gameObject);

    }

    //在对象池中已经没有该种类的对象的时候， 根据对象名 Instantiate 出一个新的对象并且分配使用 实现动态扩容
    private void ExpansionPoolCapacity(string objectName)
    {
        GameObject newGO = Instantiate(nameToPrefab[objectName][Random.Range(0, nameToPrefab[objectName].Length)]);
        newGO.transform.SetParent(poolObjectParent);
        newGO.SetActive(false);
        pools[objectName].Enqueue(newGO);
    }


    [System.Serializable]
    private class GameObjectAssets
    {
        public string assetsName;
        public int count;
        public GameObject[] prefab;
    }

}
