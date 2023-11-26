using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase To Attack", menuName = "State Machine/Condition/Chase To Attack")]
public class ChaseToAttack : ConditionSO
{
    public override bool ConditionSetUp(StateMachineSystem stateMachineSystem)
    {

        return Vector3.Distance(stateMachineSystem.currentEnemy.transform.position, stateMachineSystem.currentEnemy.player.transform.position) <= stateMachineSystem.currentEnemy.AttackStateDistance - 0.05f;
    }
}
