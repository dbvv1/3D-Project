using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI optionText;

    [SerializeField]private Button thisButton;

    private DialogueData_SO currentDialogueData; //所属的DialogueData 用于查询下一条对话
    
    private DialoguePiece currentDialoguePiece;  //所属的DialoguePiece 用于查询任务相关内容

    private string nextPieceID;

    private bool takeTask;

    private void Awake()
    {
        thisButton.onClick.AddListener(OnOptionClicked);
    }

    public void SettingOption(DialogueData_SO dialogueData, DialoguePiece dialoguePiece,DialogueOption option)
    {
        currentDialogueData = dialogueData;
        currentDialoguePiece = dialoguePiece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
        takeTask = option.takeTask;
    }

    private void OnOptionClicked()
    {
        //判断是否接受了任务
        if (takeTask && currentDialoguePiece.taskData != null)
        {
            var gameTask = TaskManager.Instance.GetTask(currentDialoguePiece.taskData);
            //如果任务列表中已经有该Task 则要判断任务是否完成 并且给予奖励
            if (gameTask != null) 
            {
                if (gameTask.TaskState == TaskStateType.Completed)
                {
                    gameTask.TaskState = TaskStateType.Finished;
                    //给予任务的奖励
                    
                }
            }
            //任务列表中没有该Task 则直接加入
            else
            {
                TaskManager.Instance.AddTask(currentDialoguePiece.taskData);
                //初始化进度：对于所有的需求，检测背包中的物品
                foreach (var taskRequire in currentDialoguePiece.taskData.taskRequires)
                {
                    InventoryManager.Instance.CheckTaskItemInBag(taskRequire.requireName);
                }
            }
            
        }
        
        //跳转到选项的下一句对话
        if (nextPieceID == string.Empty)
        {
            DialogueUIManager.Instance.CloseDialogue();
        }
        else
        {
            var targetPlace = DialogueUIManager.Instance.GetDialoguePiecesByName(nextPieceID);
            DialogueUIManager.Instance.UpdateMainDialogue(currentDialogueData.dialoguePieces[targetPlace]);
        }
    }
}