using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterStats : CharacterStats
{
    private EnemyController enemy;
    private EnemyStatsBarUI enemyStatsBarUI;
    private EnemyData_SO enemyData;
    
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<EnemyController>();
        enemyStatsBarUI = GetComponent<EnemyStatsBarUI>();
    }

    protected override void Start()
    {
        base.Start();
        enemyData=characterData as EnemyData_SO;
        
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void RecoverStats()
    {
        base.RecoverStats();
        enemyStatsBarUI.UpdateStats(CurHealth, MaxHealth, CurEnergy, MaxEnergy);
    }

    public override void TakeDamage(AttackDefinition attacker)
    {
        base.TakeDamage(attacker);
        Vector3 attackerPos = attacker.attacker.transform.position;
        Quaternion toRotation =
            Quaternion.LookRotation(
                new Vector3(attackerPos.x, transform.position.y, attackerPos.z) - transform.position);
        StartCoroutine(RotateToAttacker(toRotation));
        //执行 掉血 + OnTakeDamage事件
        float costHealth = attacker.DamageAmount - attacker.damageType switch
        {
            DamageType.Physical => CurPhysicalDefensive,
            DamageType.Magical => CurMagicalDefensive,
            DamageType.True => 0,
            _ => 0
        };
        costHealth = Mathf.Clamp(costHealth, 0, MaxHealth);
        //如果当前处于格挡状态(默认为普通格挡)
        if (IsGuard) costHealth /= 3;
        //如果不处于被处决的状态 (敌人默认直接扣除体力)
        if (!IsExecuted)
            ExpendEnergy(attacker.energyAmount);
        if (costHealth >= CurHealth)
        {
            CurHealth = 0;
            isDead = true;
            //GlobalEvent.CallOnEnemyDeath(enemy); 已经再敌人动画结束后调用
            OnDie?.Invoke();
        }
        else
        {
            CurHealth -= costHealth;
            OnTakeDamage?.Invoke();
        }
        enemyStatsBarUI.UpdateStats(CurHealth, MaxHealth,CurEnergy,MaxEnergy);
        //如果角色还没有死亡，且体力值为0，则进入为期1秒的虚弱状态，敌人处于虚弱状态时可以被处决
        if (!isDead && CurEnergy == 0 && !IsWeakState) 
        {
            IsWeakState = true;
            //将该敌人加入到虚弱敌人表中
            GameManager.Instance.AddWeakEnemy(enemy);
            GlobalEvent.CallEnemyEnterWeakState(enemy);
            PoolManager.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(2f, () => 
            {
                if(enemy!=null)
                {
                    if (!enemy.IsExecuted)
                    {
                        IsWeakState = false;
                        GlobalEvent.CallEnemyExitWeakState(enemy);
                        CurEnergy = MaxEnergy; 
                    }
                    GameManager.Instance.RemoveWeakEnemy(enemy);
                } 
            });

        }
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.Instance.RemoveWeakEnemy(enemy);
    }

    protected override void UpdateUIInfo(float x,float y,float z)
    {
        enemyStatsBarUI.UpdateStats(CurHealth,MaxHealth,CurEnergy,MaxEnergy);
    }
}
