using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvent 
{
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

    #region 物品使用造成的事件

    //使用恢复人物状态的物品事件
    public static UnityAction<float, float, float> useRecoveryItemEvent;
    public static void CallUseRecoveryItemEvent(float health, float energy, float magic)
    {
        useRecoveryItemEvent?.Invoke(health, energy, magic);
    }

    //使用增强人物状态属性的物品事件
    public static UnityAction<float, float, float> useStatsIncreaseItemEvent;
    public static void CallUseStatsIncreaseItemEvent(float health, float energy, float magic)
    {
        useStatsIncreaseItemEvent?.Invoke(health, energy, magic);
    }

    //使用增强攻击的物品的事件
    public static UnityAction<float> useAttackIncreaseItemEvent;
    public static void CallUseAttackIncreaseItemEvent(float attack)
    {
        useAttackIncreaseItemEvent?.Invoke(attack);
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
