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

    private DialogueData_SO currentDialogueData;

    private string nextPieceID;

    private void Awake()
    {
        thisButton.onClick.AddListener(OnOptionClicked);
    }

    public void SettingOption(DialogueData_SO dialogueData, DialogueOption option)
    {
        currentDialogueData = dialogueData;
        optionText.text = option.text;
        nextPieceID = option.targetID;
    }

    private void OnOptionClicked()
    {
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