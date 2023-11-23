using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��һ�˳��ӽ������������Ϊ
public class FirstPersonCamera : MonoBehaviour
{
    [Header("��һ�˳��ӽ���� ����")]
    public float senX;      //X��������
    public float senY;      //Y��������

    [Header("���")]
    public Transform playerFaceDirection;  //��������ķ���

    public Transform camPosition;

    private PlayerController playerController;

    private MouseController mouseInput;

    private float xRotation;

    private float yRotation;

    private void Awake()
    {
        mouseInput = new MouseController();
        playerController = playerFaceDirection.GetComponentInParent<PlayerController>();
    }

    private void OnEnable()
    {
        mouseInput.Enable();
        GlobalEvent.switchToFirstPersonEvent += OnSwitchToFirstPerson;
    }

    private void OnDisable()
    {
        mouseInput.Disable();
        GlobalEvent.switchToFirstPersonEvent -= OnSwitchToFirstPerson;
    }

    private void OnSwitchToFirstPerson()
    {
        //�ڴӵ����˳�ת����һ�˳Ƶ�ʱ�� ��Ҫ����yRotation
        yRotation = Vector3.Angle(playerFaceDirection.forward, Vector3.forward);
        if (Vector3.Cross(playerFaceDirection.forward, Vector3.forward).y > 0) yRotation *= -1;
    }

    private void Update()
    {
        transform.position = camPosition.position;
        Vector2 mouse = mouseInput.GamePlay.MouseMove.ReadValue<Vector2>();
        yRotation += mouse.x * Time.deltaTime * senX; //��y�����ת���������ң�
        xRotation -= mouse.y * Time.deltaTime * senY; //��x�����ת���������£�

        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        //����������ĳ��� ѡ��1��ʼ�ճ��������ָ�ķ���   ѡ��2��
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //transform.forward = camPosition.forward;

        playerFaceDirection.rotation = Quaternion.Euler(0, yRotation, 0); //���������泯�ķ���               
    }
}
