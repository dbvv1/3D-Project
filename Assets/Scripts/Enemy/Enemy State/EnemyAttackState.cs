using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack State", menuName = "State Machine/State/EnemyAttackState")]
public class EnemyAttackState : StateActionSO
{
    public override void OnEnter(StateMachineSystem stateMachineSystem)
    {
        stateMachineSystem.currentStateType = EnemyState.AttackState;
        stateMachineSystem.currentEnemy.curSpeed = 0;//停下移动准备攻击
    }

    public override void OnUpdate(StateMachineSystem stateMachineSystem)
    {
        EnemyController currentEnemy = stateMachineSystem.currentEnemy;
        if (currentEnemy == null || currentEnemy.IsDead||currentEnemy.IsHurt) return;
        float distance = Vector3.Distance(currentEnemy.transform.position, currentEnemy.player.transform.position);
        //敌人在攻击范围内，先判断敌人现在是否正在攻击
        if(distance<currentEnemy.AttackStateDistance&&!currentEnemy.IsAttack)
        {
            if (!currentEnemy.IsHurt)
                Attack(currentEnemy, distance);
        }
        //敌人的视野始终跟随人物
        Vector3 playerPos = currentEnemy.player.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(playerPos.x, currentEnemy.transform.position.y, playerPos.z) - currentEnemy.transform.position);
        currentEnemy.transform.rotation=(Quaternion.Slerp(currentEnemy.transform.rotation, toRotation, Time.deltaTime * currentEnemy.rotateSpeed));
    }

    public override void OnExit(StateMachineSystem stateMachineSystem)
    {
        stateMachineSystem.currentEnemy.curSpeed = stateMachineSystem.currentEnemy.chaseSpeed;
    }

    private void Attack(EnemyController currentEnemy,float distance)
    {
        //敌人在近战攻击距离内 使用近战攻击
        if (distance < currentEnemy.AttackDistanceNear)
        {
            AttackNear(currentEnemy);
        }
        //否则，即如果敌人在远程攻击距离内，使用远程攻击
        else if (distance < currentEnemy.AttackDistanceFar)
        {
            AttackFar(currentEnemy);
        }
    }

    private void AttackNear(EnemyController currentEnemy)
    {
        currentEnemy.anim.SetTrigger("AttackNear");
    }

    private void AttackFar(EnemyController currentEnemy)
    {
        currentEnemy.anim.SetTrigger("AttackFar");
    }
}
