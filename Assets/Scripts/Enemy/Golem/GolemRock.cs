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
    private GolemController golemController;

    [Header("石块属性")] [SerializeField] private float force;

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
        this.golemController = golemController;
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
        rb.velocity = Vector3.one;
        FlyToTarget();
        CurrentRockState = RockState.HitPlayer;
    }


    private void FlyToTarget()
    {
        Vector3 direction = (target.transform.position + Vector3.up - transform.position).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
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
                    //如果玩家此时是处于完美格挡的状态 则直接弹反岩石
                    //若处于普通格挡的状态 移动的距离需要减少
                    CurrentRockState = RockState.HitNothing;
                    var playerInfo = other.gameObject.GetComponent<PlayerAnimationController>();
                    if (playerInfo.IsGuard) direction /= 2;
                    other.gameObject.GetComponent<CharacterController>().Move(direction);
                    other.gameObject.GetComponent<PlayerCharacterStats>().TakeDamage(attackDefinition);
                    if (playerInfo.IsPerfectParry())
                    {
                        Vector3 toEnemy = (golemController.FocusTransform.position - transform.position).normalized;
                        rb.velocity = Vector3.one;
                        CurrentRockState = RockState.HitEnemy;
                        attackDefinition.attacker = playerInfo.GetComponent<PlayerCharacterStats>();
                        rb.AddForce(toEnemy * force, ForceMode.Impulse);
                    }
                }

                break;
            case RockState.HitEnemy:
                if (other.gameObject.CompareTag("Enemy"))
                {
                    CurrentRockState = RockState.HitNothing;
                    Instantiate(rockBreakEffect, transform.position, Quaternion.identity);
                    other.gameObject.GetComponent<CharacterController>().Move(direction);
                    other.gameObject.GetComponent<EnemyCharacterStats>().TakeDamage(attackDefinition);
                    Destroy(gameObject);
                }

                break;
            case RockState.HitNothing:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CurrentRockState == RockState.HitNothing)
        {
            if (other.gameObject.CompareTag("Player Weapon"))
            {
                rb.velocity = Vector3.one;
                Vector3 direction = (transform.position - target.transform.position).normalized;
                //如果玩家目前是锁定状态，则直接将岩石飞向锁定的敌人
                var playerController = other.GetComponentInParent<PlayerController>();
                if (playerController.GetCurrentLockEnemy != null)
                    direction = (playerController.GetCurrentLockEnemy.FocusTransform.position - transform.position).normalized;
                rb.AddForce(direction * force, ForceMode.Impulse);
                attackDefinition.attacker = playerController.playerCurrentStats;
                CurrentRockState = RockState.HitEnemy;
            }
        }
    }
}