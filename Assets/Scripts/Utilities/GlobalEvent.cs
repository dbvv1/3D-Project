using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvent 
{
    //ת���һ�˳�ʱ���¼�
    public static UnityAction SwitchToFirstPersonEvent;
    public static void CallSwitchToFirstPersonEvent()
    {
        SwitchToFirstPersonEvent?.Invoke();
    }
    
    //��������ʱ���¼�
    public static UnityAction<EnemyController> EnemyDeathEvent;
    public static void CallOnEnemyDeath(EnemyController enemy)
    {
        EnemyDeathEvent?.Invoke(enemy);
    }
    
    //���س�����ɺ���¼�
    public static UnityAction<Vector3> AfterSceneLoadEvent;

    public static void CallAfterSceneLoadEvent(Vector3 pos)
    {
        AfterSceneLoadEvent?.Invoke(pos);
    }
    
    //�����³���֮ǰ���¼�
    public static UnityAction BeforeSceneLoadEvent;

    public static void CallBeforeSceneLoadEvent()
    {
        BeforeSceneLoadEvent?.Invoke();
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

    #region ��Ʒʹ����ɵ��¼�

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
