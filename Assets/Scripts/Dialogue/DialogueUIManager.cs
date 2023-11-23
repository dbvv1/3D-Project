using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUIManager : Singleton<DialogueUIManager>
{
    [Header("����")] [SerializeField] private Image dialogueIcon;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;

    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private Transform optionPanel;
    
    [SerializeField] private OptionUI optionPrefab;

    public bool IsTalking { get; set; }  //��ǰ�Ƿ����ڶԻ�

    private DialogueData_SO currentDialogueData;

    private int currentIndex;        //��ǰ�Ի����е���pieces�е�λ��

    protected override void Awake()
    {
        base.Awake();
        //��һ�ּ����Ի��ķ�ʽ�����Next��ť
        nextButton.onClick.AddListener(ContinueDialogue);
    }

    private void Update()
    {
        //�ڶ��ּ����Ի��ķ�ʽ������E�� (Ҫ��û��ѡ���ǰ����)
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
        //���ڿ����Ի��ͽ����Ի��п�����һ������ �п��ܻ��ڽ����Ի���˲������Ի� �������һ��Э�̵��ӳٵ���(�ӳ�һ֡����)
        StartCoroutine(SetIsTalking(false));
        GlobalEvent.CallOnExitDialogue();
    }

    //����ָ�����ֶԻ�����DialogueData�е��±�
    public int GetDialoguePiecesByName(string pieceID)
    {
        return currentDialogueData.stringToDialoguePiece[pieceID];
    }

    public void UpdateMainDialogue(DialoguePiece dialoguePiece)
    {
        //����ͼƬ����
        if (dialoguePiece.image != null)
        {
            dialogueIcon.sprite = dialoguePiece.image;
            dialogueIcon.enabled = true;
        }
        else dialogueIcon.enabled = false;

        //�����ı�����
        //dialogueText.text = dialoguePiece.text;
        var t = DOTween.To(() => string.Empty, value => dialogueText.text = value, dialoguePiece.text, 1f).SetEase(Ease.Linear);
        t.SetOptions(true);
        //����NextButton(ֻ�е�û��ѡ��ʱ��ѡ����NextButton)
        nextButton.gameObject.SetActive(dialoguePiece.options.Count == 0);

        currentIndex = currentDialogueData.stringToDialoguePiece[dialoguePiece.ID] + 1;

        //����Options
        CreateOptions(dialoguePiece);
    }

    private void CreateOptions(DialoguePiece dialoguePiece)
    {
        //������ԭ�е�Option �󴴽��µ�Option
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