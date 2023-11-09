using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�򵥶����
public class GOPoolManager : Singleton<GOPoolManager>
{
    [SerializeField, Header("Ԥ����")] private List<GameObjectAssets> assetsList = new List<GameObjectAssets>();
    [SerializeField] private Transform poolObjectParent;

    //���ֶ�Ӧ�����Ԥ����  ���ڶ���صĶ�̬����
    private Dictionary<string, GameObject[]> nameToPrefab = new();

    //ʵ�ʵĶ����
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

        //�����������õ���Դ ���г�ʼ��
        for (int i = 0; i < assetsList.Count; i++)
        {
            //����б��һ��Ԫ�ص������Ƿ��Ѿ��ڳ��������ˣ�û�еĻ��ʹ���һ��
            if (!pools.ContainsKey(assetsList[i].assetsName))
            {
                pools.Add(assetsList[i].assetsName, new Queue<GameObject>());
                nameToPrefab.Add(assetsList[i].assetsName, assetsList[i].prefab);
                if (assetsList[i].prefab.Length == 0) return;

                //������Ϻ󣬱�������������������������5����ô�ʹ���5����Ȼ�����ֵ�
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

    //ֻ�����ȡһ�����󣬵�����Ҫ����������в�����������λ�û���ת��ʱʹ��
    public GameObject TakeGameObject(string objectName)
    {
        if (!pools.ContainsKey(objectName)) return null;

        if (pools[objectName].Count == 0)
            ExpansionPoolCapacity(objectName);

        GameObject dequeueObject = pools[objectName].Dequeue();
        dequeueObject.SetActive(true);
        return dequeueObject;
    }

    //����������λ�ú���ת��Ȼ�������SpawnObject��������Ϊ��������Ѿ��Ի�ȡ�Ķ�������˲���������������Ҫ�����������
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

    //����������λ�á���ת���û���Ȼ�������SpawnObject������ͬ������Ϊ��������Ѿ��Ի�ȡ�Ķ�������˲���.
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

    //�ڶ�������Ѿ�û�и�����Ķ����ʱ�� ���ݶ����� Instantiate ��һ���µĶ����ҷ���ʹ�� ʵ�ֶ�̬����
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
