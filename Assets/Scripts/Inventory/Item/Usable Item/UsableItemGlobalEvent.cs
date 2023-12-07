using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class UsableItemGlobalEvent
{
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
    public static UnityAction<float,float,float,float> useAbilityIncreaseItemEvent;
    public static void CallUseAttackIncreaseItemEvent(float physicalAttackIncrease,float magicAttackIncrease,float physicalDefensiveIncrease,float magicDefensiveIncrease)
    {
        useAbilityIncreaseItemEvent?.Invoke(physicalAttackIncrease,magicAttackIncrease,physicalDefensiveIncrease,magicDefensiveIncrease);
    }

    #endregion
}
