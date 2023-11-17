using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterStats : CharacterStats
{

    public override bool IsInvincible { get => InvincibleAfterHit || InvincibleWhenRoll||InvincibleWhenExecution; } //��ǰ�Ƿ����޵�״̬

    private bool invincibleWhenRoll;     //����ʱ���޵�Ч��
    public bool InvincibleWhenRoll { get => invincibleWhenRoll; set => invincibleWhenRoll = value; }

    private PlayerAnimationController PlayerAnimationInfo;

    protected override void Awake()
    {
        base.Awake();
        PlayerAnimationInfo = GetComponent<PlayerAnimationController>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GlobalEvent.UseRecoveryItemEvent += OnUseRecoveryItem;
        GlobalEvent.UseStatsIncreaseItemEvent += OnUseStatsIncreaseItem;
        GlobalEvent.UseAttackIncreaseItemEvent += OnUseAttackIncreaseItem;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GlobalEvent.UseRecoveryItemEvent -= OnUseRecoveryItem;
        GlobalEvent.UseStatsIncreaseItemEvent -= OnUseStatsIncreaseItem;
        GlobalEvent.UseAttackIncreaseItemEvent -= OnUseAttackIncreaseItem;
    }


    public override void TakeDamage(AttackDefinition attacker)
    {
        //ͨ����ҵĶ����ж��Ƿ���������
        IsPerfectGuard = PlayerAnimationInfo.IsPerfectParry();
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
    
    #region ״̬���޸�ȫ���¼�

    //ʹ�ûظ���Ʒ
    private void OnUseRecoveryItem(float health, float energy, float magic)
    {
        CurHealth = Mathf.Clamp(CurHealth + health, 0, MaxHealth);
        CurEnergy = Mathf.Clamp(CurEnergy + energy, 0, MaxEnergy);
        CurMagic = Mathf.Clamp(CurMagic +magic, 0, MaxMagic);
        PlayerStatsUIManager.Instance.UpdateHealthSlider();
        PlayerStatsUIManager.Instance.UpdateEnergySlider();
        PlayerStatsUIManager.Instance.UpdateMagicSlider();
    }
    
    //ʹ���������״̬����Ʒ
    private void OnUseStatsIncreaseItem(float health, float energy, float magic)
    {
        MaxHealth += health;
        MaxEnergy += energy;
        MaxMagic += magic;
        PlayerStatsUIManager.Instance.UpdateHealthSlider();
        PlayerStatsUIManager.Instance.UpdateEnergySlider();
        PlayerStatsUIManager.Instance.UpdateMagicSlider();
        PlayerStatsUIManager.Instance.UpdateSliderWidth(health,energy,magic);
        PlayerStatsUIManager.Instance.RefreshMaxInformation();
    }
    
    //ʹ�����ӹ���������Ʒ
    private void OnUseAttackIncreaseItem(float attack)
    {
        BasePhysicalDamage += attack;
        CurPhysicalDamage += attack;
    }
    
    
    #endregion

    protected override void UpdateUIInfo(float maxHealthChange, float maxEnergyChange, float maxMagicChange)
    {
        PlayerStatsUIManager.Instance.UpdateHealthSlider();
        PlayerStatsUIManager.Instance.UpdateEnergySlider();
        PlayerStatsUIManager.Instance.UpdateMagicSlider();
        PlayerStatsUIManager.Instance.UpdateSliderWidth(maxHealthChange, maxEnergyChange, maxMagicChange);
    }
}
