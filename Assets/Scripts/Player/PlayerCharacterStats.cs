using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterStats : CharacterStats
{

    public override bool IsInvincible { get => InvincibleAfterHit || InvincibleWhenRoll||InvincibleWhenExecution; } //��ǰ�Ƿ����޵�״̬

    private bool invincibleWhenRoll;     //����ʱ���޵�Ч��
    public bool InvincibleWhenRoll { get => invincibleWhenRoll; set => invincibleWhenRoll = value; }

    private PlayerAnimationController playerAniminf;

    protected override void Awake()
    {
        base.Awake();
        playerAniminf = GetComponent<PlayerAnimationController>();
    }


    public override void TakeDamage(AttackDefinition attacker)
    {
        //ͨ����ҵĶ����ж��Ƿ���������
        IsPerfectGuard = playerAniminf.IsPerfectParry();
        base.TakeDamage(attacker);
        //ִ�� ��Ѫ + OnTakeDamage�¼�
        float costHealth = attacker.DamageAmount - attacker.damageType switch
        {
            DamageType.Physical => CurPhysicalDefenisve,
            DamageType.Magical => CurMagicalDefensive,
            DamageType.True => 0,
            _ => 0
        };
        costHealth = Mathf.Clamp(costHealth, 0, MaxHealth);
        //�����ǰ���ڸ�״̬
        if (IsGuard)
        {
            //����������񵲵Ļ�������Ѫ + ��������
            if (IsPerfectGuard) costHealth = 0;
            //��ͨ�񵲣�ֻ��ԭ��1/5��Ѫ�������������� 
            else
            {
                costHealth /= 3;
                if (!IsExecuted)
                    ExpendEnergy(attacker.energyAmount);
            }
        }
        if (costHealth >= CurHealth)
        {
            CurHealth = 0;
            isDead = true;
            OnDie?.Invoke();
            return;
        }
        else
        {
            CurHealth -= costHealth;
            OnTakeDamage?.Invoke();
        }
        //�����ɫ��û��������������ֵΪ0�������Ϊ��1�������״̬�����˴�������״̬ʱ���Ա�����
        if (CurEnergy == 0 && !IsWeakState)
        {
            IsWeakState = true;
            GOPoolManager.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(2f, () =>
            {
                IsWeakState = false;
            });

        }
        Debug.Log("����Ѫ��:" + CurHealth + "��������:" + CurEnergy);
    }

    protected override void RecoverStats()
    {
        if (CurHealth < MaxHealth)
        {
            CurHealth = Mathf.Clamp(CurHealth + Time.deltaTime * CurHealthRecover, 0, MaxHealth);
            PlayerStatsUIManager.Instance.UpdateHealthSlider();
        }
        if (CurEnergy < MaxEnergy)
        {
            CurEnergy = Mathf.Clamp(CurEnergy + Time.deltaTime * CurEnergyRecover, 0, MaxEnergy);
            PlayerStatsUIManager.Instance.UpdateEnergySlider();
        }
        if (CurMagic < MaxMagic)
        {
            CurMagic = Mathf.Clamp(CurMagic + Time.deltaTime * CurHealthRecover, 0, MaxMagic);
            PlayerStatsUIManager.Instance.UpdateMagicSlider();
        }
    }

}
