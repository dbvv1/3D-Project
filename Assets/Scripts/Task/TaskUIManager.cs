using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskUIManager : Singleton<TaskUIManager>
{
    [Header("引用")] 
    [SerializeField] private GameObject taskPanel;
    
    [SerializeField] public Tooltip itemTooltip;

    [Header("任务Button")]
    [SerializeField] private Transform taskListTransform;

    [SerializeField] private TaskNameButton taskNameButton;
    
    [Header("任务Requirement")]
    [SerializeField] private  Transform requirementListTransform;
    [SerializeField] private  TaskRequirement requirementPrefab;
    
    [Header("任务Description")]
    [SerializeField] private TextMeshProUGUI taskContentText;

    [SerializeField] private ItemUI rewardUI;
    
    public void OpenTaskPanel()
    {
        taskPanel.SetActive(true);
        taskContentText.text=string.Empty;
        //显示面板内容
        SetupTaskList();
    }

    private void SetupTaskList()
    {
        //先删除所有不该显示的物体：ButtonList RequireList RewardList
        
        //通过TaskManager 新生成所有的task  并且进行初始化
    }

    private void SetupRequirementList(TaskData_SO taskData)
    {
        
    }
    
    private void SetupRewardList(TaskData_SO taskData)
    {
        
    }
}


