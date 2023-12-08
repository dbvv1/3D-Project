using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PanelInfo
{
    public GameObject panel;

    public PanelType panelType;

    public string panelName;

    public PanelInfo(GameObject panel,PanelType panelType,string panelName)
    {
        this.panel = panel;
        this.panelType = panelType;
        this.panelName = panelName;
    }
    
}

public class UIManager : Singleton<UIManager>
{
    [Header("Panel����")] 
    [SerializeField] private GameObject menuImage;
    
    [SerializeField] private GameObject backGroundImage;

    [SerializeField] private GameObject characterStatsPanel;

    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private GameObject questTaskPanel;

    [SerializeField] private GameObject skillPanel;

    private readonly List<PanelInfo> panels = new();

    [Header("��������")] 
    [SerializeField] public Canvas enemyStatsBarCanvas;

    [SerializeField] private TextMeshProUGUI panelTitleText;
    [SerializeField] private TextMeshProUGUI leftPanelInfoText;
    [SerializeField] private TextMeshProUGUI rightPanelInfoText;

    [Header("���˵�����")] 
    [SerializeField] private GameSceneSO menuScene;

    public FadeCanvas fadeCanvas;

    private UIInputController inputActions;

    private PanelType currentPanel;

    private int currentPanelIndex = 0;

    private PanelType CurrentPanel
    {
        get => currentPanel;
        set
        {
            CloseAllPanels();
            //�趨��ɫ���ͱ������
            characterStatsPanel.SetActive(value != PanelType.None && value != PanelType.QuestTask);
            backGroundImage.SetActive(value != PanelType.None);
            MouseManager.Instance.SetMouseCursorByPanelType(value);
            if (value != PanelType.None)
            {
                GlobalEvent.CallStopTheWorldEvent();
            }
            else
            {
                GlobalEvent.CallContinueTheWorldEvent();
                InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
            }

            currentPanel = value;
            // �������ı�����Ϣ
            SetLeftAndRightPanelInfo();
            
            // ���ÿ������Ʒ����ʾ
            InventoryManager.Instance.SetActionContainerParent(currentPanel);

            if (currentPanel != PanelType.None) panels[currentPanelIndex].panel.SetActive(true);
            if(currentPanel==PanelType.QuestTask) TaskUIManager.Instance.OpenTaskPanel();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        menuImage.SetActive(true);
        inputActions = new UIInputController();
        // ��ʼ����Ҫ���е��������
        panels.Add(new PanelInfo(inventoryPanel, PanelType.Inventory, "��Ʒ�ͽ�ɫ"));
        panels.Add(new PanelInfo(questTaskPanel, PanelType.QuestTask, "����"));
        panels.Add(new PanelInfo(skillPanel, PanelType.Skill, "���ܺͽ�ɫ"));
    }

    private void Start()
    {
        CurrentPanel = PanelType.None;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        //����Tab�� ���� B �� ����UI Panel���� (ʹ��lambda���ʽ)
        inputActions.UI.OpenBagUI.started += _ =>
        {
            if (CurrentPanel != PanelType.None) CurrentPanel = PanelType.None;
            else
            {
                currentPanelIndex = 0;
                CurrentPanel = PanelType.Inventory;
            }
        };
        //����ESC�� �򿪽��� ���� �ر����н��� 
        inputActions.UI.CloseAllUI.started += _ =>
        {
            if (CurrentPanel != PanelType.None)
                CurrentPanel = PanelType.None;
            else
            {
                currentPanelIndex = 0;
                CurrentPanel = PanelType.Inventory;
            }
        };
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    
    // �ص����˵����
    public void ReturnToMainMenu()
    {
        DataManager.Instance.Save();
        Time.timeScale = 1;
        SceneLoader.Instance.SceneTransition(menuScene, Vector3.zero, true);
    }

    public void SetPanelAfterLoad(SceneType sceneType)
    {
        menuImage.SetActive(sceneType == SceneType.Persistent | sceneType == SceneType.Menu);
        if (sceneType == SceneType.RestScene | sceneType == SceneType.FightScene) CloseAllPanels();
    }

    
    // �ر����е����
    private void CloseAllPanels()
    {
        backGroundImage.SetActive(false);
        inventoryPanel.SetActive(false);
        characterStatsPanel.SetActive(false);
        questTaskPanel.SetActive(false);
        skillPanel.SetActive(false);
    }

    // �õ���һ��������Ϣ
    private int GetNextPanelIndex()
    {
        return (currentPanelIndex + 1) % panels.Count;
    }

    // �õ���һ��������Ϣ
    private int GetPrePanelIndex()
    {
        return (currentPanelIndex - 1 + panels.Count) % panels.Count;
    }
    
    // ��������������Ϣ
    private void SetLeftAndRightPanelInfo()
    {
        panelTitleText.text = panels[currentPanelIndex].panelName;
        leftPanelInfoText.text = panels[GetPrePanelIndex()].panelName;
        rightPanelInfoText.text = panels[GetNextPanelIndex()].panelName;
    }
    

    #region Button�ļ����¼�

    //���������һ�����
    public void OnClickNextPanelButton()
    {
        currentPanelIndex = GetNextPanelIndex();
        CurrentPanel = panels[currentPanelIndex].panelType;
    }

    //���������һ�����
    public void OnClickPreviousPanelButton()
    {
        currentPanelIndex = GetPrePanelIndex();
        CurrentPanel = panels[currentPanelIndex].panelType;
    }


    //��������Ĺرհ�ťʱ
    public void OnCloseButtonClicked()
    {
        CurrentPanel = PanelType.None;
        currentPanelIndex = 0;
    }

    #endregion
}