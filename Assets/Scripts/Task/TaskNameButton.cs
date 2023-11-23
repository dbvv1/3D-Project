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
      //��������״̬���費ͬ��ʾ
   }

   private void UpdateTaskContent()
   {
      //�����Ҳ���ı�����:�������飬����Ҫ��������
   }
   
}
