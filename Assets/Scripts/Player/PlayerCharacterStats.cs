using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterStats : CharacterStats
{

    public override bool IsInvincible { get => InvincibleAfterHit || InvincibleWhenRoll||InvincibleWhenExecution; } //当前是否处于无敌状态

    private bool invincibleWhenRoll;     //翻滚时的无敌效果
    public bool InvincibleWhenRoll { get => invincibleWhenRoll; set => invincibleWhenRoll = value; }

    private PlayerAnimationController playerAniminf;

    protected override void Awake()
    {
        base.Awake();
        playerAniminf = GetComponent<PlayerAnimationController>();
    }


    public override void TakeDamage(AttackDefinition attacker)
    {
        //通过玩家的动画判断是否处于完美格挡
        IsPerfectGuard = playerAniminf.IsPerfectParry();
        base.TakeDamage(attacker);
        //执行 掉血 + OnTakeDamage事件
        float costHealth = attacker.DamageAmount - attacker.damageType switch
        {
            DamageType.Physical => CurPhysicalDefenisve,
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

}
