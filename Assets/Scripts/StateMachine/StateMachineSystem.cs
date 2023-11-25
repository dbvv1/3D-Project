using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineSystem : MonoBehaviour
{
    public TransitionSO transition;

    public StateActionSO currentState;

    //敌人相关组件
    public EnemyState currentStateType;

    public EnemyController currentEnemy;
    
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
        //TODO:当前默认为敌人的状态机，后续采用继承，继承出不同种类的状态机（敌人,NPC,友军）等等
        if (CanEnemyAction())
            if(currentState!=null)currentState.OnUpdate(this);

    }

    private bool CanEnemyAction()
    {
        if (currentEnemy == null || currentEnemy.IsDead || currentEnemy.IsHurt || currentEnemy.enemyCharacterStats.IsWeakState||currentEnemy.IsExecuted) return false;
        return true;
    }
}
