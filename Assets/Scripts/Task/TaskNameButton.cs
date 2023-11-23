using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskNameButton : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI taskNameText;
   
   [SerializeField] private TextMeshProUGUI taskDescriptionText;
   
   private TaskData_SO currentTaskData;

   private void Awake()
   {
      GetComponent<Button>().onClick.AddListener(UpdateTaskContent);
   }


   public void SetupNameButton(TaskData_SO taskData)
   {
      currentTaskData = taskData;
      //根据任务状态给予不同显示
   }

   private void UpdateTaskContent()
   {
      //设置右侧的文本内容:任务详情，任务要求，任务奖励
   }
   
}
