using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO dialogueData;

    private bool canTalk;

    private void Update()
    {
        if (canTalk && Input.GetKeyDown(KeyCode.E))
        {
            //进入对话
            OpenDialogue();
        }
    }

    private void OpenDialogue()
    {
        //打开UI面板 传入当前的dialogueData
        DialogueUIManager.Instance.SettingDialogueData(dialogueData);
        DialogueUIManager.Instance.OpenDialogue();
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
        canTalk = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canTalk = false;
    }
}
