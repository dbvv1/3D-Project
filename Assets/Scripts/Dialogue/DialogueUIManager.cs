using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIManager : Singleton<DialogueUIManager>
{
    [Header("ÒýÓÃ")] 
    public Image dialogueIcon;
    
    public TextMeshProUGUI dialogueText;

    public Button nextButton;

    public GameObject dialoguePanel;
    
    private DialogueData_SO currentDialogueData;

    private int currentIndex = 0;

    private void Update()
    {
        
    }

    public void OpenDialogue()
    {
        dialoguePanel.SetActive(true);
    }
    
    public void SettingDialogueData(DialogueData_SO dialogueData)
    {
        currentDialogueData = dialogueData;
        currentIndex = 0;
    }

    private void UpdateMainDialogue()
    {
        
    }

}
