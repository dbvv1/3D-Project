using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyGenerator : MonoBehaviour
{
    [Header("����Ԥ����")] 
    [SerializeField] private List<EnemyController> enemyPrefabs;

    [Header("������������")] 
    
    [SerializeField] private float generateTime;   // ÿ������ȴʱ�䣨ɱ�����е��˺�
    
    [SerializeField] private int generateMinEnemyCount; // ÿ�����ɵ����ٵ���

    [SerializeField] private int generateMaxEnemyCount; // ÿ�����ɵ�������

    [SerializeField] private float generateEliteProbability; // ���ɾ�Ӣ���˵ĸ���

    [SerializeField] private float restTimeAfterBossFight;   // bossս��ĵ���ʱ��

    [Header("����")] 
    [SerializeField] private GameObject transitionPatrol; // ս�������Ĵ����ţ��������boss֮����һ��ʱ��
    
    private int generateTimes;          // ��ǰ�Ĳ�����ÿ10������һ��boss

    private bool isBossFightOver;       // �Ƿ�����Boss��������

    private bool isFightOver;           // �Ƿ�����һ��ս��������

    // ��ͬ�׼����˵����ɹ�����������ĳ���׼���������ɲ�ͬ�������
    private readonly List<EnemyFactory> normalEnemyFactories = new();
    private readonly List<EnemyFactory> eliteEnemyFactories = new();
    private readonly List<EnemyFactory> bossEnemyFactories = new();

    // �ֵ乤��������ָ������ĳ����ĵ���
    private readonly Dictionary<string, EnemyFactory> enemyNameToFactories = new();
    
    private float generateTimeCounter; // ������ȴʱ��ļ�ʱ��
    private float GenerateTimeCounter
    {
        get => generateTimeCounter;
        set
        {
            generateTimeCounter = value;
            if (value <= 0)
            {
                StartCoroutine(GenerateEnemy());
            }
        }
    }


    private void Awake()
    {
        foreach (var enemyPrefab in enemyPrefabs)
        {
            var factory = enemyPrefab.CreateFactory(enemyPrefab);
            switch (enemyPrefab.EnemyLevel)
            {
                case EnemyLevelType.Normal:
                    normalEnemyFactories.Add(factory);
                    break;
                case EnemyLevelType.Elite:
                    eliteEnemyFactories.Add(factory);
                    break;
                case EnemyLevelType.Boss:
                    bossEnemyFactories.Add(factory);
                    break;
            }
            enemyNameToFactories.Add(enemyPrefab.EnemyName, factory);
        }
    }

    private void Start()
    {
        generateTimes = 0;
        GenerateTimeCounter = generateTime;
    }

    private void OnEnable()
    {
        GlobalEvent.enemyDeathEvent += CheckFightOver;
    }

    private void OnDisable()
    {
        GlobalEvent.enemyDeathEvent -= CheckFightOver;
    }

    private void CheckFightOver(EnemyController enemyController)
    {
        isFightOver = GameManager.Instance.enemies.Count == 0;
        isBossFightOver = isFightOver & (generateTimes % 10 == 0);
        if (isBossFightOver)
        {
            //�������
            transitionPatrol.gameObject.SetActive(true);
            generateTimeCounter = restTimeAfterBossFight;
        }
    }

    private void Update()
    {
        // ��ʱ����ʱ�Ĺ���1����ǰ��ս������ 2����������Ϊ0 3����ǰ�������ڼ��س���
        if (SceneLoader.Instance.GetCurrentSceneType == SceneType.FightScene && GameManager.Instance.enemies.Count == 0&& !SceneLoader.Instance.IsLoading)
            GenerateTimeCounter -= Time.deltaTime;

        // �����boss����������E�� �����̽�����һ��
        if (isBossFightOver&&Input.GetKeyDown(KeyCode.E))
        {
            GenerateTimeCounter = 0;
        }
        
    }


    private IEnumerator GenerateEnemy()
    {
        ++generateTimes;
        transitionPatrol.gameObject.SetActive(false);
        // ����boss�����
        if (generateTimes % 10 == 0)
        {
            CreateRandomBossEnemy();
            yield break;
        }
        // ������ͨ/��Ӣ����
        int generateEnemyCount = Random.Range(generateMinEnemyCount, generateMaxEnemyCount);
        for (int i = 0; i < generateEnemyCount; i++)
        {
            float random = Random.value;
            if (random < generateEliteProbability)
            {
                CreateRandomEliteEnemy();
            }
            else
            {
                CreateRandomNormalEnemy();
            }

            yield return new WaitForSeconds(1f);
        }
    }


    private void CreateRandomNormalEnemy()
    {
        var enemy = normalEnemyFactories[Random.Range(0, normalEnemyFactories.Count)].CreateEnemy();
    }

    private void CreateRandomEliteEnemy()
    {
        var enemy = eliteEnemyFactories[Random.Range(0, eliteEnemyFactories.Count)].CreateEnemy();
    }

    private void CreateRandomBossEnemy()
    {
        // TODO����ʱû�����boss 
        var enemy = eliteEnemyFactories[Random.Range(0, eliteEnemyFactories.Count)].CreateEnemy();
        // var enemy = bossEnemyFactories[Random.Range(0, bossEnemyFactories.Count)].CreateEnemy();
    }
}