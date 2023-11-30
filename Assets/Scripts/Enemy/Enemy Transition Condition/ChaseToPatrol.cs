using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase To Patrol", menuName = "State Machine/Condition/Chase To Patrol")]
public class ChaseToPatrol : ConditionSO
{
    public override bool ConditionSetUp(StateMachineSystem stateMachineSystem)
    {
        return stateMachineSystem.currentEnemy.CanReturnToPatrolState();
    }
}
