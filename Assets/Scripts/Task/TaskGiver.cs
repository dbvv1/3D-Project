using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(DialogueController))]
public class TaskGiver : MonoBehaviour
{
   private DialogueController dialogueController;
   [SerializeField]private TaskData_SO currentTaskData;

   [Header("不同任务状态下的对话")] 
   [SerializeField] private DialogueData_SO startDialogueData;
   [SerializeField] private DialogueData_SO progressDialogueData;
   [SerializeField] private DialogueData_SO completeDialogueData;
   [SerializeField] private DialogueData_SO finishDialogueData;
   
   public TaskStateType TaskState
   {
      get
      {
         var taskOnGame = TaskManager.Instance.GetTask(currentTaskData);
         return taskOnGame == null ? TaskStateType.Started : taskOnGame.TaskState;
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

   //在Update中检测任务的状态并且更新NPC的对话内容
   private void Update()
   {
      dialogueController.currentDialogueData = TaskState switch
      {
         TaskStateType.NotStarted => startDialogueData,
         TaskStateType.Started => progressDialogueData,
         TaskStateType.Completed => completeDialogueData,
         TaskStateType.Finished => finishDialogueData,
         _ => null
      };
   }
}
