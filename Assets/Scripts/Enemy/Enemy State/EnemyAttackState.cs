using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack State", menuName = "State Machine/State/EnemyAttackState")]
public class EnemyAttackState : StateActionSO
{
    public override void OnEnter(EnemyController currentEnemy)
    {
        currentEnemy.curSpeed = 0;//停下移动准备攻击
    }

    public override void OnUpdate(EnemyController currentEnemy)
    {
        Vector3 playerPos = currentEnemy.player.transform.position;
        
        if (currentEnemy == null || currentEnemy.IsDead||currentEnemy.IsHurt) return;
        float distance = Vector3.Distance(currentEnemy.transform.position,
            new Vector3(playerPos.x, currentEnemy.transform.position.y, playerPos.z));
        //敌人在攻击范围内，先判断敌人现在是否正在攻击
        if(distance<currentEnemy.AttackStateDistance&&!currentEnemy.IsAttack)
        {
            if (!currentEnemy.IsHurt)
                Attack(currentEnemy, distance);
        }
        //敌人的视野始终跟随人物
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(playerPos.x, currentEnemy.transform.position.y, playerPos.z) - currentEnemy.transform.position);
        currentEnemy.transform.rotation=(Quaternion.Slerp(currentEnemy.transform.rotation, toRotation, Time.deltaTime * currentEnemy.rotateSpeed));
    }

    public override void OnExit(EnemyController currentEnemy)
    {
        currentEnemy.curSpeed = currentEnemy.chaseSpeed;
    }

    private void Attack(EnemyController currentEnemy,float distance)
    {
        //敌人在近战攻击距离内 使用近战攻击
        if (distance < currentEnemy.AttackDistanceNear)
        {
            currentEnemy.AttackNearF();
        }
        //否则，即如果敌人在远程攻击距离内，使用远程攻击
        else if (distance < currentEnemy.AttackDistanceFar)
        {
            currentEnemy.AttackFarF();
        }
    }

}
