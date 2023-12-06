using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New UsableItemData", menuName = "Item/UsableItemData")]
public class UseableItemData_SO : ScriptableObject
{
    // 另一种方法：让UseEffect成为 ScriptableObject 类型的抽象基类，然后就可以通过创建List 在Unity的编辑器窗口中动态添加效果
    // 优点是实现了动态添加 效率更高   缺点在于对于每一种不同的效果（数值不同也要） 都要创建一个与之对应的 ScriptableObject，比较麻烦
    //[SerializeField,JsonProperty] private List<UseEffect> useEffects = new List<UseEffect>();

    [SerializeField] public List<IUseEffect> useEffects = new List<IUseEffect>();
    
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