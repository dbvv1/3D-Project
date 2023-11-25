using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentDialogueData;

    private bool canTalk = false;

    private void Update()
    {
        if (canTalk && Input.GetKeyDown(KeyCode.E) && !DialogueUIManager.Instance.IsTalking) 
        {
            //进入对话
            OpenDialogue();
        }
    }

    private void OpenDialogue()
    {
        //打开UI面板 传入当前的dialogueData
        DialogueUIManager.Instance.OpenDialogue(currentDialogueData);
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = false;
            DialogueUIManager.Instance.CloseDialogue();
        }
    }
}
