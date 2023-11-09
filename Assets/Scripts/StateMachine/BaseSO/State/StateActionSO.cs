using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//具体的某一个状态 该做哪些事
public abstract class StateActionSO : ScriptableObject
{
    public virtual void OnEnter(StateMachineSystem stateMachineSystem) { }

    public abstract void OnUpdate(StateMachineSystem stateMachineSystem);

    public virtual void OnExit(StateMachineSystem stateMachineSystem) { }

}
