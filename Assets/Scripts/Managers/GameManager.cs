using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[DefaultExecutionOrder(-100)]
public class GameManager : Singleton<GameManager>
{
    public PlayerCharacterStats playerCurrentStats;
    
    public HashSet<EnemyController> enemies = new();      //记录所有的敌人

    public HashSet<EnemyController> weakEnemies=new();  //记录所有处于虚弱状态的敌人 

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
    
    


    #region 管理所有敌人
    public void RegisterEnemy(EnemyController enemy)
    {
        enemies.Add(enemy);
    }
    public void UnRegisterEnemy(EnemyController enemy)
    {
        enemies.Remove(enemy);
    }
    #endregion

    #region 管理所有处于虚弱状态的敌人(用于人物进行处决)
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
