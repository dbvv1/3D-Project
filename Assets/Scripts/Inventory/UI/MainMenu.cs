using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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