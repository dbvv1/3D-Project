using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TaskManager : Singleton<TaskManager>,ISavable
{
    public List<TaskData_SO> tasks = new();

    private readonly Dictionary<string, TaskData_SO> taskDict = new();
    
    protected virtual void OnEnable()
    {
        ((ISavable)this).RegisterSaveData();
    }

    protected virtual void OnDisable()
    {
        ((ISavable)this).UnRegisterSaveData();
    }


    public void AddTask(TaskData_SO taskData)
    {
        var newTask = Instantiate(taskData);
        newTask.TaskState = TaskStateType.Started;
        newTask.RestRequireAmount = newTask.taskRequires.Count;
        tasks.Add(newTask);
        taskDict.Add(newTask.name, newTask);
    }

    //敌人死亡 或 拾取物品的时候调用
    public void UpdateTaskProgress(string requireName, int amount = 1)
    {
        foreach (var task in tasks)
        {
            if (task.TaskState == TaskStateType.Finished) continue;
            var matchTaskRequire = task.taskRequires.Find(r => r.requireName == requireName);
            if (matchTaskRequire!= null)
            {
                //如果是减少了物品，则要检测任务的要求是否变得不满足了
                if (matchTaskRequire.currentAmount >= matchTaskRequire.requireAmount &&
                    matchTaskRequire.currentAmount + amount < matchTaskRequire.requireAmount)
                {
                    ++task.RestRequireAmount;
                }
                //正常流程 物品变化后判断是否满足了条件要求
                matchTaskRequire.currentAmount += amount;
                if (matchTaskRequire.currentAmount >= matchTaskRequire.requireAmount)
                {
                    --task.RestRequireAmount;
                }
            }
        }
    }
    // public bool HaveTask(TaskData_SO targetTaskData)
    // {
    //     return tasks.Find(task => task.taskName == targetTaskData.taskName) != null;
    // }
    //
    // public TaskData_SO GetTask(TaskData_SO targetTaskData)
    // {
    //     return tasks.Find(task => task.taskName == targetTaskData.taskName);
    // }
    
    public bool HaveTask(TaskData_SO targetTaskData)
    {
        return taskDict.ContainsKey(targetTaskData.taskName);
    }

    public TaskData_SO GetTask(TaskData_SO targetTaskData)
    {
        return taskDict.TryGetValue(targetTaskData.taskName, out var task) ? task : null;
    }

    #region  任务的保存接口
    public string GetDataID()
    {
        return "Task";
    }

    public void SaveData(Data data)
    {
        data.tasks.Clear();
        data.taskDict.Clear();
        foreach (var task in tasks)
        {
            var newTask = Instantiate(task);
            data.tasks.Add(newTask);
            data.taskDict.Add(newTask.name,newTask);
        }
    }

    public void LoadData(Data data)
    {
        tasks.Clear();
        taskDict.Clear();
        foreach (var task in data.tasks)
        {
            var newTask = Instantiate(task);
            tasks.Add(newTask);
            taskDict.Add(newTask.name,newTask);
        }
    }
    #endregion
    
}
