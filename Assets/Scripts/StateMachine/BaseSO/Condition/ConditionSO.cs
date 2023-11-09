using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//转换条件
public abstract class ConditionSO : ScriptableObject
{
    public abstract bool ConditionSetUp(StateMachineSystem stateMachineSystem);//条件是否成立

}
