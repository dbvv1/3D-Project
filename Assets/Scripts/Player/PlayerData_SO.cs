using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Player Data",menuName ="Character/Player Data")]
public class PlayerData_SO : CharacterData_SO
{
    [Header("主角相关属性")] 
    public int money;

    public int attributePoint;         //可以分配的属性点
    
    public int skillPoint;             //可以分配的技能点

    public override void InitCharacterData(CharacterData_SO characterDataSo)
    {
        base.InitCharacterData(characterDataSo);
        money = ((PlayerData_SO)characterDataSo).money;
        skillPoint = ((PlayerData_SO)characterDataSo).skillPoint;
        attributePoint = ((PlayerData_SO)characterDataSo).attributePoint;
    }
}
