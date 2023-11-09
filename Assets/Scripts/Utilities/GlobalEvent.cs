using UnityEngine.Events;

public static class GlobalEvent 
{
    //转向第一人称时的事件
    public static UnityAction SwitchToFirstPersonEvent;
    public static void CallSwitchToFirstPersonEvent()
    {
        SwitchToFirstPersonEvent?.Invoke();
    }

    public static UnityAction<EnemyController> EnemyDeath;
    public static void CallOnEnemyDeath(EnemyController enemy)
    {
        EnemyDeath?.Invoke(enemy);
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

}
