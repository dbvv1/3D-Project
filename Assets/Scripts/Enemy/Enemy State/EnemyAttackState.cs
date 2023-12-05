using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack State", menuName = "State Machine/State/EnemyAttackState")]
public class EnemyAttackState : StateActionSO
{
    public override void OnEnter(EnemyController currentEnemy)
    {
        currentEnemy.curSpeed = 0;//ͣ���ƶ�׼������
    }

    public override void OnUpdate(EnemyController currentEnemy)
    {
        Vector3 playerPos = currentEnemy.player.transform.position;
        
        if (currentEnemy == null || currentEnemy.IsDead||currentEnemy.IsHurt) return;
        float distance = Vector3.Distance(currentEnemy.transform.position,
            new Vector3(playerPos.x, currentEnemy.transform.position.y, playerPos.z));
        //�����ڹ�����Χ�ڣ����жϵ��������Ƿ����ڹ���
        if(distance<currentEnemy.AttackStateDistance&&!currentEnemy.IsAttack)
        {
            if (!currentEnemy.IsHurt)
                Attack(currentEnemy, distance);
        }
        //���˵���Ұʼ�ո�������
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(playerPos.x, currentEnemy.transform.position.y, playerPos.z) - currentEnemy.transform.position);
        currentEnemy.transform.rotation=(Quaternion.Slerp(currentEnemy.transform.rotation, toRotation, Time.deltaTime * currentEnemy.rotateSpeed));
    }

    public override void OnExit(EnemyController currentEnemy)
    {
        currentEnemy.curSpeed = currentEnemy.chaseSpeed;
    }

    private void Attack(EnemyController currentEnemy,float distance)
    {
        //�����ڽ�ս���������� ʹ�ý�ս����
        if (distance < currentEnemy.AttackDistanceNear)
        {
            currentEnemy.AttackNearF();
        }
        //���򣬼����������Զ�̹��������ڣ�ʹ��Զ�̹���
        else if (distance < currentEnemy.AttackDistanceFar)
        {
            currentEnemy.AttackFarF();
        }
    }

}
