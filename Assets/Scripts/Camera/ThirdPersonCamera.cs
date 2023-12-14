using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//第三人称视角下 摄像机的行为
public class ThirdPersonCamera : MonoBehaviour
{
    [Header("组件")]
    public Transform faceDirection;

    public Transform combatLookAt;

    public Transform player;

    private CinemachineFreeLook cinemachine;

    private void Awake()
    {
        cinemachine=GetComponent<CinemachineFreeLook>();
    }

    private void OnEnable()
    {
        //从第一人称到第三人称的时候 需要调整   XAxis.Value  横向
        //从锁定人称到第三人称的时候 需要调整   YAxis.Value  纵向
        float angle = Vector3.Angle(player.forward, Vector3.forward);
        if (Vector3.Cross(player.forward, Vector3.forward).y > 0) angle *= -1;
        cinemachine.m_XAxis.Value = angle;
        cinemachine.m_YAxis.Value = 0.7f;
    }

    private void LateUpdate()
    {
        //faceDirection.position = new Vector3(transform.position.x, cinemachine.LookAt.position.y, transform.position.z);
        //faceDirection.LookAt(cinemachine.LookAt);
        // 第三人称普通视角和俯视角
        if (CameraManager.Instance.currentCameraStyle == CameraStyle.ThirdPersonNormal || CameraManager.Instance.currentCameraStyle == CameraStyle.ThirdPersonTopDown) 
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            faceDirection.forward = viewDir.normalized;
        }
        // 第三人称战斗视角
        else if(CameraManager.Instance.currentCameraStyle==CameraStyle.ThirdPersonCombat)
        {
            Vector3 viewDir = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            faceDirection.forward = viewDir.normalized;
        }
    }
}
