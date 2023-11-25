using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineSystem : MonoBehaviour
{
    public TransitionSO transition;

    public StateActionSO currentState;

    //����������
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
        if(transition!=null)transition.TryGetApplyCondition(this);//ÿһ֡��ȥ���Ƿ��г���������
        //TODO:��ǰĬ��Ϊ���˵�״̬�����������ü̳У��̳г���ͬ�����״̬��������,NPC,�Ѿ����ȵ�
        if (CanEnemyAction())
            if(currentState!=null)currentState.OnUpdate(this);

    }

    private bool CanEnemyAction()
    {
        if (currentEnemy == null || currentEnemy.IsDead || currentEnemy.IsHurt || currentEnemy.enemyCharacterStats.IsWeakState||currentEnemy.IsExecuted) return false;
        return true;
    }
}
