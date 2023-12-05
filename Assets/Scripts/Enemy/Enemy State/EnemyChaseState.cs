using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chase State", menuName = "State Machine/State/EnemyChaseState")]
public class EnemyChaseState : StateActionSO
{
    public override void OnEnter(EnemyController currentEnemy)
    {
        currentEnemy.StopAllCoroutines();
        currentEnemy.IsWait = false;
        currentEnemy.curSpeed = currentEnemy.chaseSpeed;
        //currentEnemy.StartCoroutine(currentEnemy.WaitPatrolTime(0, currentEnemy.player.transform.position));  ��update�е�ת�ӽǳ�ͻ��
    }

    public override void OnUpdate(EnemyController currentEnemy)
    {
        if (currentEnemy == null || currentEnemy.IsDead || currentEnemy.IsHurt) return;
        Vector3 playerPos = currentEnemy.player.transform.position;
        //���˵���Ұʼ�ո�������
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(playerPos.x, currentEnemy.transform.position.y, playerPos.z) - currentEnemy.transform.position);
        currentEnemy.transform.rotation=Quaternion.Slerp(currentEnemy.transform.rotation, toRotation, Time.deltaTime * currentEnemy.rotateSpeed);
    }


}
