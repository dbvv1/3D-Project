using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character/Character Data")] 
public class CharacterData_SO : ScriptableObject
{
    [Header("基本属性")]
    public float maxHealth;            //最大生命值

    public float curHealth;

    public float maxEnergy;            //最大能量值

    public float curEnergy;

    public float maxMagic;             //最大魔法值
    
    public float curMagic;

    public float baseHealthRecover;    //基本生命回复  （value/秒）

    public float curHealthRecover;

    public float baseEnergyRecover;    //基本能量回复
    
    public float curEnergyRecover;

    public float baseMagicRecover;     //基本魔法回复
    
    public float curMagicRecover;

    public float basePhysicalDamage;   //基础物理伤害
    
    public float curPhysicalDamage;

    public float baseSkillDamage;      //基础法术伤害
    
    public float curSkillDamage;

    public float basePhysicalDefensive;//基础物理防御
    
    public float curPhysicalDefensive;

    public float baseMagicalDefensive; //基础魔法防御

    public float curMagicalDefensive;
    
    [Header("基础三围属性")]
    public int basePowerPoint;         //力量点数 影响最大生命，物理伤害
    
    public int curPowerPoint;

    public int baseAgilityPoint;       //敏捷点数 影响最大能量，能量恢复
    
    public int curAgilityPoint;

    public int baseIntelligencePoint;  //智力点数 影响最大魔法，法术伤害
    
    public int curIntelligencePoint;

    [Header("升级信息")]
    public float baseExp;                //升级所需的基础经验值

    public int curLevel;
    
    public float curNeedExp;

    public float curExp;

    public float levelBuf;               //升级的整体加成
    
    
    [Header("特殊属性")]
    public float invincibleTimeAfterHit;//受伤后的无敌时间

    
    public virtual void InitCharacterData(float maxHealth, float curHealth, float maxEnergy, float curEnergy, float maxMagic, float curMagic, float baseHealthRecover, float curHealthRecover, float baseEnergyRecover, float curEnergyRecover, float baseMagicRecover, float curMagicRecover, float basePhysicalDamage, float curPhysicalDamage, float baseSkillDamage, float curSkillDamage, float basePhysicalDefensive, float curPhysicalDefensive, float baseMagicalDefensive, float curMagicalDefensive, int basePowerPoint, int curPowerPoint, int baseAgilityPoint, int curAgilityPoint, int baseIntelligencePoint, int curIntelligencePoint, float baseExp, int curLevel, float curNeedExp, float curExp, float levelBuf, float invincibleTimeAfterHit)
    {
        this.maxHealth = maxHealth;
        this.curHealth = curHealth;
        this.maxEnergy = maxEnergy;
        this.curEnergy = curEnergy;
        this.maxMagic = maxMagic;
        this.curMagic = curMagic;
        this.baseHealthRecover = baseHealthRecover;
        this.curHealthRecover = curHealthRecover;
        this.baseEnergyRecover = baseEnergyRecover;
        this.curEnergyRecover = curEnergyRecover;
        this.baseMagicRecover = baseMagicRecover;
        this.curMagicRecover = curMagicRecover;
        this.basePhysicalDamage = basePhysicalDamage;
        this.curPhysicalDamage = curPhysicalDamage;
        this.baseSkillDamage = baseSkillDamage;
        this.curSkillDamage = curSkillDamage;
        this.basePhysicalDefensive = basePhysicalDefensive;
        this.curPhysicalDefensive = curPhysicalDefensive;
        this.baseMagicalDefensive = baseMagicalDefensive;
        this.curMagicalDefensive = curMagicalDefensive;
        this.basePowerPoint = basePowerPoint;
        this.curPowerPoint = curPowerPoint;
        this.baseAgilityPoint = baseAgilityPoint;
        this.curAgilityPoint = curAgilityPoint;
        this.baseIntelligencePoint = baseIntelligencePoint;
        this.curIntelligencePoint = curIntelligencePoint;
        this.baseExp = baseExp;
        this.curLevel = curLevel;
        this.curNeedExp = curNeedExp;
        this.curExp = curExp;
        this.levelBuf = levelBuf;
        this.invincibleTimeAfterHit = invincibleTimeAfterHit;
    }


    public virtual void InitCharacterData (CharacterData_SO characterDataSo)
    {
        this.maxHealth = characterDataSo.maxHealth;
        this.curHealth = characterDataSo.curHealth;
        this.maxEnergy = characterDataSo.maxEnergy;
        this.curEnergy = characterDataSo.curEnergy;
        this.maxMagic = characterDataSo.maxMagic;
        this.curMagic = characterDataSo.curMagic;
        this.baseHealthRecover = characterDataSo.baseHealthRecover;
        this.curHealthRecover = characterDataSo.curHealthRecover;
        this.baseEnergyRecover = characterDataSo.baseEnergyRecover;
        this.curEnergyRecover = characterDataSo.curEnergyRecover;
        this.baseMagicRecover =characterDataSo. baseMagicRecover;
        this.curMagicRecover = characterDataSo.curMagicRecover;
        this.basePhysicalDamage = characterDataSo.basePhysicalDamage;
        this.curPhysicalDamage = characterDataSo.curPhysicalDamage;
        this.baseSkillDamage = characterDataSo.baseSkillDamage;
        this.curSkillDamage = characterDataSo.curSkillDamage;
        this.basePhysicalDefensive = characterDataSo.basePhysicalDefensive;
        this.curPhysicalDefensive = characterDataSo.curPhysicalDefensive;
        this.baseMagicalDefensive = characterDataSo.baseMagicalDefensive;
        this.curMagicalDefensive = characterDataSo.curMagicalDefensive;
        this.basePowerPoint = characterDataSo.basePowerPoint;
        this.curPowerPoint = characterDataSo.curPowerPoint;
        this.baseAgilityPoint = characterDataSo.baseAgilityPoint;
        this.curAgilityPoint = characterDataSo.curAgilityPoint;
        this.baseIntelligencePoint = characterDataSo.baseIntelligencePoint;
        this.curIntelligencePoint = characterDataSo.curIntelligencePoint;
        this.baseExp = characterDataSo.baseExp;
        this.curLevel = characterDataSo.curLevel;
        this.curNeedExp = characterDataSo.curNeedExp;
        this.curExp = characterDataSo.curExp;
        this.levelBuf = characterDataSo.levelBuf;
        this.invincibleTimeAfterHit = characterDataSo.invincibleTimeAfterHit;
    }
}
