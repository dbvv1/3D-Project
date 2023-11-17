using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvent 
{
    //转向第一人称时的事件
    public static UnityAction SwitchToFirstPersonEvent;
    public static void CallSwitchToFirstPersonEvent()
    {
        SwitchToFirstPersonEvent?.Invoke();
    }
    
    //敌人死亡时的事件
    public static UnityAction<EnemyController> EnemyDeathEvent;
    public static void CallOnEnemyDeath(EnemyController enemy)
    {
        EnemyDeathEvent?.Invoke(enemy);
    }
    
    //加载场景完成后的事件
    public static UnityAction<Vector3> AfterSceneLoadEvent;

    public static void CallAfterSceneLoadEvent(Vector3 pos)
    {
        AfterSceneLoadEvent?.Invoke(pos);
    }
    
    //加载新场景之前的事件
    public static UnityAction BeforeSceneLoadEvent;

    public static void CallBeforeSceneLoadEvent()
    {
        BeforeSceneLoadEvent?.Invoke();
    }


    #region 聚焦于敌人时的标志显示事件

    public static UnityAction<EnemyController> EnterFocusOnEnemy;
    public static void CallEnterFocusOnEnemy(EnemyController enemy)
    {
        EnterFocusOnEnemy?.Invoke(enemy);
    }

    public static UnityAction ExitFocusOnEnemy;
    public static void CallExitFocusOnEnemy()
    {
        ExitFocusOnEnemy?.Invoke();
    }

    #endregion

    #region 敌人处决时的红点创建事件

    public static UnityAction<EnemyController> EnemyEnterWeakState;
    public static void CallEnemyEnterWeakState(EnemyController enemy)
    {
        EnemyEnterWeakState?.Invoke(enemy);
    }

    public static UnityAction<EnemyController> EnemyExitWeakState;
    public static void CallEnemyExitWeakState(EnemyController enemy)
    {
        EnemyExitWeakState?.Invoke(enemy);
    }
    
    #endregion


    #region 动画全局Behaviour

    //翻滚动画结束后的事件
    public static UnityAction RollAnimationOverEvent;
    public static void CallRollAnimationOverEvent()
    {
        RollAnimationOverEvent?.Invoke();
    }

    //落地动画结束后的事件
    public static UnityAction LandAnimationOverEvent;
    public static void CallLandAnimationOverEvent()
    {
        LandAnimationOverEvent?.Invoke();
    }
    
    #endregion

    #region 物品使用造成的事件

    public static UnityAction<float, float, float> UseRecoveryItemEvent;
    
    public static void CallUseRecoveryItemEvent(float health, float energy, float magic)
    {
        UseRecoveryItemEvent?.Invoke(health, energy, magic);
    }

    public static UnityAction<float, float, float> UseStatsIncreaseItemEvent;
    
    public static void CallUseStatsIncreaseItemEvent(float health, float energy, float magic)
    {
        UseStatsIncreaseItemEvent?.Invoke(health, energy, magic);
    }

    public static UnityAction<float> UseAttackIncreaseItemEvent;
    
    public static void CallUseAttackIncreaseItemEvent(float attack)
    {
        UseAttackIncreaseItemEvent?.Invoke(attack);
    }

    #endregion

    public static UnityAction StopTheWorldEvent;

    public static void CallStopTheWorldEvent()
    {
        StopTheWorldEvent?.Invoke();
    }
    
    public static UnityAction ContinueTheWorldEvent;

    public static void CallContinueTheWorldEvent()
    {
        ContinueTheWorldEvent?.Invoke();
    }

}
