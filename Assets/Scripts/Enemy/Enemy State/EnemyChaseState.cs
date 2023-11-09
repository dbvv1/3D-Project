using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chase State", menuName = "State Machine/State/EnemyChaseState")]
public class EnemyChaseState : StateActionSO
{
    public override void OnEnter(StateMachineSystem stateMachineSystem)
    {
        stateMachineSystem.currentStateType = EnemyState.ChaseState;
        stateMachineSystem.currentEnemy.StopAllCoroutines();
        stateMachineSystem.currentEnemy.IsWait = false;
        stateMachineSystem.currentEnemy.curSpeed = stateMachineSystem.currentEnemy.chaseSpeed;
        //currentEnemy.StartCoroutine(currentEnemy.WaitPatrolTime(0, currentEnemy.player.transform.position));  和update中的转视角冲突了
    }

    public override void OnUpdate(StateMachineSystem stateMachineSystem)
    {
        EnemyController currentEnemy = stateMachineSystem.currentEnemy;
        if (currentEnemy == null || currentEnemy.IsDead || currentEnemy.IsHurt) return;
        Vector3 playerPos = currentEnemy.player.transform.position;
        //敌人的视野始终跟随人物
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(playerPos.x, currentEnemy.transform.position.y, playerPos.z) - currentEnemy.transform.position);
        currentEnemy.transform.rotation=Quaternion.Slerp(currentEnemy.transform.rotation, toRotation, Time.deltaTime * currentEnemy.rotateSpeed);
    }


}
