using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Attack To Chase",menuName ="State Machine/Condition/Attack To Chase")]
public class AttackToChase : ConditionSO
{
    public override bool ConditionSetUp(EnemyController currentEnemy)
    {
        float distance = Vector3.Distance(currentEnemy.transform.position, currentEnemy.player.transform.position);
        //������˳����˹������룬��ת��Ϊ׷��״̬
        return distance > currentEnemy.AttackStateDistance;
    }
}
