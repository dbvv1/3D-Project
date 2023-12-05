using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyGenerator : MonoBehaviour
{
    [Header("敌人预制体")] 
    [SerializeField] private List<EnemyController> enemyPrefabs;

    [Header("敌人生成设置")] 
    
    [SerializeField] private float generateTime;   // 每波的冷却时间（杀死所有敌人后）
    
    [SerializeField] private int generateMinEnemyCount; // 每波生成的最少敌人

    [SerializeField] private int generateMaxEnemyCount; // 每波生成的最大敌人

    [SerializeField] private float generateEliteProbability; // 生成精英敌人的概率

    [SerializeField] private float restTimeAfterBossFight;   // boss战后的调整时间

    [Header("引用")] 
    [SerializeField] private GameObject transitionPatrol; // 战斗场景的传送门：规则打完boss之后会打开一段时间
    
    private int generateTimes;          // 当前的波数：每10波生成一个boss

    private bool isBossFightOver;       // 是否是在Boss波结束后

    private bool isFightOver;           // 是否是在一波战斗结束后

    // 不同阶级敌人的生成工厂：用于在某个阶级上随机生成不同种类敌人
    private readonly List<EnemyFactory> normalEnemyFactories = new();
    private readonly List<EnemyFactory> eliteEnemyFactories = new();
    private readonly List<EnemyFactory> bossEnemyFactories = new();

    // 字典工厂：可以指定生成某种类的敌人
    private readonly Dictionary<string, EnemyFactory> enemyNameToFactories = new();
    
    private float generateTimeCounter; // 生成冷却时间的计时器
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
            //激活传送门
            transitionPatrol.gameObject.SetActive(true);
            generateTimeCounter = restTimeAfterBossFight;
        }
    }

    private void Update()
    {
        // 计时器计时的规则：1：当前是战斗场景 2：敌人数量为0 3：当前不是正在加载场景
        if (SceneLoader.Instance.GetCurrentSceneType == SceneType.FightScene && GameManager.Instance.enemies.Count == 0&& !SceneLoader.Instance.IsLoading)
            GenerateTimeCounter -= Time.deltaTime;

        // 如果在boss波结束后按下E键 则立刻进入下一波
        if (isBossFightOver&&Input.GetKeyDown(KeyCode.E))
        {
            GenerateTimeCounter = 0;
        }
        
    }


    private IEnumerator GenerateEnemy()
    {
        ++generateTimes;
        transitionPatrol.gameObject.SetActive(false);
        // 生成boss类敌人
        if (generateTimes % 10 == 0)
        {
            CreateRandomBossEnemy();
            yield break;
        }
        // 生成普通/精英敌人
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
        // TODO：暂时没有设计boss 
        var enemy = eliteEnemyFactories[Random.Range(0, eliteEnemyFactories.Count)].CreateEnemy();
        // var enemy = bossEnemyFactories[Random.Range(0, bossEnemyFactories.Count)].CreateEnemy();
    }
}