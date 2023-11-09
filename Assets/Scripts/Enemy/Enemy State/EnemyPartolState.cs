using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Patrol State", menuName = "State Machine/State/EnemyPatrolState")] 
public class EnemyPartolState : StateActionSO
{

    [SerializeField]protected float waitTime;  //巡逻到点位后的等待时间

    public override void OnEnter(StateMachineSystem stateMachineSystem)
    {
        EnemyController currentEnemy = stateMachineSystem.currentEnemy;
        stateMachineSystem.currentStateType = EnemyState.PatrolState;
        currentEnemy.partolTargetPos=currentEnemy.GetRandomPartolPoint();
        currentEnemy.StartCoroutine(currentEnemy.WaitPatrolTime(waitTime/2, currentEnemy.partolTargetPos));
    }

    public override void OnUpdate(StateMachineSystem stateMachineSystem)
    {
        //如果到达巡逻点 则进入等待，后选择下一个巡逻点
        EnemyController currentEnemy = stateMachineSystem.currentEnemy;
        if (currentEnemy == null || currentEnemy.IsDead||currentEnemy.IsHurt) return;
        if (!currentEnemy.IsWait&&FinishTargetPos(currentEnemy))
        {
            TurnToNewPatrolPoint(currentEnemy);
        }
    }

    
    //到达了指定的巡逻点
    private bool FinishTargetPos(EnemyController currentEnemy)
    {
        return Vector3.Distance(currentEnemy.transform.position, currentEnemy.partolTargetPos) < 1f;
    }

    //Wait，之后转向新的巡逻点
    private void TurnToNewPatrolPoint(EnemyController currentEnemy)
    {
        currentEnemy.partolTargetPos = currentEnemy.GetRandomPartolPoint();
        currentEnemy.StartCoroutine(currentEnemy.WaitPatrolTime(waitTime,currentEnemy.partolTargetPos));
    }

}
