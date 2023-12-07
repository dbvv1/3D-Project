using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class UsableItemGlobalEvent
{
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
    public static UnityAction<float,float,float,float> useAbilityIncreaseItemEvent;
    public static void CallUseAttackIncreaseItemEvent(float physicalAttackIncrease,float magicAttackIncrease,float physicalDefensiveIncrease,float magicDefensiveIncrease)
    {
        useAbilityIncreaseItemEvent?.Invoke(physicalAttackIncrease,magicAttackIncrease,physicalDefensiveIncrease,magicDefensiveIncrease);
    }

    #endregion
}
