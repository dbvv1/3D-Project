using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character/Character Data")] 
public class CharacterData_SO : ScriptableObject
{
    [Header("��������")]
    public float maxHealth;            //�������ֵ

    public float maxEnergy;            //�������ֵ

    public float maxMagic;             //���ħ��ֵ

    public float baseHealthRecover;    //���������ظ�  ��value/�룩

    public float baseEnergyRecover;    //���������ظ�

    public float baseMagicRecover;     //����ħ���ظ�

    public float basePhysicalDamage;   //���������˺�

    public float baseSkillDamage;      //���������˺�

    public float basePhysicalDefensive;//�����������

    public float baseMagicalDefensive; //����ħ������

    [Header("������Χ����")]
    public int basePowerPoint;         //�������� Ӱ����������������˺�

    public int baseAgilityPoint;       //���ݵ��� Ӱ����������������ָ�

    public int baseIntelligencePoint;  //�������� Ӱ�����ħ���������˺�

    [Header("������Ϣ")]
    public float baseExp;                //��������Ļ�������ֵ

    public float levelBuf;               //����������ӳ�

    [Header("��������")]
    public float invincibleTimeAfterHit;//���˺���޵�ʱ��

}
