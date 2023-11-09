using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Patrol State", menuName = "State Machine/State/EnemyPatrolState")] 
public class EnemyPartolState : StateActionSO
{

    [SerializeField]protected float waitTime;  //Ѳ�ߵ���λ��ĵȴ�ʱ��

    public override void OnEnter(StateMachineSystem stateMachineSystem)
    {
        EnemyController currentEnemy = stateMachineSystem.currentEnemy;
        stateMachineSystem.currentStateType = EnemyState.PatrolState;
        currentEnemy.partolTargetPos=currentEnemy.GetRandomPartolPoint();
        currentEnemy.StartCoroutine(currentEnemy.WaitPatrolTime(waitTime/2, currentEnemy.partolTargetPos));
    }

    public override void OnUpdate(StateMachineSystem stateMachineSystem)
    {
        //�������Ѳ�ߵ� �����ȴ�����ѡ����һ��Ѳ�ߵ�
        EnemyController currentEnemy = stateMachineSystem.currentEnemy;
        if (currentEnemy == null || currentEnemy.IsDead||currentEnemy.IsHurt) return;
        if (!currentEnemy.IsWait&&FinishTargetPos(currentEnemy))
        {
            TurnToNewPatrolPoint(currentEnemy);
        }
    }

    
    //������ָ����Ѳ�ߵ�
    private bool FinishTargetPos(EnemyController currentEnemy)
    {
        return Vector3.Distance(currentEnemy.transform.position, currentEnemy.partolTargetPos) < 1f;
    }

    //Wait��֮��ת���µ�Ѳ�ߵ�
    private void TurnToNewPatrolPoint(EnemyController currentEnemy)
    {
        currentEnemy.partolTargetPos = currentEnemy.GetRandomPartolPoint();
        currentEnemy.StartCoroutine(currentEnemy.WaitPatrolTime(waitTime,currentEnemy.partolTargetPos));
    }

}
