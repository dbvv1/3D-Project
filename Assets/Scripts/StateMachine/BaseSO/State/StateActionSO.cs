using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ĳһ��״̬ ������Щ��
public abstract class StateActionSO : ScriptableObject
{
    public EnemyState enemyStateType;
    
    public virtual void OnEnter(EnemyController enemy) { }

    public abstract void OnUpdate(EnemyController enemy);

    public virtual void OnExit(EnemyController enemy) { }

}
