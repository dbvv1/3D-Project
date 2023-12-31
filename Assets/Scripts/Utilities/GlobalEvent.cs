using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvent
{
    public static UnityAction newGameEvent;

    public static void CallNewGameEvent()
    {
        newGameEvent?.Invoke();
    }

    public static UnityAction continueGameEvent;
    public static void CallContinueGameEvent()
    {
        continueGameEvent?.Invoke();
    }

    public static UnityAction quitGameEvent;

    public static void CallQuitGameEvent()
    {
        quitGameEvent?.Invoke();
    }

    public static UnityAction enterMenuSceneEvent;

    public static void CallEnterMenuSceneEvent()
    {
        enterMenuSceneEvent?.Invoke();
    }
    
    public static UnityAction exitMenuSceneEvent;

    public static void CallExitMenuSceneEvent()
    {
        exitMenuSceneEvent?.Invoke();
    }
    
    //转向第一人称时的事件
    public static UnityAction switchToFirstPersonEvent;
    public static void CallSwitchToFirstPersonEvent()
    {
        switchToFirstPersonEvent?.Invoke();
    }
    
    //敌人死亡时的事件
    public static UnityAction<EnemyController> enemyDeathEvent;
    public static void CallOnEnemyDeath(EnemyController enemy)
    {
        enemyDeathEvent?.Invoke(enemy);
    }
    
    //加载场景完成后的事件
    public static UnityAction<Vector3> afterSceneLoadEvent;

    public static void CallAfterSceneLoadEvent(Vector3 pos)
    {
        afterSceneLoadEvent?.Invoke(pos);
    }
    
    //加载新场景之前的事件
    public static UnityAction beforeSceneLoadEvent;

    public static void CallBeforeSceneLoadEvent()
    {
        beforeSceneLoadEvent?.Invoke();
    }


    #region 聚焦于敌人时的标志显示事件

    public static UnityAction<EnemyController> enterFocusOnEnemy;
    public static void CallEnterFocusOnEnemy(EnemyController enemy)
    {
        enterFocusOnEnemy?.Invoke(enemy);
    }

    public static UnityAction exitFocusOnEnemy;
    public static void CallExitFocusOnEnemy()
    {
        exitFocusOnEnemy?.Invoke();
    }

    #endregion

    #region 敌人处决时的红点创建事件

    public static UnityAction<EnemyController> enemyEnterWeakState;
    public static void CallEnemyEnterWeakState(EnemyController enemy)
    {
        enemyEnterWeakState?.Invoke(enemy);
    }

    public static UnityAction<EnemyController> enemyExitWeakState;
    public static void CallEnemyExitWeakState(EnemyController enemy)
    {
        enemyExitWeakState?.Invoke(enemy);
    }
    
    #endregion


    #region 动画全局Behaviour

    //翻滚动画结束后的事件
    public static UnityAction rollAnimationOverEvent;
    public static void CallRollAnimationOverEvent()
    {
        rollAnimationOverEvent?.Invoke();
    }

    //落地动画结束后的事件
    public static UnityAction landAnimationOverEvent;
    public static void CallLandAnimationOverEvent()
    {
        landAnimationOverEvent?.Invoke();
    }
    
    #endregion

    //停止世界时间时的事件：设置TimeScale  停止人物等
    public static UnityAction stopTheWorldEvent;
    public static void CallStopTheWorldEvent()
    {
        stopTheWorldEvent?.Invoke();
    }
    
    public static UnityAction continueTheWorldEvent;
    public static void CallContinueTheWorldEvent()
    {
        continueTheWorldEvent?.Invoke();
    }
    
    //进入对话时触发的事件：停止人物  光标的显示
    public static UnityAction onEnterDialogue;
    public static void CallOnEnterDialogue()
    {
        onEnterDialogue?.Invoke();
    }
    
    //离开对话时触发的事件
    public static UnityAction onExitDialogue;
    public static void CallOnExitDialogue()
    {
        onExitDialogue?.Invoke();
    }
}
