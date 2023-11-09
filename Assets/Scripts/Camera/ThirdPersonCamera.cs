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
        //在从第一人称转到第三人称的时候 需要调整 XAxis.Value
        float angle = Vector3.Angle(player.forward, Vector3.forward);
        if (Vector3.Cross(player.forward, Vector3.forward).y > 0) angle *= -1;
        cinemachine.m_XAxis.Value = angle;
    }

    private void Update()
    {
        //faceDirection.position = new Vector3(transform.position.x, cinemachine.LookAt.position.y, transform.position.z);
        //faceDirection.LookAt(cinemachine.LookAt);
        if (CameraManager.Instance.currentCameraStyle == CameraStyle.ThirdPersonNormal || CameraManager.Instance.currentCameraStyle == CameraStyle.ThirdPersonTopDown) 
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            faceDirection.forward = viewDir.normalized;
        }
        else if(CameraManager.Instance.currentCameraStyle==CameraStyle.ThirdPersonCombat)
        {
            Vector3 viewDir = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            faceDirection.forward = viewDir.normalized;
        }
    }
}
