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

    private DialogueData_SO currentDialogueData; //������DialogueData ���ڲ�ѯ��һ���Ի�
    
    private DialoguePiece currentDialoguePiece;  //������DialoguePiece ���ڲ�ѯ�����������

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
        //�ж��Ƿ����������
        if (takeTask && currentDialoguePiece.taskData != null)
        {
            //���ص�TaskManager��
            //��������б����Ѿ��и�Task ��Ҫ�ж������Ƿ���� ���Ҹ��轱��
            
            
        }
        
        //��ת��ѡ�����һ��Ի�
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