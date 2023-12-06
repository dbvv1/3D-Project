using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

// ʵ������ ʹ�� ���� ScriptableObject �ĳ������ ��ʵ�ֲ�ͬ��Ч��
// public abstract class UseEffect:ScriptableObject
// {
//     public abstract void OnUse(){}
// }

// ʵ�ֶ��� ʹ�û��� �ӿ� ��ʵ�ֲ�ͬЧ��
public interface IUseEffect
{
    public void OnUse();
}

[System.Serializable]
public class StatsRecoveryEffect : IUseEffect
{
    [Header("�ظ�����")] 
    [SerializeField, JsonProperty] private float healthRecovery;

    [SerializeField, JsonProperty] private float energyRecovery;

    [SerializeField, JsonProperty] private float magicRecovery;

    public StatsRecoveryEffect(float healthRecovery, float energyRecovery, float magicRecovery)
    {
        this.healthRecovery = healthRecovery;
        this.energyRecovery = energyRecovery;
        this.magicRecovery = magicRecovery;
    }
    
    public void Reset(StatsRecoveryEffect other)
    {
        healthRecovery = other.healthRecovery;
        energyRecovery = other.energyRecovery;
        magicRecovery = other.magicRecovery;
    }
    
    public void OnUse()
    {
        UsableItemGlobalEvent.CallUseRecoveryItemEvent(healthRecovery, energyRecovery, magicRecovery);
    }
    

}

[System.Serializable]
public class StatsIncreaseEffect : IUseEffect
{
    [Header("״̬���Ա仯")] 
    [SerializeField, JsonProperty] private float healthLimitIncrease;

    [SerializeField, JsonProperty] private float energyLimitIncrease;

    [SerializeField, JsonProperty] private float magicLimitIncrease;
    
    public StatsIncreaseEffect(float healthLimitIncrease, float energyLimitIncrease, float magicLimitIncrease)
    {
        this.healthLimitIncrease = healthLimitIncrease;
        this.energyLimitIncrease = energyLimitIncrease;
        this.magicLimitIncrease = magicLimitIncrease;
    }
    
    public void Reset(StatsIncreaseEffect other)
    {
        healthLimitIncrease = other.healthLimitIncrease;
        energyLimitIncrease = other.energyLimitIncrease;
        magicLimitIncrease = other.magicLimitIncrease;
    }

    public void OnUse()
    {
        UsableItemGlobalEvent.CallUseStatsIncreaseItemEvent(healthLimitIncrease,energyLimitIncrease,magicLimitIncrease);
    }

}

[System.Serializable]
public class AbilityIncreaseEffect : IUseEffect
{
    [Header("�������Ա仯")]
    [SerializeField, JsonProperty] private float physicalAttackIncrease;
    
    [SerializeField, JsonProperty] private float magicAttackIncrease;
    
    [SerializeField, JsonProperty] private float physicalDefensiveIncrease;
    
    [SerializeField, JsonProperty] private float magicDefensiveIncrease;

    public AbilityIncreaseEffect(float physicalAttackIncrease, float magicAttackIncrease,
        float physicalDefensiveIncrease, float magicDefensiveIncrease)
    {
        this.physicalAttackIncrease = physicalAttackIncrease;
        this.magicAttackIncrease = magicAttackIncrease;
        this.magicDefensiveIncrease = magicDefensiveIncrease;
        this.magicDefensiveIncrease =magicDefensiveIncrease;
    }

    public void Reset(AbilityIncreaseEffect other)
    {
        physicalAttackIncrease = other.physicalAttackIncrease;
        magicAttackIncrease = other.magicAttackIncrease;
        magicDefensiveIncrease = other.magicDefensiveIncrease;
        magicDefensiveIncrease = other.magicDefensiveIncrease;
    }
    
    public void OnUse()
    {
        UsableItemGlobalEvent.CallUseAttackIncreaseItemEvent(physicalAttackIncrease,magicAttackIncrease,physicalDefensiveIncrease,magicDefensiveIncrease);
    }

}

[System.Serializable]
public class EventInvokeEffect : IUseEffect
{
    [Header("��Ʒʹ�ô�����һЩ�����¼�")] 
    [SerializeField, JsonProperty] private UnityEvent usableItemEvent;

    public EventInvokeEffect(UnityEvent usableItemEvent)
    {
        this.usableItemEvent = usableItemEvent;
    }
    
    public void Reset(EventInvokeEffect other)
    {
        usableItemEvent = other.usableItemEvent;
    }
    
    public void OnUse()
    {
        usableItemEvent?.Invoke();
    }
}
