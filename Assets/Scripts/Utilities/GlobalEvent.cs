using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvent 
{
    //ת���һ�˳�ʱ���¼�
    public static UnityAction switchToFirstPersonEvent;
    public static void CallSwitchToFirstPersonEvent()
    {
        switchToFirstPersonEvent?.Invoke();
    }
    
    //��������ʱ���¼�
    public static UnityAction<EnemyController> enemyDeathEvent;
    public static void CallOnEnemyDeath(EnemyController enemy)
    {
        enemyDeathEvent?.Invoke(enemy);
    }
    
    //���س�����ɺ���¼�
    public static UnityAction<Vector3> afterSceneLoadEvent;

    public static void CallAfterSceneLoadEvent(Vector3 pos)
    {
        afterSceneLoadEvent?.Invoke(pos);
    }
    
    //�����³���֮ǰ���¼�
    public static UnityAction beforeSceneLoadEvent;

    public static void CallBeforeSceneLoadEvent()
    {
        beforeSceneLoadEvent?.Invoke();
    }


    #region �۽��ڵ���ʱ�ı�־��ʾ�¼�

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

    #region ���˴���ʱ�ĺ�㴴���¼�

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


    #region ����ȫ��Behaviour

    //����������������¼�
    public static UnityAction rollAnimationOverEvent;
    public static void CallRollAnimationOverEvent()
    {
        rollAnimationOverEvent?.Invoke();
    }

    //��ض�����������¼�
    public static UnityAction landAnimationOverEvent;
    public static void CallLandAnimationOverEvent()
    {
        landAnimationOverEvent?.Invoke();
    }
    
    #endregion

    #region ��Ʒʹ����ɵ��¼�

    //ʹ�ûָ�����״̬����Ʒ�¼�
    public static UnityAction<float, float, float> useRecoveryItemEvent;
    public static void CallUseRecoveryItemEvent(float health, float energy, float magic)
    {
        useRecoveryItemEvent?.Invoke(health, energy, magic);
    }

    //ʹ����ǿ����״̬���Ե���Ʒ�¼�
    public static UnityAction<float, float, float> useStatsIncreaseItemEvent;
    public static void CallUseStatsIncreaseItemEvent(float health, float energy, float magic)
    {
        useStatsIncreaseItemEvent?.Invoke(health, energy, magic);
    }

    //ʹ����ǿ��������Ʒ���¼�
    public static UnityAction<float> useAttackIncreaseItemEvent;
    public static void CallUseAttackIncreaseItemEvent(float attack)
    {
        useAttackIncreaseItemEvent?.Invoke(attack);
    }

    #endregion

    //ֹͣ����ʱ��ʱ���¼�������TimeScale  ֹͣ�����
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
    
    //����Ի�ʱ�������¼���ֹͣ����  ������ʾ
    public static UnityAction onEnterDialogue;
    public static void CallOnEnterDialogue()
    {
        onEnterDialogue?.Invoke();
    }
    
    //�뿪�Ի�ʱ�������¼�
    public static UnityAction onExitDialogue;
    public static void CallOnExitDialogue()
    {
        onExitDialogue?.Invoke();
    }
}
