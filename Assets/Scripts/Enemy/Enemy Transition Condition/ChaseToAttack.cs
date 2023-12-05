using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase To Attack", menuName = "State Machine/Condition/Chase To Attack")]
public class ChaseToAttack : ConditionSO
{
    public override bool ConditionSetUp(EnemyController currentEnemy)
    {

        return Vector3.Distance(currentEnemy.transform.position, currentEnemy.player.transform.position) <= currentEnemy.AttackStateDistance - 0.05f;
    }
}
