using UnityEngine.Events;

public static class GlobalEvent 
{
    //ת���һ�˳�ʱ���¼�
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


    #region �۽��ڵ���ʱ�ı�־��ʾ�¼�

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

    #region ���˴���ʱ�ĺ�㴴���¼�

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


    #region ����ȫ��Behaviour

    //����������������¼�
    public static UnityAction RollAnimationOverEvent;
    public static void CallRollAnimationOverEvent()
    {
        RollAnimationOverEvent?.Invoke();
    }

    //��ض�����������¼�
    public static UnityAction LandAnimationOverEvent;
    public static void CallLandAnimationOverEvent()
    {
        LandAnimationOverEvent?.Invoke();
    }
    #endregion

}
