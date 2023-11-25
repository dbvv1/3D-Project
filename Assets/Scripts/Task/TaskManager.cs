using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TaskManager : Singleton<TaskManager>,ISavable
{
    public List<TaskData_SO> tasks = new();


    public void AddTask(TaskData_SO taskData)
    {
        var newTask = Instantiate(taskData);
        tasks.Add(newTask);
        
    }

    //敌人死亡 或 拾取物品的时候调用
    public void UpdateTaskProgress(string requireName, int amount = 1)
    {
        foreach (var task in tasks)
        {
            if (task.TaskState == TaskStateType.Finished) continue;
            var matchTask = task.taskRequires.Find(r => r.requireName == requireName);
            if (matchTask!= null)
            {
                //如果是减少了物品，则要检测任务的要求是否变得不满足了
                if (matchTask.currentAmount >= matchTask.requireAmount &&
                    matchTask.currentAmount + amount <= matchTask.requireAmount)
                {
                    ++task.RestRequireAmount;
                }
                //正常流程 物品变化后判断是否满足了条件要求
                matchTask.currentAmount += amount;
                if (matchTask.currentAmount >= matchTask.requireAmount)
                {
                    --task.RestRequireAmount;
                }
            }
        }
    }
    
    public bool HaveTask(TaskData_SO targetTaskData)
    {
        return tasks.Find(task => task == targetTaskData) != null;
    }

    public TaskData_SO GetTask(TaskData_SO targetTaskData)
    {
        return tasks.Find(task => task == targetTaskData);
    }

    #region  任务的保存接口
    public string GetDataID()
    {
        return "Task";
    }

    public void SaveData(Data data)
    {
        data.tasks.Clear();
        foreach (var task in tasks)
        {
            data.tasks.Add(Instantiate(task));
        }
    }

    public void LoadData(Data data)
    {
        tasks.Clear();
        foreach (var task in data.tasks)
        {
            tasks.Add(Instantiate(task));
        }
    }
    #endregion
    
}
