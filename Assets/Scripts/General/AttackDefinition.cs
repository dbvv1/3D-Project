using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))] 
public class AttackDefinition : MonoBehaviour
{
    public float attackBaseDamage;

    public float energyAmount;         //如果对方是敌人 或者 对方正在普通格挡 会造成对方的体力消耗

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

        //碰撞体挂载在子物体上的特殊情况
        if (other.GetComponentInParent<MetalonController>())
            other.GetComponentInParent<CharacterStats>()?.TakeDamage(this);
    }

}
