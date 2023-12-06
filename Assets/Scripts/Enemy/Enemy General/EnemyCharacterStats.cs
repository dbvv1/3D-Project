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
        //ִ�� ��Ѫ + OnTakeDamage�¼�
        float costHealth = attacker.DamageAmount - attacker.damageType switch
        {
            DamageType.Physical => CurPhysicalDefensive,
            DamageType.Magical => CurMagicalDefensive,
            DamageType.True => 0,
            _ => 0
        };
        costHealth = Mathf.Clamp(costHealth, 0, MaxHealth);
        //�����ǰ���ڸ�״̬(Ĭ��Ϊ��ͨ��)
        if (IsGuard) costHealth /= 3;
        //��������ڱ�������״̬ (����Ĭ��ֱ�ӿ۳�����)
        if (!IsExecuted)
            ExpendEnergy(attacker.energyAmount);
        if (costHealth >= CurHealth)
        {
            CurHealth = 0;
            isDead = true;
            //GlobalEvent.CallOnEnemyDeath(enemy); �Ѿ��ٵ��˶������������
            OnDie?.Invoke();
        }
        else
        {
            CurHealth -= costHealth;
            OnTakeDamage?.Invoke();
        }
        enemyStatsBarUI.UpdateStats(CurHealth, MaxHealth,CurEnergy,MaxEnergy);
        //�����ɫ��û��������������ֵΪ0�������Ϊ��1�������״̬�����˴�������״̬ʱ���Ա�����
        if (!isDead && CurEnergy == 0 && !IsWeakState) 
        {
            IsWeakState = true;
            //���õ��˼��뵽�������˱���
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
