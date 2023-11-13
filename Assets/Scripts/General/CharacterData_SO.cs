using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character/Character Data")] 
public class CharacterData_SO : ScriptableObject
{
    [Header("��������")]
    public float maxHealth;            //�������ֵ

    public float curHealth;

    public float maxEnergy;            //�������ֵ

    public float curEnergy;

    public float maxMagic;             //���ħ��ֵ
    
    public float curMagic;

    public float baseHealthRecover;    //���������ظ�  ��value/�룩

    public float curHealthRecover;

    public float baseEnergyRecover;    //���������ظ�
    
    public float curEnergyRecover;

    public float baseMagicRecover;     //����ħ���ظ�
    
    public float curMagicRecover;

    public float basePhysicalDamage;   //���������˺�
    
    public float curPhysicalDamage;

    public float baseSkillDamage;      //���������˺�
    
    public float curSkillDamage;

    public float basePhysicalDefensive;//�����������
    
    public float curPhysicalDefensive;

    public float baseMagicalDefensive; //����ħ������

    public float curMagicalDefensive;
    
    [Header("������Χ����")]
    public int basePowerPoint;         //�������� Ӱ����������������˺�
    
    public int curPowerPoint;

    public int baseAgilityPoint;       //���ݵ��� Ӱ����������������ָ�
    
    public int curAgilityPoint;

    public int baseIntelligencePoint;  //�������� Ӱ�����ħ���������˺�
    
    public int curIntelligencePoint;

    [Header("������Ϣ")]
    public float baseExp;                //��������Ļ�������ֵ

    public int curLevel;
    
    public float curNeedExp;

    public float curExp;

    public float levelBuf;               //����������ӳ�
    
    
    [Header("��������")]
    public float invincibleTimeAfterHit;//���˺���޵�ʱ��

    
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
