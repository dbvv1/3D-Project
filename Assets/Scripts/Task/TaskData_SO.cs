using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task",menuName = "Task/Task Data")]
public class TaskData_SO : ScriptableObject
{
    [System.Serializable]
    public class TaskRequire
    {
        //任务需求
        public string requireName;
        
        public int requireAmount;

        public int currentAmount;

    }
    
    public string taskName;
    [TextArea]
    public string taskDescription;

    public TaskStateType TaskState { get; set; }
    
    public List<TaskRequire> taskRequires = new();

    public List<InventoryItem> rewards = new();

    private int restRequireAmount;

    public int RestRequireAmount
    {
        get => restRequireAmount;
        set
        {
            restRequireAmount = value;
            if (restRequireAmount == 0) TaskState = TaskStateType.Completed;
            else TaskState = TaskStateType.Started;
        }
    }

    public void GiveRewards()
    {
        foreach (var reward in rewards)
        {
            if (reward.itemAmount < 0)
            {
                int costAmount = -reward.itemAmount;
                InventoryManager.Instance.TaskCostItem(reward, costAmount);
            }
            else
            {
                InventoryManager.Instance.TaskRewardItem(reward, reward.itemAmount);
            }
        }
    }

    public void InitTaskData(TaskData_SO taskData)
    {
        
    }
    
}
