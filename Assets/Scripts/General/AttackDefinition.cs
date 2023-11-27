using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))] 
public class AttackDefinition : MonoBehaviour
{
    public float attackBaseDamage;

    public float energyAmount;         //����Է��ǵ��� ���� �Է�������ͨ�� ����ɶԷ�����������

    public CharacterStats attacker;

    public DamageType damageType;

    public float DamageAmount => attackBaseDamage + damageType switch
    {
        DamageType.Physical => attacker.CurPhysicalDamage,
        DamageType.Magical => attacker.CurMagicDamage,
        DamageType.True => 0,
        _ => 0
    };


    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<CharacterStats>()?.TakeDamage(this);

        //��ײ��������������ϵ��������
        if (other.GetComponentInParent<MetalonController>())
            other.GetComponentInParent<CharacterStats>()?.TakeDamage(this);
    }

}
