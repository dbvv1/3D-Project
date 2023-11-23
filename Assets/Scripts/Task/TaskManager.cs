using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TaskManager : Singleton<TaskManager>
{
    public class GameTask
    {
        public TaskData_SO taskData;
        
        public TaskStateType TaskState
        {
            get => taskData.taskState;
            set => taskData.taskState = value;
        }
        
    }
    
    public List<TaskData_SO> tasks = new();


    public void AddTask(TaskData_SO taskData)
    {
        
    }
    
    //根据TaskName判断任务列表中是否已经有这个任务了
    public bool HaveTask(TaskData_SO taskData)
    {
        return false;
    }

    public GameTask GetTask(TaskData_SO taskData)
    {
        return null;
    }
}
