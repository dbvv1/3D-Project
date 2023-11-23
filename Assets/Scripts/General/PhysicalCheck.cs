using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//角色的物理检测相关
public class PhysicalCheck : MonoBehaviour
{
    private Animator anim;

    [Header("物理状态")]
    public bool isOnGround;
    public bool IsOnGround {
        get => isOnGround;
        set
        {
            isOnGround = value;
            anim.SetBool(OnGround, value);
        }
    }

    [Header("检测参数")]
    public LayerMask groundLayer;

    public LayerMask barrierLayer;

    public LayerMask enemyLayer;

    public Vector3 bottomOffset;

    public float groundCheckRadius;
    private static readonly int OnGround = Animator.StringToHash("IsOnGround");


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        IsOnGround = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer);
        IsOnGround |= Physics.CheckSphere(transform.position, groundCheckRadius, enemyLayer);
    }

    //判断角色前方是否有阻挡
    public bool HaveBarrierInMoveDirection(Vector3 origin,Vector3 direction,float distance)
    {
        return Physics.Raycast(origin, direction, distance, enemyLayer) || Physics.Raycast(origin, direction, distance, barrierLayer);
    }

    //可视化
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position+ transform.up * 0.5f, transform.forward);
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }

    
}
