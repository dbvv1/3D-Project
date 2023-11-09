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
}
