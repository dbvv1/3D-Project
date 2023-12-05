using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrol To Chase", menuName = "State Machine/Condition/Patrol To Chase")]
public class PatrolToChase : ConditionSO
{
    public override bool ConditionSetUp(EnemyController currentEnemy)
    {
        return currentEnemy.FindPlayer;
    }
}
