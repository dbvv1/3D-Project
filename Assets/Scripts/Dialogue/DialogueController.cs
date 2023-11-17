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
            //����Ի�
            OpenDialogue();
        }
    }

    private void OpenDialogue()
    {
        //��UI��� ���뵱ǰ��dialogueData
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
