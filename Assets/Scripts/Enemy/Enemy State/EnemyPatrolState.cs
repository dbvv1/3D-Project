using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Patrol State", menuName = "State Machine/State/EnemyPatrolState")] 
public class EnemyPatrolState : StateActionSO
{

    [SerializeField]protected float waitTime;  //Ѳ�ߵ���λ��ĵȴ�ʱ��

    public override void OnEnter(EnemyController currentEnemy)
    {
        currentEnemy.patrolTargetPos = currentEnemy.GetRandomPatrolPoint();
        currentEnemy.StartCoroutine(currentEnemy.WaitPatrolTime(waitTime/2, currentEnemy.patrolTargetPos));
    }

    public override void OnUpdate(EnemyController currentEnemy)
    {
        //�������Ѳ�ߵ� �����ȴ�����ѡ����һ��Ѳ�ߵ�
        if (currentEnemy == null || currentEnemy.IsDead||currentEnemy.IsHurt) return;
        if (!currentEnemy.IsWait&&FinishTargetPos(currentEnemy))
        {
            TurnToNewPatrolPoint(currentEnemy);
        }
    }

    
    //������ָ����Ѳ�ߵ�
    private bool FinishTargetPos(EnemyController currentEnemy)
    {
        return Vector3.Distance(currentEnemy.transform.position, currentEnemy.patrolTargetPos) < 1f;
    }

    //Wait��֮��ת���µ�Ѳ�ߵ�
    private void TurnToNewPatrolPoint(EnemyController currentEnemy)
    {
        currentEnemy.patrolTargetPos = currentEnemy.GetRandomPatrolPoint();
        currentEnemy.StartCoroutine(currentEnemy.WaitPatrolTime(waitTime,currentEnemy.patrolTargetPos));
    }

}
