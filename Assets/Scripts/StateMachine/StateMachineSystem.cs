using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineSystem : MonoBehaviour
{
    public TransitionSO tmpTransition;

    private TransitionSO transition;

    public StateActionSO currentState;

    //敌人相关组件
    public EnemyState currentStateType;

    public EnemyController currentEnemy;

    private void Awake()
    {
        currentEnemy = GetComponent<EnemyController>();
        transition = ScriptableObject.Instantiate(tmpTransition);
        transition.Init(this);
    }

    private void Start()
    {
        currentState?.OnEnter(this);
    }

    private void Update()
    {
        StateMachineTick();
    }

    private void StateMachineTick() 
    {
        transition?.TryGetApplyCondition();//每一帧都去找是否有成立的条件
        //TODO:当前默认为敌人的状态机，后续采用继承，继承出不同种类的状态机（敌人,NPC,友军）等等
        if (CanEnemyAction())
            currentState?.OnUpdate(this);

    }

    private bool CanEnemyAction()
    {
        if (currentEnemy == null || currentEnemy.IsDead || currentEnemy.IsHurt || currentEnemy.enemyCharacterStats.IsWeakState||currentEnemy.IsExecuted) return false;
        return true;
    }
}
