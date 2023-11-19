using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUIManager : Singleton<DialogueUIManager>
{
    [Header("引用")] [SerializeField] private Image dialogueIcon;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;

    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private Transform optionPanel;
    
    [SerializeField] private OptionUI optionPrefab;

    public bool IsTalking { get; set; }

    private DialogueData_SO currentDialogueData;

    private int currentIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        //第一种继续对话的方式：点击Next按钮
        nextButton.onClick.AddListener(ContinueDialogue);
    }

    private void Update()
    {
        //第二种继续对话的方式：按下E键 (要在没有选项的前提下)
        if (IsTalking && Input.GetKeyDown(KeyCode.E) && nextButton.gameObject.activeInHierarchy)
        {
            ContinueDialogue();
        }
    }

    public void OpenDialogue(DialogueData_SO dialogueData)
    {
        currentDialogueData = dialogueData;
        currentIndex = 0;
        IsTalking = true;
        dialoguePanel.SetActive(true);
        UpdateMainDialogue(currentDialogueData.dialoguePieces[0]);
    }

    public void CloseDialogue()
    {
        currentDialogueData = null;
        currentIndex = 0;
        IsTalking = false;
        dialoguePanel.SetActive(false);
    }

    //返回指定名字对话所在DialogueData中的下标
    public int GetDialoguePiecesByName(string name)
    {
        return currentDialogueData.stringToDialoguePiece[name];
    }

    public void UpdateMainDialogue(DialoguePiece dialoguePiece)
    {
        //更新图片内容
        if (dialoguePiece.image != null)
        {
            dialogueIcon.sprite = dialoguePiece.image;
            dialogueIcon.enabled = true;
        }
        else dialogueIcon.enabled = false;

        //更新文本内容
        var t = DOTween.To(() => string.Empty, value => dialogueText.text = value, dialoguePiece.text, 1f)
                .SetEase(Ease.Linear);

        //处理NextButton(只有当没有选项时才选择有NextButton)
        if (dialoguePiece.options.Count == 0)
        {
            nextButton.gameObject.SetActive(true);
        }
        else nextButton.gameObject.SetActive(false);

        currentIndex = currentDialogueData.stringToDialoguePiece[dialoguePiece.ID] + 1;

        //创建Options
        CreateOptions(dialoguePiece);
    }

    private void CreateOptions(DialoguePiece dialogPieces)
    {
        //先销毁原有的Option 后创建新的Option
        foreach (Transform child in optionPanel) Destroy(child.gameObject);

        foreach (var option in dialogPieces.options)
        {
            var optionUI = Instantiate(optionPrefab, optionPanel);
            optionUI.SettingOption(currentDialogueData, option);
        }
        
    }

    private void ContinueDialogue()
    {
        if (currentIndex <= currentDialogueData.dialoguePieces.Count - 1)
            UpdateMainDialogue(currentDialogueData.dialoguePieces[currentIndex]);
        else
            CloseDialogue();
    }
}