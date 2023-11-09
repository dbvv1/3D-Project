using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Attack To Chase",menuName ="State Machine/Condition/Attack To Chase")]
public class AttackToChase : ConditionSO
{
    public override bool ConditionSetUp(StateMachineSystem stateMachineSystem)
    {
        float distance = Vector3.Distance(stateMachineSystem.currentEnemy.transform.position, stateMachineSystem.currentEnemy.player.transform.position);
        //如果敌人超出了攻击距离，则转变为追逐状态
        return distance > stateMachineSystem.currentEnemy.AttackStateDistance;
    }
}
