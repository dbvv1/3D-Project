using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySleepState : StateActionSO
{
    //���˵Ļ���״̬ ʲô������
    private float sleepTimeCounter;
    public override void OnEnter(StateMachineSystem stateMachineSystem)
    {
       
    }

    public override void OnUpdate(StateMachineSystem stateMachineSystem)
    {
        sleepTimeCounter += Time.deltaTime;
    }
}
