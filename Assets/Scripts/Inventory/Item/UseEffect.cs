using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public interface UseEffect
{
    public void OnUse();
}

[System.Serializable]
public class StatsRecoveryEffect : UseEffect
{
    [Header("回复功能")] 
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
        GlobalEvent.CallUseRecoveryItemEvent(healthRecovery, energyRecovery, magicRecovery);
    }
    

}

[System.Serializable]
public class StatsIncreaseEffect : UseEffect
{
    [Header("状态属性变化")] 
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
        GlobalEvent.CallUseStatsIncreaseItemEvent(healthLimitIncrease,energyLimitIncrease,magicLimitIncrease);
    }

}

[System.Serializable]
public class AbilityIncreaseEffect : UseEffect
{
    [Header("能力属性变化")]
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
        GlobalEvent.CallUseAttackIncreaseItemEvent(physicalAttackIncrease,magicAttackIncrease,physicalDefensiveIncrease,magicDefensiveIncrease);
    }

}

[System.Serializable]
public class EventInvokeEffect : UseEffect
{
    [Header("物品使用触发的一些特殊事件")] 
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
