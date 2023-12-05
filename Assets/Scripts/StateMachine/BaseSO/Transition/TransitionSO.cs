using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transition", menuName = "State Machine/Transition")]
public class TransitionSO : ScriptableObject
{
    public bool isInit = false;
    
    //转换的信息
    [Serializable]
    private class StateTransitionConfig 
    {
        public StateActionSO fromState;
        public StateActionSO toState;
        public ConditionSO condition;
        public int priority;
    }
    
    
    //存储所有状态转换信息和条件
    private Dictionary<StateActionSO, List<StateTransitionConfig>> states = new Dictionary<StateActionSO, List<StateTransitionConfig>>();

    //获取状态配置，即外部面板的手动配置信息
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
    /// 保存所有状态配置信息
    /// </summary>
    private void SaveAllStateTransitionInfo() 
    {
        foreach (var item in configStateData)
        {
            //这个时候外面面板已经配置好信息了。我们需要将它们的转换关系保存起来
            if (!states.ContainsKey(item.fromState)) 
            {
                //检测现在存储字典是否有存在的Key,如果没有我们需要创建一个，并且初始化它的条件存储容器
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
    /// 尝试去获取条件成立的新状态
    /// </summary>
    public void TryGetApplyCondition(EnemyController currentEnemy) 
    {
        int transitionPriority = -1;
        StateActionSO toState = null;

        if (ReferenceEquals(currentEnemy.CurrentEnemyState, null)) return;
        
        //遍历当前状态能转的状态是否有条件成立
        if (states.ContainsKey(currentEnemy.CurrentEnemyState))
        {
            foreach (var stateItem in states[currentEnemy.CurrentEnemyState])
            {
                if (stateItem.condition.ConditionSetUp(currentEnemy))
                {
                    if (stateItem.priority > transitionPriority)
                    {
                        transitionPriority = stateItem.priority;
                        toState = stateItem.toState;
                    }
                }
            }
        }
        //字典中没有当前状态的描述 直接返回
        else return;

        //可以进行转换状态
        if (!ReferenceEquals(toState, null))  
        {
            currentEnemy.CurrentEnemyState.OnExit(currentEnemy);
            currentEnemy.CurrentEnemyState = toState;
            currentEnemy.CurrentEnemyState.OnEnter(currentEnemy);            
        }
    }



}
