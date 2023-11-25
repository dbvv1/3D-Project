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
      //��������״̬���費ͬ��ʾ
      switch (currentTaskData.TaskState)
      {
         case TaskStateType.Started:
            taskNameText.text = taskData.taskName + "�������У�";
            break;
         case TaskStateType.Completed:
            taskNameText.text = taskData.taskName + "������ɣ�";
            break;
         case TaskStateType.Finished:
            taskNameText.text = taskData.taskName + "���ѽ�����";
            break;
      }
   }

   private void UpdateTaskContent()
   {
      //�����Ҳ���ı�����:�������飬����Ҫ��������
      TaskUIManager.Instance.SetUpTaskDescription(currentTaskData);
      TaskUIManager.Instance.SetupRequirementList(currentTaskData);
      TaskUIManager.Instance.SetupRewardList(currentTaskData);
   }
   
}
