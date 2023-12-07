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
    
    public readonly HashSet<EnemyController> enemies = new();      //��¼���еĵ���

    public readonly HashSet<EnemyController> weakEnemies=new();  //��¼���д�������״̬�ĵ��� 

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
