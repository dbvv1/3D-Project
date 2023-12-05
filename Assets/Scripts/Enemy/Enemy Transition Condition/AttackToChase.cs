using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Attack To Chase",menuName ="State Machine/Condition/Attack To Chase")]
public class AttackToChase : ConditionSO
{
    public override bool ConditionSetUp(EnemyController currentEnemy)
    {
        float distance = Vector3.Distance(currentEnemy.transform.position, currentEnemy.player.transform.position);
        //如果敌人超出了攻击距离，则转变为追逐状态
        return distance > currentEnemy.AttackStateDistance;
    }
}
