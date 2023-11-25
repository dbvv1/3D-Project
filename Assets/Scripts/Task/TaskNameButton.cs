using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskNameButton : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI taskNameText;
   
   
   private TaskData_SO currentTaskData;

   private void Awake()
   {
      GetComponent<Button>().onClick.AddListener(UpdateTaskContent);
   }


   public void SetupNameButton(TaskData_SO taskData)
   {
      taskNameText.text = taskData.taskName;
      currentTaskData = taskData;
      //根据任务状态给予不同显示
      switch (currentTaskData.TaskState)
      {
         case TaskStateType.Started:
            taskNameText.text = taskData.taskName + "（进行中）";
            break;
         case TaskStateType.Completed:
            taskNameText.text = taskData.taskName + "（已完成）";
            break;
         case TaskStateType.Finished:
            taskNameText.text = taskData.taskName + "（已结束）";
            break;
      }
   }

   private void UpdateTaskContent()
   {
      //设置右侧的文本内容:任务详情，任务要求，任务奖励
      TaskUIManager.Instance.SetUpTaskDescription(currentTaskData);
      TaskUIManager.Instance.SetupRequirementList(currentTaskData);
      TaskUIManager.Instance.SetupRewardList(currentTaskData);
   }
   
}
