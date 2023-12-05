using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//具体的某一个状态 该做哪些事
public abstract class StateActionSO : ScriptableObject
{
    public EnemyState enemyStateType;
    
    public virtual void OnEnter(EnemyController enemy) { }

    public abstract void OnUpdate(EnemyController enemy);

    public virtual void OnExit(EnemyController enemy) { }

}
