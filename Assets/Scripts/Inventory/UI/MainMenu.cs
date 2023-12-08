using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    // ʵ��"�µ���Ϸ"�߼�
    public void NewGame()
    {
        GlobalEvent.CallNewGameEvent();
        //SceneLoader.Instance.TransitionToRestScene();
    }
    
    // ʵ��"����"�߼�
    public void ContinueGame()
    {
        GlobalEvent.CallContinueGameEvent();
    }
    
    // ʵ��"�˳�"�߼�
    public void QuitGame()
    {
        GlobalEvent.CallQuitGameEvent();
        
    }
}