using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

[DefaultExecutionOrder(-100)]
public class GameManager : Singleton<GameManager>
{
    public PlayerCharacterStats playerCurrentStats;
    
    public readonly HashSet<EnemyController> enemies = new();      //记录所有的敌人

    public readonly HashSet<EnemyController> weakEnemies=new();  //记录所有处于虚弱状态的敌人 

    public GameConfig gameConfig;
    
    private void Start()
    {
        Application.targetFrameRate = 80;
    }

    private void OnEnable()
    {
        GlobalEvent.stopTheWorldEvent += StopTheGame;
        GlobalEvent.continueTheWorldEvent += ContinueTheGame;
        GlobalEvent.quitGameEvent += OnQuitGame;
    }

    private void OnDisable()
    {
        GlobalEvent.stopTheWorldEvent -= StopTheGame;
        GlobalEvent.continueTheWorldEvent -= ContinueTheGame;
        GlobalEvent.quitGameEvent-=OnQuitGame;
    }

    private void OnQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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
