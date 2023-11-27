using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[DefaultExecutionOrder(-100)]
public class GameManager : Singleton<GameManager>
{
    public PlayerCharacterStats playerCurrentStats;
    
    public HashSet<EnemyController> enemies = new();      //��¼���еĵ���

    public HashSet<EnemyController> weakEnemies=new();  //��¼���д�������״̬�ĵ��� 

    public GameConfig gameConfig;


    private void Start()
    {
        Application.targetFrameRate = 120;
        
    }

    private void OnEnable()
    {
        GlobalEvent.stopTheWorldEvent += StopTheGame;
        GlobalEvent.continueTheWorldEvent += ContinueTheGame;
    }

    private void OnDisable()
    {
        GlobalEvent.stopTheWorldEvent -= StopTheGame;
        GlobalEvent.continueTheWorldEvent -= ContinueTheGame;
    }

    private void StopTheGame()
    {
        Time.timeScale = 0;
    }

    private void ContinueTheGame()
    {
        Time.timeScale = 1;
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
