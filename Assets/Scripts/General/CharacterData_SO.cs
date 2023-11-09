using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character/Character Data")] 
public class CharacterData_SO : ScriptableObject
{
    [Header("基本属性")]
    public float maxHealth;            //最大生命值

    public float maxEnergy;            //最大能量值

    public float maxMagic;             //最大魔法值

    public float baseHealthRecover;    //基本生命回复  （value/秒）

    public float baseEnergyRecover;    //基本能量回复

    public float baseMagicRecover;     //基本魔法回复

    public float basePhysicalDamage;   //基础物理伤害

    public float baseSkillDamage;      //基础法术伤害

    public float basePhysicalDefensive;//基础物理防御

    public float baseMagicalDefensive; //基础魔法防御

    [Header("基础三围属性")]
    public int basePowerPoint;         //力量点数 影响最大生命，物理伤害

    public int baseAgilityPoint;       //敏捷点数 影响最大能量，能量恢复

    public int baseIntelligencePoint;  //智力点数 影响最大魔法，法术伤害

    [Header("升级信息")]
    public float baseExp;                //升级所需的基础经验值

    public float levelBuf;               //升级的整体加成

    [Header("特殊属性")]
    public float invincibleTimeAfterHit;//受伤后的无敌时间

}
