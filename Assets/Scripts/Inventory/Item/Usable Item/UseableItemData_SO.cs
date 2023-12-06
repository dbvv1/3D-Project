using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New UsableItemData", menuName = "Item/UsableItemData")]
public class UseableItemData_SO : ScriptableObject
{
    // ��һ�ַ�������UseEffect��Ϊ ScriptableObject ���͵ĳ�����࣬Ȼ��Ϳ���ͨ������List ��Unity�ı༭�������ж�̬���Ч��
    // �ŵ���ʵ���˶�̬��� Ч�ʸ���   ȱ�����ڶ���ÿһ�ֲ�ͬ��Ч������ֵ��ͬҲҪ�� ��Ҫ����һ����֮��Ӧ�� ScriptableObject���Ƚ��鷳
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