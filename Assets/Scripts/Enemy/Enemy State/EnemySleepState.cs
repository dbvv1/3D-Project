using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySleepState : StateActionSO
{
    //���˵Ļ���״̬ ʲô������
    private float sleepTimeCounter;
    public override void OnEnter(EnemyController currentEnemy)
    {
       
    }

    public override void OnUpdate(EnemyController currentEnemy)
    {
        sleepTimeCounter += Time.deltaTime;
    }
}
