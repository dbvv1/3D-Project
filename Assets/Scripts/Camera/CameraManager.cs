using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : Singleton<CameraManager>
{
    [Header("不同摄像机")]
    public GameObject firstPersonCamera;
    public GameObject thirdPersonNormalCamera;
    public GameObject thirdPersonCombatCamera;
    public GameObject thirdPersonTopDownCamera;

    public GameObject thirdPersonLockCamera;
    private ThirdLockEnemyCamera lockCamera;

    public CinemachineBrain cinemachineBrain;

    [Header("游戏状态")]
    public CameraStyle currentCameraStyle;          //当前摄像机所处的状态


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

    //对于按键的处理 规定按下C键进入战斗视角  V键进入第一视角 T键进入俯视角
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
        //设置鼠标的激活或隐藏 （第三人称战斗视角）
        if (newStyle == CameraStyle.ThirdPersonCombat||newStyle==CameraStyle.FirstPerson) MouseManager.Instance.ShowMouseCursor();
        else MouseManager.Instance.HideMouseCursor();

        //先设置所有相机为false 在选择激活新相机
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


    //实现一个遮挡剔除的效果：如果摄像机的视线被其他物体遮挡的话，则暂时将这些物体变为透明
    private void Transparent()
    {
        //射线方向为从摄像机朝着人物
        Transform player = GameManager.Instance.playerCurrentStats.transform;
        Vector3 direction = player.position - transform.position;//射线方向为摄像头指向人物
        float distance = Vector3.Distance(player.position, transform.position);
        RaycastHit[] hits;
        if (Physics.Raycast(transform.position, direction, out rayHitInfo))
        {
            //射线第一个击中的目标不是主角人物
            if(!rayHitInfo.transform.CompareTag("Player"))
            {
                hits = Physics.RaycastAll(transform.position, direction, distance+10);
                foreach (var hit in hits) 
                {
                    //如果射线已经击中了主角人物，直接返回即可
                    if (hit.transform.CompareTag("Player")) return;

                    //如果命中的物体存在Render
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
            //射线第一个击中的目标即是主角人物
            else
            {
                ClearTransparentObjects();
            }
        }
    }


    //取消原先物体的遮挡剔除
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
