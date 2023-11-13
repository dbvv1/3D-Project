using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterStats : CharacterStats
{
    private EnemyController enemy;

    private HealthBarUI healthBarUI;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<EnemyController>();
        healthBarUI = GetComponent<HealthBarUI>();
    }

    public override void TakeDamage(AttackDefinition attacker)
    {
        if (IsInvincible) return;
        base.TakeDamage(attacker);
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
            GlobalEvent.CallOnEnemyDeath(enemy);
            OnDie?.Invoke();
        }
        else
        {
            CurHealth -= costHealth;
            OnTakeDamage?.Invoke();
        }
        healthBarUI.UpdateHealthBar(CurHealth, MaxHealth);
        //�����ɫ��û��������������ֵΪ0�������Ϊ��1�������״̬�����˴�������״̬ʱ���Ա�����
        if (!isDead && CurEnergy == 0 && !IsWeakState) 
        {
            IsWeakState = true;
            //���õ��˼��뵽�������˱���
            GameManager.Instance.AddWeakEnemy(enemy);
            GlobalEvent.CallEnemyEnterWeakState(enemy);
            GOPoolManager.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(2f, () => 
            {
                if(enemy!=null)
                {
                    if (!enemy.IsExecuted)
                    {
                        IsWeakState = false;
                        GlobalEvent.CallEnemyExitWeakState(enemy);
                    }
                    CurEnergy = MaxEnergy; GameManager.Instance.RemoveWeakEnemy(enemy);
                } 
            });

        }
        Debug.Log("����Ѫ��:" + CurHealth);
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.Instance.RemoveWeakEnemy(enemy);
    }

    protected override void UpdateUIInfo()
    {
        healthBarUI.UpdateHealthBar(CurHealth,MaxHealth);
    }
}
