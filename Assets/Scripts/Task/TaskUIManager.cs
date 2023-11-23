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

    [SerializeField] private TaskNameButton taskNameButton;
    
    [Header("����Requirement")]
    [SerializeField] private  Transform requirementListTransform;
    [SerializeField] private  TaskRequirement requirementPrefab;
    
    [Header("����Description")]
    [SerializeField] private TextMeshProUGUI taskContentText;

    [SerializeField] private ItemUI rewardUI;
    
    public void OpenTaskPanel()
    {
        taskPanel.SetActive(true);
        taskContentText.text=string.Empty;
        //��ʾ�������
        SetupTaskList();
    }

    private void SetupTaskList()
    {
        //��ɾ�����в�����ʾ�����壺ButtonList RequireList RewardList
        
        //ͨ��TaskManager ���������е�task  ���ҽ��г�ʼ��
    }

    private void SetupRequirementList(TaskData_SO taskData)
    {
        
    }
    
    private void SetupRewardList(TaskData_SO taskData)
    {
        
    }
}


