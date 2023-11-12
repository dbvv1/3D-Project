using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : Singleton<CameraManager>
{
    [Header("��ͬ�����")]
    public GameObject firstPersonCamera;
    public GameObject thirdPersonNormalCamera;
    public GameObject thirdPersonCombatCamera;
    public GameObject thirdPersonTopDownCamera;

    public GameObject thirdPersonLockCamera;
    private ThirdLockEnemyCamera lockCamera;

    public CinemachineBrain cinemachineBrain;

    [Header("��Ϸ״̬")]
    public CameraStyle currentCameraStyle;          //��ǰ�����������״̬


    private RaycastHit rayHitInfo;

    private List<Renderer> transparentObjects = new List<Renderer>();

    protected override void Awake()
    {
        base.Awake();
        lockCamera = thirdPersonLockCamera.GetComponent<ThirdLockEnemyCamera>();
    }

    private void Start()
    {
        SwitchCameraStyle(CameraStyle.ThirdPersonNormal);
        cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseIn;
    }

    private void Update()
    {
        //Transparent();

        KeyProcessing();
    }

    //���ڰ����Ĵ��� �涨����C������ս���ӽ�  V�������һ�ӽ� T�����븩�ӽ�
    private void KeyProcessing()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (currentCameraStyle != CameraStyle.FirstPerson)
                SwitchCameraStyle(CameraStyle.FirstPerson);
            else SwitchCameraStyle(CameraStyle.ThirdPersonNormal);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCameraStyle(CameraStyle.ThirdPersonCombat);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchCameraStyle(CameraStyle.ThirdPersonTopDown);
        }
    }

    public void SwitchCameraStyle(CameraStyle newStyle)
    {
        if (currentCameraStyle == newStyle) return;
        //�������ļ�������� �������˳�ս���ӽǣ�
        if (newStyle == CameraStyle.ThirdPersonCombat||newStyle==CameraStyle.FirstPerson) MouseManager.Instance.ShowMouseCursor();
        else MouseManager.Instance.HideMouseCursor();

        //�������������Ϊfalse ��ѡ�񼤻������
        SetAllCameraFalse();

        if (newStyle == CameraStyle.FirstPerson || currentCameraStyle == CameraStyle.FirstPerson) 
            cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        else
            cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseIn;
        switch (newStyle)
        {
            case CameraStyle.FirstPerson:
                firstPersonCamera.SetActive(true);
                GlobalEvent.CallSwitchToFirstPersonEvent();
                break;
            case CameraStyle.ThirdPersonNormal:
                thirdPersonNormalCamera.SetActive(true);
                break;
            case CameraStyle.ThirdPersonCombat:
                thirdPersonCombatCamera.SetActive(true);
                break;
            case CameraStyle.ThirdPersonTopDown:
                thirdPersonTopDownCamera.SetActive(true);
                break;
            case CameraStyle.None:
                break;
        }
        currentCameraStyle = newStyle;
    }

    public void SwitchToThirdPersonLockEnemy(EnemyController currentEnemy)
    {
        if (currentCameraStyle == CameraStyle.ThirdPersonLock) return;
        SetAllCameraFalse();
        thirdPersonLockCamera.SetActive(true);
        lockCamera.Init(currentEnemy);
        currentCameraStyle = CameraStyle.ThirdPersonLock;
    }

    private void SetAllCameraFalse()
    {
        firstPersonCamera.SetActive(false);
        thirdPersonNormalCamera.SetActive(false);
        thirdPersonCombatCamera.SetActive(false);
        thirdPersonTopDownCamera.SetActive(false);
        thirdPersonLockCamera.SetActive(false);
    }


    //ʵ��һ���ڵ��޳���Ч�����������������߱����������ڵ��Ļ�������ʱ����Щ�����Ϊ͸��
    private void Transparent()
    {
        //���߷���Ϊ���������������
        Transform player = GameManager.Instance.playerCurrentStats.transform;
        Vector3 direction = player.position - transform.position;//���߷���Ϊ����ͷָ������
        float distance = Vector3.Distance(player.position, transform.position);
        RaycastHit[] hits;
        if (Physics.Raycast(transform.position, direction, out rayHitInfo))
        {
            //���ߵ�һ�����е�Ŀ�겻����������
            if(!rayHitInfo.transform.CompareTag("Player"))
            {
                hits = Physics.RaycastAll(transform.position, direction, distance+10);
                foreach (var hit in hits) 
                {
                    //��������Ѿ��������������ֱ�ӷ��ؼ���
                    if (hit.transform.CompareTag("Player")) return;

                    //������е��������Render
                    if(hit.transform.TryGetComponent<Renderer>(out var renderer))
                    {
                        foreach(var m in renderer.materials)
                        {
                            Color changeColor = m.color; changeColor.a = 0.2f;
                            m.color = changeColor;
                        }
                        transparentObjects.Add(renderer);
                    }
                }
            }
            //���ߵ�һ�����е�Ŀ�꼴����������
            else
            {
                ClearTransparentObjects();
            }
        }
    }


    //ȡ��ԭ��������ڵ��޳�
    private void ClearTransparentObjects()
    {
        if(transparentObjects!=null)
        {
            foreach(var r in transparentObjects)
            {
                foreach(var m in r.materials)
                {
                    Color changeColor = m.color; changeColor.a = 1f;
                    m.color = changeColor;
                }
            }
            transparentObjects.Clear();
        }
       
    }

}
