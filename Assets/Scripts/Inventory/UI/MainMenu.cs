using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    // 实现"新的游戏"逻辑
    public void NewGame()
    {
        GlobalEvent.CallNewGameEvent();
        //SceneLoader.Instance.TransitionToRestScene();
    }
    
    // 实现"继续"逻辑
    public void ContinueGame()
    {
        GlobalEvent.CallContinueGameEvent();
    }
    
    // 实现"退出"逻辑
    public void QuitGame()
    {
        GlobalEvent.CallQuitGameEvent();
        
    }
}