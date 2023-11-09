using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

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
            anim.SetBool("IsOnGround", value);
        }
    }

    [Header("检测参数")]
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

    //判断角色前方是否有阻挡
    public bool haveBarrierInMoveDirectino(Vector3 origin,Vector3 direction,float distance)
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
