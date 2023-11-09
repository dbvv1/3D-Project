using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineSystem : MonoBehaviour
{
    public TransitionSO tmpTransition;

    private TransitionSO transition;

    public StateActionSO currentState;

    //����������
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
        transition?.TryGetApplyCondition();//ÿһ֡��ȥ���Ƿ��г���������
        //TODO:��ǰĬ��Ϊ���˵�״̬�����������ü̳У��̳г���ͬ�����״̬��������,NPC,�Ѿ����ȵ�
        if (CanEnemyAction())
            currentState?.OnUpdate(this);

    }

    private bool CanEnemyAction()
    {
        if (currentEnemy == null || currentEnemy.IsDead || currentEnemy.IsHurt || currentEnemy.enemyCharacterStats.IsWeakState||currentEnemy.IsExecuted) return false;
        return true;
    }
}
