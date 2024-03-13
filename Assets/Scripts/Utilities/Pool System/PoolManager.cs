using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//�򵥶����
public class PoolManager : Singleton<PoolManager>
{
    [SerializeField, Header("Ԥ����")] private List<GameObjectAssets> assetsList = new List<GameObjectAssets>();
    public Transform poolObjectParent;

    //���ֶ�Ӧ�����Ԥ����  ���ڶ���صĶ�̬����
    private Dictionary<string, GameObject[]> nameToPrefab = new();

    //ʵ�ʵĶ����
    private Dictionary<string, Queue<GameObject>> pools = new ();

    //����ص�ʹ����Ϣ
    private Dictionary<string, PoolUseInfo> poolUseInfos = new();

    protected override void Awake()
    {
        base.Awake();
        InitPool();
    }

    private void Start()
    {
        // Ĭ��ÿ���������һ�ζ����  �������ɱ��Ϊ��ͬ���ೡ����ͬ������ʱ�䣩
        InvokeRepeating(nameof(ClearExcessObjects), 300f, 300f);
    }


    private void InitPool()
    {
        if (assetsList.Count == 0) return;

        //�����������õ���Դ ���г�ʼ��
        for (int i = 0; i < assetsList.Count; i++)
        {
            //����б�Ԫ�ص������Ƿ��Ѿ��ڳ��������ˣ�û�еĻ��ʹ���һ��
            if (!pools.ContainsKey(assetsList[i].assetsName))
            {
                pools.Add(assetsList[i].assetsName, new Queue<GameObject>());
                nameToPrefab.Add(assetsList[i].assetsName, assetsList[i].prefab);
                poolUseInfos.Add(assetsList[i].assetsName, new PoolUseInfo(assetsList[i].count, 0,assetsList[i].count));
            }
            //����assetsList�б��а������ظ���������
            else
            {
                poolUseInfos[assetsList[i].assetsName].totalCount += assetsList[i].count;
                poolUseInfos[assetsList[i].assetsName].initCount += assetsList[i].count;
            }
            //������Ϻ󣬱�������������������������5����ô�ʹ���5����Ȼ�����ֵ�
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

    //ֻ�����ȡһ�����󣬵�����Ҫ����������в�����������λ�û���ת��ʱʹ��
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

    //����������λ�ú���ת��Ȼ�������SpawnObject��������Ϊ��������Ѿ��Ի�ȡ�Ķ�������˲���������������Ҫ�����������
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

    //����������λ�á���ת���û���Ȼ�������SpawnObject������ͬ������Ϊ��������Ѿ��Ի�ȡ�Ķ�������˲���.
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

    //�ڶ�������Ѿ�û�и�����Ķ����ʱ�� ���ݶ����� Instantiate ��һ���µĶ����ҷ���ʹ�� ʵ�ֶ�̬����
    private void ExpansionPoolCapacity(string objectName)
    {
        //ÿ������1.5��������
        int expandCount = Mathf.CeilToInt(poolUseInfos[objectName].totalCount * 1.5f) - poolUseInfos[objectName].totalCount;
        if (expandCount == 0) expandCount = 1; //��ֹ��ʼ����Ϊ0ʱ�޷�����
        poolUseInfos[objectName].totalCount += expandCount;
        for (int i = 0; i < expandCount; i++)
        {
            var newGo = Instantiate(nameToPrefab[objectName][Random.Range(0, nameToPrefab[objectName].Length)], poolObjectParent, true);
            newGo.SetActive(false);
            pools[objectName].Enqueue(newGo);
        }
        
    }
    
    //���������й���Ķ��󣺼ȿ��Զ�ʱ���ã�Ҳ�����ֶ�����
    public void ClearExcessObjects()
    {
        StopAllCoroutines();
        StartCoroutine(ClearPools());
    }

    private IEnumerator ClearPools()
    {
        foreach (var poolInfo in poolUseInfos)
        {
            //��ĳ�ֶ���δʹ�õĶ��󳬹����ܶ�����Ŀ��һ��ʱ������������
            if (poolInfo.Value.NeedClearObject())
            {
                //ÿ�μ��ݵ�ԭ���������� 2/3 ��ȥ���� 1/3 �����Ķ���
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
            //ÿ֡����һ�ֶ��󣬷��ö�����ж���������࣬һ���Դ�����ɵĿ���
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
