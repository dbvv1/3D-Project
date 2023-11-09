using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : Singleton<GameManager>
{
    public PlayerCharacterStats playerCurrentStats;

    public HashSet<EnemyController> enemies = new();      //��¼���еĵ���

    public HashSet<EnemyController> weakEnemies=new();  //��¼���д�������״̬�ĵ��� 

    [SerializeField] private AssetReference gameConfigRef;

    [HideInInspector]public GameConfig gameConfig;

    protected override void Awake()
    {
        base.Awake();
        Addressables.LoadAssetAsync<GameConfig>(gameConfigRef).Completed += (handle) => gameConfig = handle.Result;
    }

    private void Start()
    {
        Application.targetFrameRate = 120;
    }

    #region �������е���
    public void RegisterEnemy(EnemyController enemy)
    {
        enemies.Add(enemy);
    }
    public void UnRegisterEnemy(EnemyController enemy)
    {
        enemies.Remove(enemy);
    }
    #endregion

    #region �������д�������״̬�ĵ���(����������д���)
    public void AddWeakEnemy(EnemyController enemy)
    {
        weakEnemies.Add(enemy);
    }
    public void RemoveWeakEnemy(EnemyController enemy)
    {
        weakEnemies.Remove(enemy);
    }
    #endregion

}
