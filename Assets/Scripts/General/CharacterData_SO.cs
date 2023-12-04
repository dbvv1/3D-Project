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
    public int baseExp;                //��������Ļ�������ֵ

    public int curLevel;
    
    public int curNeedExp;

    public int curExp;

    public float levelBuf;               //����������ӳ�
    
    
    [Header("��������")]
    public float invincibleTimeAfterHit;//���˺���޵�ʱ��
    

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
