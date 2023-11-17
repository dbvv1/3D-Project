using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "New UsableItemData", menuName = "Item/UsableItemData")]
public class UseableItemData_SO : ScriptableObject
{
    [Header("回复功能")] 
    [SerializeField, JsonProperty] private float healthRecovery;

    [SerializeField, JsonProperty] private float energyRecovery;

    [SerializeField, JsonProperty] private float magicRecovery;

    [Header("属性变化")] 
    [SerializeField, JsonProperty] private float healthLimitIncrease;

    [SerializeField, JsonProperty] private float energyLimitIncrease;

    [SerializeField, JsonProperty] private float magicLimitIncrease;

    [SerializeField, JsonProperty] private float baseAttackIncrease;


    public void OnUse()
    {
        GlobalEvent.CallUseRecoveryItemEvent(healthRecovery, energyRecovery, magicRecovery);
        GlobalEvent.CallUseStatsIncreaseItemEvent(healthLimitIncrease, energyLimitIncrease, magicLimitIncrease);
        GlobalEvent.CallUseAttackIncreaseItemEvent(baseAttackIncrease);
    }

    public void InitUsableItemData(UseableItemData_SO usableItemData)
    {
        healthRecovery = usableItemData.healthRecovery;
        energyRecovery = usableItemData.energyRecovery;
        magicRecovery = usableItemData.magicRecovery;
        healthLimitIncrease = usableItemData.healthLimitIncrease;
        energyLimitIncrease = usableItemData.energyLimitIncrease;
        magicLimitIncrease = usableItemData.magicLimitIncrease;
        baseAttackIncrease = usableItemData.baseAttackIncrease;
    }
    
}