using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//第一人称视角下摄像机的行为
public class FirstPersonCamera : MonoBehaviour
{
    [Header("第一人称视角相机 参数")]
    public float senX;      //X轴灵敏度
    public float senY;      //Y轴灵敏度

    [Header("组件")]
    public Transform playerFaceDirection;  //人物面向的方向

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
        //在从第三人称转到第一人称的时候 需要调整yRotation
        yRotation = Vector3.Angle(playerFaceDirection.forward, Vector3.forward);
        if (Vector3.Cross(playerFaceDirection.forward, Vector3.forward).y > 0) yRotation *= -1;
    }

    private void Update()
    {
        transform.position = camPosition.position;
        Vector2 mouse = mouseInput.GamePlay.MouseMove.ReadValue<Vector2>();
        yRotation += mouse.x * Time.deltaTime * senX; //绕y轴的旋转（控制左右）
        xRotation -= mouse.y * Time.deltaTime * senY; //绕x轴的旋转（控制上下）

        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        //调整摄像机的朝向 选择1：始终朝向鼠标所指的方向   选择2：
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //transform.forward = camPosition.forward;

        playerFaceDirection.rotation = Quaternion.Euler(0, yRotation, 0); //调整人物面朝的方向               
    }
}
