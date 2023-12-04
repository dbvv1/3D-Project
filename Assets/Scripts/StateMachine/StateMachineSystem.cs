using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachineEnemy
{
    public void RegisterStateMachineEnemy();

    public void UnRegisterStateMachineEnemy();
}

public class StateMachineSystem : MonoBehaviour
{
    public TransitionSO transition;

    public StateActionSO currentState;

    //敌人相关组件
    public EnemyState currentStateType;

    [HideInInspector]public EnemyController currentEnemy;
    
    private void Awake()
    {
        currentEnemy = GetComponent<EnemyController>();
    }

    private void Start()
    {
        if(currentState!=null)currentState.OnEnter(this);
    }

    private void Update()
    {
        StateMachineTick();
    }

    private void StateMachineTick() 
    {
        if(transition!=null)transition.TryGetApplyCondition(this);//每一帧都去找是否有成立的条件
      
        if (CanEnemyAction())
            if(currentState!=null)currentState.OnUpdate(this);

    }

    private bool CanEnemyAction()
    {
        if (currentEnemy == null || currentEnemy.IsDead || currentEnemy.IsHurt || currentEnemy.enemyCharacterStats.IsWeakState||currentEnemy.IsExecuted) return false;
        return true;
    }
}
