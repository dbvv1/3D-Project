using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Player Data",menuName ="Character/Player Data")]
public class PlayerData_SO : CharacterData_SO
{
    [Header("主角相关属性")] 
    public int money;

    public override void InitCharacterData(CharacterData_SO characterDataSo)
    {
        base.InitCharacterData(characterDataSo);
        money = ((PlayerData_SO)characterDataSo).money;
    }
}
