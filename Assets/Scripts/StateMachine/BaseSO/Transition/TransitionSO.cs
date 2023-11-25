using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transition", menuName = "State Machine/Transition")]
public class TransitionSO : ScriptableObject
{
    public bool isInit = false;
    
    //ת������Ϣ
    [Serializable]
    private class StateTransitionConfig 
    {
        public StateActionSO fromState;
        public StateActionSO toState;
        public ConditionSO condition;
        public int priority;
    }
    
    
    //�洢����״̬ת����Ϣ������
    private Dictionary<StateActionSO, List<StateTransitionConfig>> states = new Dictionary<StateActionSO, List<StateTransitionConfig>>();

    //��ȡ״̬���ã����ⲿ�����ֶ�������Ϣ
    [SerializeField] private List<StateTransitionConfig> configStateData = new List<StateTransitionConfig>();

    
    /*public void Init()
    {
        isInit = true;
        SaveAllStateTransitionInfo();
    }*/

#if UNITY_EDITOR
    private void OnValidate()
    {
        SaveAllStateTransitionInfo();
    }
#endif

    /// <summary>
    /// ��������״̬������Ϣ
    /// </summary>
    private void SaveAllStateTransitionInfo() 
    {
        foreach (var item in configStateData)
        {
            //���ʱ����������Ѿ����ú���Ϣ�ˡ�������Ҫ�����ǵ�ת����ϵ��������
            if (!states.ContainsKey(item.fromState)) 
            {
                //������ڴ洢�ֵ��Ƿ��д��ڵ�Key,���û��������Ҫ����һ�������ҳ�ʼ�����������洢����
                states.Add(item.fromState, new List<StateTransitionConfig>());
                states[item.fromState].Add(item);
            }
            else 
            {
                states[item.fromState].Add(item);
            }
        }
    }


    /// <summary>
    /// ����ȥ��ȡ������������״̬
    /// </summary>
    public void TryGetApplyCondition(StateMachineSystem stateMachineSystem) 
    {
        int transitionPriority = -1;
        StateActionSO toState = null;

        //������ǰ״̬��ת��״̬�Ƿ�����������
        if (states.ContainsKey(stateMachineSystem.currentState))
        {
            foreach (var stateItem in states[stateMachineSystem.currentState])
            {
                if (stateItem.condition.ConditionSetUp(stateMachineSystem))
                {
                    if (stateItem.priority > transitionPriority)
                    {
                        transitionPriority = stateItem.priority;
                        toState = stateItem.toState;
                    }
                }
            }
        }
        //�ֵ���û�е�ǰ״̬������ ֱ�ӷ���
        else return;

        //���Խ���ת��״̬
        if (toState != null) 
        {
            stateMachineSystem.currentState.OnExit(stateMachineSystem);
            Debug.Log(toState);
            stateMachineSystem.currentState = toState;
            stateMachineSystem.currentState.OnEnter(stateMachineSystem);            
        }
    }



}
