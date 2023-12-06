using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New UsableItemData", menuName = "Item/UsableItemData")]
public class UseableItemData_SO : ScriptableObject
{
    [SerializeField,JsonProperty] private StatsRecoveryEffect statsRecoveryEffect;

    [SerializeField,JsonProperty] private StatsIncreaseEffect statsIncreaseEffect;

    [SerializeField,JsonProperty] private AbilityIncreaseEffect abilityIncreaseEffect;
    
    [SerializeField,JsonProperty] private EventInvokeEffect eventInvokeEffect;

    public void OnUse()
    {
        statsRecoveryEffect.OnUse();
        statsIncreaseEffect.OnUse();
        abilityIncreaseEffect.OnUse();
        eventInvokeEffect.OnUse();
    }


    public void InitUsableItemData(UseableItemData_SO usableItemData)
    {
        statsRecoveryEffect.Reset(usableItemData.statsRecoveryEffect);
        statsIncreaseEffect.Reset(usableItemData.statsIncreaseEffect);
        abilityIncreaseEffect.Reset(usableItemData.abilityIncreaseEffect);
        eventInvokeEffect.Reset(usableItemData.eventInvokeEffect);
    }
    
    
}