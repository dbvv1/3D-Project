using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemRock : MonoBehaviour
{
    public enum RockState
    {
        HitPlayer,
        HitEnemy,
        HitNothing
    }
    
    private Rigidbody rb;
    private MeshCollider col;
    
    [Header("石块属性")]
    [SerializeField] private float force;

    [SerializeField] private GameObject rockBreakEffect;

    private RockState CurrentRockState { get; set; }

    private AttackDefinition attackDefinition;

    private GameObject target;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        attackDefinition = GetComponent<AttackDefinition>();
    }
    

    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1f)
            CurrentRockState = RockState.HitNothing;
    }

    public void InitAfterCreate(GolemController golemController)
    {
        target = golemController.player.gameObject;
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(100, 100, 100);
        rb.isKinematic = true;
        attackDefinition.attacker = golemController.enemyCharacterStats;
        CurrentRockState = RockState.HitNothing;
    }

    public void InitAfterFly(GolemController golemController)
    {
        rb.isKinematic = false;
        transform.SetParent(GOPoolManager.Instance.poolObjectParent);
        FlyToTarget();
        rb.velocity = Vector3.one;
        CurrentRockState = RockState.HitPlayer;
    }


    private void FlyToTarget()
    {
        Vector3 direction = (target.transform.position+Vector3.up - transform.position + transform.up).normalized;
        rb.AddForce(direction*force, ForceMode.Impulse);
    }
    

    private void OnCollisionEnter(Collision other)
    {
        //完成造成伤害的逻辑 + 击退目标
        Vector3 direction = (target.transform.position + Vector3.up - transform.position).normalized;
        switch (CurrentRockState)
        {
            
            case RockState.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<PlayerCharacterStats>().TakeDamage(attackDefinition);
                }
                break;
            case RockState.HitEnemy:
                if (other.gameObject.CompareTag("Enemy"))
                {
                    Instantiate(rockBreakEffect, transform.position, Quaternion.identity);
                    other.gameObject.GetComponent<EnemyCharacterStats>().TakeDamage(attackDefinition);
                    Destroy(gameObject);   
                }
                break;
            case RockState.HitNothing:
                if (other.gameObject.CompareTag("Player Weapon"))
                {
                    rb.velocity = Vector3.one;
                    CurrentRockState = RockState.HitEnemy;
                }
                break;
        }
    }
}
