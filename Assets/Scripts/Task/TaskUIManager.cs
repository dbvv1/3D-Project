using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskUIManager : Singleton<TaskUIManager>
{
    [Header("����")] 
    [SerializeField] private GameObject taskPanel;
    
    [SerializeField] public Tooltip itemTooltip;

    [Header("����Button")]
    [SerializeField] private Transform taskListTransform;

    [SerializeField] private TaskNameButton taskNameButtonPrefab;
    
    [Header("����Requirement")]
    [SerializeField] private  Transform requirementListTransform;
    [SerializeField] private  TaskRequirement requirementPrefab;

    [Header("����Reward")] 
    [SerializeField] private GameObject rewardTitle;
    [SerializeField] private Transform rewardListTransform;
    [SerializeField] private ItemUI rewardUIPrefab;
    
    [Header("����Description")]
    [SerializeField] private TextMeshProUGUI taskDescriptionText;
    
    
    public void OpenTaskPanel()
    {
        taskPanel.SetActive(true);
        taskDescriptionText.text=string.Empty;
        rewardTitle.SetActive(false);
        //��ʾ�������
        SetupTaskList();
    }
    
    //����UI������������б�
    private void SetupTaskList()
    {
        //��ɾ�����в�����ʾ�����壺ButtonList RequireList RewardList
        foreach (Transform taskButton in taskListTransform)
            Destroy(taskButton.gameObject);

        foreach (Transform requirement in requirementListTransform)
            Destroy(requirement.gameObject);

        foreach (Transform reward in rewardListTransform)
            Destroy(reward.gameObject);
        
        //ͨ��TaskManager ���������е�task  ���ҽ��г�ʼ��
        foreach (var task in TaskManager.Instance.tasks)
        {
            var taskButton = Instantiate(taskNameButtonPrefab, taskListTransform);
            taskButton.SetupNameButton(task);
        }
    }
    
    //����UI�Ҳ����������
    public void SetUpTaskDescription(TaskData_SO taskData)
    {
        taskDescriptionText.text = taskData.taskDescription;
    }

    //����UI�Ҳ����������б�
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
    
    //����UI�Ҳ��������б�
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


