using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����˳��ӽ��� ���������Ϊ
public class ThirdPersonCamera : MonoBehaviour
{
    [Header("���")]
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
        //�ӵ�һ�˳Ƶ������˳Ƶ�ʱ�� ��Ҫ����   XAxis.Value  ����
        //�������˳Ƶ������˳Ƶ�ʱ�� ��Ҫ����   YAxis.Value  ����
        float angle = Vector3.Angle(player.forward, Vector3.forward);
        if (Vector3.Cross(player.forward, Vector3.forward).y > 0) angle *= -1;
        cinemachine.m_XAxis.Value = angle;
        cinemachine.m_YAxis.Value = 0.7f;
    }

    private void LateUpdate()
    {
        //faceDirection.position = new Vector3(transform.position.x, cinemachine.LookAt.position.y, transform.position.z);
        //faceDirection.LookAt(cinemachine.LookAt);
        // �����˳���ͨ�ӽǺ͸��ӽ�
        if (CameraManager.Instance.currentCameraStyle == CameraStyle.ThirdPersonNormal || CameraManager.Instance.currentCameraStyle == CameraStyle.ThirdPersonTopDown) 
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            faceDirection.forward = viewDir.normalized;
        }
        // �����˳�ս���ӽ�
        else if(CameraManager.Instance.currentCameraStyle==CameraStyle.ThirdPersonCombat)
        {
            Vector3 viewDir = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            faceDirection.forward = viewDir.normalized;
        }
    }
}
