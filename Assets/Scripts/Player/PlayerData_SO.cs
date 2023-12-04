using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Player Data",menuName ="Character/Player Data")]
public class PlayerData_SO : CharacterData_SO
{
    [Header("�����������")] 
    public int money;

    public int attributePoint;         //���Է�������Ե�
    
    public int skillPoint;             //���Է���ļ��ܵ�

    public override void InitCharacterData(CharacterData_SO characterDataSo)
    {
        base.InitCharacterData(characterDataSo);
        money = ((PlayerData_SO)characterDataSo).money;
        skillPoint = ((PlayerData_SO)characterDataSo).skillPoint;
        attributePoint = ((PlayerData_SO)characterDataSo).attributePoint;
    }
}
