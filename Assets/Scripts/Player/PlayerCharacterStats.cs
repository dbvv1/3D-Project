using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterStats : CharacterStats
{

    public override bool IsInvincible { get => InvincibleAfterHit || InvincibleWhenRoll||InvincibleWhenExecution; } //当前是否处于无敌状态

    private bool invincibleWhenRoll;     //翻滚时的无敌效果
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
        //通过玩家的动画判断是否处于完美格挡
        IsPerfectGuard = PlayerAnimationInfo.IsPerfectParry();
        base.TakeDamage(attacker);
        //执行 掉血 + OnTakeDamage事件
        float costHealth = attacker.DamageAmount - attacker.damageType switch
        {
            DamageType.Physical => CurPhysicalDefensive,
            DamageType.Magical => CurMagicalDefensive,
            DamageType.True => 0,
            _ => 0
        };
        costHealth = Mathf.Clamp(costHealth, 0, MaxHealth);
        //如果当前处于格挡状态
        if (IsGuard)
        {
            //如果是完美格挡的话：不掉血 + 不掉体力
            if (IsPerfectGuard) costHealth = 0;
            //普通格挡：只掉原先1/5的血，但会消耗体力 
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
        //如果角色还没有死亡，且体力值为0，则进入为期1秒的虚弱状态，敌人处于虚弱状态时可以被处决
        if (CurEnergy == 0 && !IsWeakState)
        {
            IsWeakState = true;
            GOPoolManager.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(2f, () =>
            {
                IsWeakState = false;
            });

        }
        Debug.Log("主角血量:" + CurHealth + "主角能量:" + CurEnergy);
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
    
    #region 状态的修改全局事件

    //使用回复物品
    private void OnUseRecoveryItem(float health, float energy, float magic)
    {
        CurHealth = Mathf.Clamp(CurHealth + health, 0, MaxHealth);
        CurEnergy = Mathf.Clamp(CurEnergy + energy, 0, MaxEnergy);
        CurMagic = Mathf.Clamp(CurMagic +magic, 0, MaxMagic);
        PlayerStatsUIManager.Instance.UpdateHealthSlider();
        PlayerStatsUIManager.Instance.UpdateEnergySlider();
        PlayerStatsUIManager.Instance.UpdateMagicSlider();
    }
    
    //使用增加最大状态的物品
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
    
    //使用增加攻击力的物品
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
