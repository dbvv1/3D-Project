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

    //�������� �� ʰȡ��Ʒ��ʱ�����
    public void UpdateTaskProgress(string requireName, int amount = 1)
    {
        foreach (var task in tasks)
        {
            if (task.TaskState == TaskStateType.Finished) continue;
            var matchTask = task.taskRequires.Find(r => r.requireName == requireName);
            if (matchTask!= null)
            {
                //����Ǽ�������Ʒ����Ҫ��������Ҫ���Ƿ��ò�������
                if (matchTask.currentAmount >= matchTask.requireAmount &&
                    matchTask.currentAmount + amount <= matchTask.requireAmount)
                {
                    ++task.RestRequireAmount;
                }
                //�������� ��Ʒ�仯���ж��Ƿ�����������Ҫ��
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

    #region  ����ı���ӿ�
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
