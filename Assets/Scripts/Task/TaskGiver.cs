using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class TaskGiver : MonoBehaviour
{
   private DialogueController dialogueController;
   [SerializeField]private TaskData_SO currentTaskData;

   [Header("不同任务状态下的对话")] 
   [SerializeField] private DialogueData_SO startDialogueData;
   [SerializeField] private DialogueData_SO progressDialogueData;
   [SerializeField] private DialogueData_SO CompleteDialogueData;
   [SerializeField] private DialogueData_SO FinishDialogueData;

   public TaskStateType TaskState
   {
      get
      {
         var task = TaskManager.Instance.GetTask(currentTaskData);
         if (task == null) return TaskStateType.NotStarted;
         return task.TaskState;
      }
   }
   
   private void Awake()
   {
      dialogueController=GetComponent<DialogueController>();
   }

   private void Start()
   {
      dialogueController.currentDialogueData = startDialogueData;
   }

   //TODO：暂时在Update中检测任务的状态并且更新NPC的对话内容
   private void Update()
   {
      dialogueController.currentDialogueData = TaskState switch
      {
         TaskStateType.NotStarted => startDialogueData,
         TaskStateType.Started => progressDialogueData,
         TaskStateType.Completed => CompleteDialogueData,
         TaskStateType.Finished => FinishDialogueData,
         _ => null
      };
   }
}
