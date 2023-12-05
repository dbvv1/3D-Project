using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySleepState : StateActionSO
{
    //敌人的缓冲状态 什么都不做
    private float sleepTimeCounter;
    public override void OnEnter(EnemyController currentEnemy)
    {
       
    }

    public override void OnUpdate(EnemyController currentEnemy)
    {
        sleepTimeCounter += Time.deltaTime;
    }
}
