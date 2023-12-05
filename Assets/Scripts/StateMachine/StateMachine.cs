using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IStateMachineEnemy
{
    public void RegisterStateMachineEnemy();

    public void UnRegisterStateMachineEnemy();
}

public abstract class StateMachine<T> : Singleton<StateMachine<T>> where T : EnemyController
{
    [SerializeField] private TransitionSO transition;

    [SerializeField] private StateActionSO initState;

    private readonly List<T> enemies = new();
    

    private void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            StateMachineTick(enemies[i]);
        }
    }

    private void StateMachineTick(EnemyController enemy)
    {
        if (transition != null) transition.TryGetApplyCondition(enemy); //每一帧都去找是否有成立的条件

        if (CanEnemyAction(enemy))
            if (enemy.CurrentEnemyState != null)
                enemy.CurrentEnemyState.OnUpdate(enemy);
    }

    //敌人是否能行动的条件判断
    protected virtual bool CanEnemyAction(EnemyController enemy)
    {
        if (enemy == null || enemy.IsDead || enemy.IsHurt || enemy.enemyCharacterStats.IsWeakState ||
            enemy.IsExecuted) return false;
        return true;
    }

    #region 添加删除敌人的接口

    public void AddEnemy(T enemy)
    {
        enemies.Add(enemy);
        StartCoroutine(InitEnemyState(enemy));
    }

    public void RemoveEnemy(T enemy)
    {
        enemies.Remove(enemy);
    }

    private IEnumerator  InitEnemyState(T enemy)
    {
        yield return null;
        if (enemy != null && CanEnemyAction(enemy))
        {
            enemy.CurrentEnemyState = initState;
            enemy.CurrentEnemyState.OnEnter(enemy);
        }
    }
    

    #endregion
}