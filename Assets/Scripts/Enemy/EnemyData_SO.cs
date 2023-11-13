using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Character/Enemy Data")]
public class EnemyData_SO : CharacterData_SO
{
    [Header("µĞÈË¹¥»÷ÅĞ¶¨")]
    public float attackDistanceNear;       //¶Ì¾àÀë
    public float attackDistanceFar;        //³¤¾àÀë
    public float attackStateDistance;      //½øÈë¹¥»÷×´Ì¬µÄ¾àÀë


    public override void InitCharacterData(CharacterData_SO characterDataSo)
    {
        base.InitCharacterData(characterDataSo);
        this.attackDistanceFar = ((EnemyData_SO)characterDataSo).attackDistanceFar;
        this.attackDistanceNear =  ((EnemyData_SO)characterDataSo).attackDistanceNear;
        this.attackStateDistance =  ((EnemyData_SO)characterDataSo).attackStateDistance;
    }
}
