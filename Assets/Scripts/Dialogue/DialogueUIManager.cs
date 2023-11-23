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

    public bool IsTalking { get; set; }  //当前是否正在对话

    private DialogueData_SO currentDialogueData;

    private int currentIndex;        //当前对话进行到的pieces中的位置

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
        GlobalEvent.CallOnEnterDialogue();
    }

    public void CloseDialogue()
    {
        currentDialogueData = null;
        currentIndex = 0;
        dialoguePanel.SetActive(false);
        //由于开启对话和结束对话有可能是一个按键 有可能会在结束对话的瞬间后开启对话 因此设置一个协程的延迟调用(延迟一帧即可)
        StartCoroutine(SetIsTalking(false));
        GlobalEvent.CallOnExitDialogue();
    }

    //返回指定名字对话所在DialogueData中的下标
    public int GetDialoguePiecesByName(string pieceID)
    {
        return currentDialogueData.stringToDialoguePiece[pieceID];
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
        //dialogueText.text = dialoguePiece.text;
        var t = DOTween.To(() => string.Empty, value => dialogueText.text = value, dialoguePiece.text, 1f).SetEase(Ease.Linear);
        t.SetOptions(true);
        //处理NextButton(只有当没有选项时才选择有NextButton)
        nextButton.gameObject.SetActive(dialoguePiece.options.Count == 0);

        currentIndex = currentDialogueData.stringToDialoguePiece[dialoguePiece.ID] + 1;

        //创建Options
        CreateOptions(dialoguePiece);
    }

    private void CreateOptions(DialoguePiece dialoguePiece)
    {
        //先销毁原有的Option 后创建新的Option
        foreach (Transform child in optionPanel) Destroy(child.gameObject);

        foreach (var option in dialoguePiece.options)
        {
            var optionUI = Instantiate(optionPrefab, optionPanel);
            optionUI.SettingOption(currentDialogueData, dialoguePiece, option);
        }
        
    }

    private void ContinueDialogue()
    {
        if (currentIndex <= currentDialogueData.dialoguePieces.Count - 1)
            UpdateMainDialogue(currentDialogueData.dialoguePieces[currentIndex]);
        else
            CloseDialogue();
    }

    private IEnumerator SetIsTalking(bool value)
    {
        yield return null;
        IsTalking = value;
    }
}