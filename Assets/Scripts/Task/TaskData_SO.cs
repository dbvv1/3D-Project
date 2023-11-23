using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task",menuName = "Task/Task Data")]
public class TaskData_SO : ScriptableObject
{
    [System.Serializable]
    public class TaskRequire
    {
        //TODO:暂时以目标的名字作为判断
        public string requireName;
        
        public int requireAmount;

        public int currentAmount;

    }
    
    public string taskName;
    [TextArea]
    public string taskDescription;

    public TaskStateType taskState;

    public List<TaskRequire> taskRequires = new();

    public List<InventoryItem> rewards = new();
}
