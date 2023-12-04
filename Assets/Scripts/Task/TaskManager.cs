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

    //�������� �� ʰȡ��Ʒ��ʱ�����
    public void UpdateTaskProgress(string requireName, int amount = 1)
    {
        foreach (var task in tasks)
        {
            if (task.TaskState == TaskStateType.Finished) continue;
            var matchTaskRequire = task.taskRequires.Find(r => r.requireName == requireName);
            if (matchTaskRequire!= null)
            {
                //����Ǽ�������Ʒ����Ҫ��������Ҫ���Ƿ��ò�������
                if (matchTaskRequire.currentAmount >= matchTaskRequire.requireAmount &&
                    matchTaskRequire.currentAmount + amount < matchTaskRequire.requireAmount)
                {
                    ++task.RestRequireAmount;
                }
                //�������� ��Ʒ�仯���ж��Ƿ�����������Ҫ��
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

    #region  ����ı���ӿ�
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
