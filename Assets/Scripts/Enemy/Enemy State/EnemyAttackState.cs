using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack State", menuName = "State Machine/State/EnemyAttackState")]
public class EnemyAttackState : StateActionSO
{
    public override void OnEnter(StateMachineSystem stateMachineSystem)
    {
        stateMachineSystem.currentStateType = EnemyState.AttackState;
        stateMachineSystem.currentEnemy.curSpeed = 0;//ͣ���ƶ�׼������
    }

    public override void OnUpdate(StateMachineSystem stateMachineSystem)
    {
        EnemyController currentEnemy = stateMachineSystem.currentEnemy;
        if (currentEnemy == null || currentEnemy.IsDead||currentEnemy.IsHurt) return;
        float distance = Vector3.Distance(currentEnemy.transform.position, currentEnemy.player.transform.position);
        //�����ڹ�����Χ�ڣ����жϵ��������Ƿ����ڹ���
        if(distance<currentEnemy.AttackStateDistance&&!currentEnemy.IsAttack)
        {
            if (!currentEnemy.IsHurt)
                Attack(currentEnemy, distance);
        }
        //���˵���Ұʼ�ո�������
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
        //�����ڽ�ս���������� ʹ�ý�ս����
        if (distance < currentEnemy.AttackDistanceNear)
        {
            AttackNear(currentEnemy);
        }
        //���򣬼����������Զ�̹��������ڣ�ʹ��Զ�̹���
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
