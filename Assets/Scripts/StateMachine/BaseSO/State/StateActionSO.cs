using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ĳһ��״̬ ������Щ��
public abstract class StateActionSO : ScriptableObject
{
    public virtual void OnEnter(StateMachineSystem stateMachineSystem) { }

    public abstract void OnUpdate(StateMachineSystem stateMachineSystem);

    public virtual void OnExit(StateMachineSystem stateMachineSystem) { }

}
