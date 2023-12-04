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

    [SerializeField] private TaskNameButton taskNameButtonPrefab;
    
    [Header("任务Requirement")]
    [SerializeField] private  Transform requirementListTransform;
    [SerializeField] private  TaskRequirement requirementPrefab;

    [Header("任务Reward")] 
    [SerializeField] private GameObject rewardTitle;
    [SerializeField] private Transform rewardListTransform;
    [SerializeField] private ItemUI rewardUIPrefab;
    
    [Header("任务Description")]
    [SerializeField] private TextMeshProUGUI taskDescriptionText;
    
    
    public void OpenTaskPanel()
    {
        taskPanel.SetActive(true);
        taskDescriptionText.text=string.Empty;
        rewardTitle.SetActive(false);
        //显示面板内容
        SetupTaskList();
    }
    
    //设置UI左侧任务名称列表
    private void SetupTaskList()
    {
        //先删除所有不该显示的物体：ButtonList RequireList RewardList
        foreach (Transform taskButton in taskListTransform)
            Destroy(taskButton.gameObject);

        foreach (Transform requirement in requirementListTransform)
            Destroy(requirement.gameObject);

        foreach (Transform reward in rewardListTransform)
            Destroy(reward.gameObject);
        
        //通过TaskManager 新生成所有的task  并且进行初始化
        foreach (var task in TaskManager.Instance.tasks)
        {
            var taskButton = Instantiate(taskNameButtonPrefab, taskListTransform);
            taskButton.SetupNameButton(task);
        }
    }
    
    //设置UI右侧的任务详情
    public void SetUpTaskDescription(TaskData_SO taskData)
    {
        taskDescriptionText.text = taskData.taskDescription;
    }

    //设置UI右侧任务需求列表
    public void SetupRequirementList(TaskData_SO taskData)
    {
        foreach (Transform requirement in requirementListTransform)
            Destroy(requirement.gameObject);
        foreach (var requirement in taskData.taskRequires)
        {
            var requirementUI = Instantiate(requirementPrefab, requirementListTransform);
            if (taskData.TaskState == TaskStateType.Finished)
                requirementUI.SetupRequirement(requirement.requireName, true);
            else
                requirementUI.SetupRequirement(requirement.requireName, requirement.requireAmount, requirement.currentAmount);
        }
    }
    
    //设置UI右侧任务奖励列表
    public void SetupRewardList(TaskData_SO taskData)
    {
        foreach (Transform reward in rewardListTransform)
            Destroy(reward.gameObject);
        rewardTitle.SetActive(true);
        foreach (var reward in taskData.rewards)
        {
            var rewardItem = Instantiate(rewardUIPrefab, rewardListTransform);
            rewardItem.SetUpItemUI(reward.itemData, reward.itemAmount);
        }
    }
}


