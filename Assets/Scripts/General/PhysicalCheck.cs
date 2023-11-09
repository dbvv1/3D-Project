using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

//��ɫ�����������
public class PhysicalCheck : MonoBehaviour
{
    private Animator anim;

    [Header("����״̬")]
    public bool isOnGround;
    public bool IsOnGround {
        get => isOnGround;
        set
        {
            isOnGround = value;
            anim.SetBool("IsOnGround", value);
        }
    }

    [Header("������")]
    public LayerMask groundLayer;

    public LayerMask barrierLayer;

    public LayerMask enemyLayer;

    public Vector3 buttomOffset;

    public float groundCheckRadius;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        IsOnGround = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer);
        IsOnGround |= Physics.CheckSphere(transform.position, groundCheckRadius, enemyLayer);
    }

    //�жϽ�ɫǰ���Ƿ����赲
    public bool haveBarrierInMoveDirectino(Vector3 origin,Vector3 direction,float distance)
    {
        return Physics.Raycast(origin, direction, distance, enemyLayer) || Physics.Raycast(origin, direction, distance, barrierLayer);
    }

    //���ӻ�
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position+ transform.up * 0.5f, transform.forward);
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }

    
}
