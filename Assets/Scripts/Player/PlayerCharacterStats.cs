using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterStats : CharacterStats
{

    public override bool IsInvincible => InvincibleAfterHit || InvincibleWhenRoll||InvincibleWhenExecution; //当前是否处于无敌状态

    public bool InvincibleWhenRoll { get; set; }

    public int Money
    {
        get => playerData.money;
        set => playerData.money = value;
    }

    public int SkillPoint
    {
        get => playerData.skillPoint;
        set => playerData.skillPoint = value;
    }

    public int AttributePoint
    {
        get=>playerData.attributePoint;
        set => playerData.attributePoint = value;
    }

    private PlayerData_SO playerData;
    private PlayerAnimationController playerAnimationInfo;
    private PlayerController playerController;

    protected override void Awake()
    {
        base.Awake();
        playerAnimationInfo = GetComponent<PlayerAnimationController>();
        playerController = GetComponent<PlayerController>();
    }

    protected override void Start()
    {
        base.Start();
        playerData=characterData as PlayerData_SO;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GlobalEvent.useRecoveryItemEvent += OnUseRecoveryItem;
        GlobalEvent.useStatsIncreaseItemEvent += OnUseStatsIncreaseItem;
        GlobalEvent.useAttackIncreaseItemEvent += OnUseAttackIncreaseItem;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GlobalEvent.useRecoveryItemEvent -= OnUseRecoveryItem;
        GlobalEvent.useStatsIncreaseItemEvent -= OnUseStatsIncreaseItem;
        GlobalEvent.useAttackIncreaseItemEvent -= OnUseAttackIncreaseItem;
    }

    protected override void LevelUp()
    {
        base.LevelUp();
        SkillPoint++;
        AttributePoint++;
        PlayerStatsUIManager.Instance.UpdateSliderWidth(playerData.maxHealth * (1 - 1 / (1 + playerData.levelBuf)),
            playerData.maxEnergy * (1 - 1 / (1 + playerData.levelBuf)),
            playerData.maxMagic * (1 - 1 / (1 + playerData.levelBuf)));
        PlayerStatsUIManager.Instance.UpdateSliderValue();
        PlayerStatsUIManager.Instance.RefreshMaxInformation();
    }

    public void ChangeExp(int expChangeAmount)
    {
        //当前升级所需的经验值小于所给的就经验，需要升级
        if (CurNeedExp - CurExp <= expChangeAmount)
        {
            CurExp = expChangeAmount - (CurNeedExp - CurExp);
            LevelUp();
        }
        else
        {
            CurExp+=expChangeAmount;
        }

        Debug.Log("角色经验值：" + CurExp);
        PlayerStatsUIManager.Instance.UpdateLevelInfo();
    }

    public void ChangeMoney(int moneyChangeAmount)
    {
        Money += moneyChangeAmount;
        PlayerStatsUIManager.Instance.UpdateMoneyAmountInfo();
    }


    public override void TakeDamage(AttackDefinition attacker)
    {
        //如果当前人物不是处于锁定状态 则转向
        if (playerController.GetCurrentLockEnemy == null)
        {
            Vector3 attackerPos = attacker.attacker.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(
                    new Vector3(attackerPos.x, transform.position.y, attackerPos.z) - transform.position);
            StartCoroutine(RotateToAttacker(toRotation));
        }
        
        //通过玩家的动画判断是否处于完美格挡
        IsPerfectGuard = playerAnimationInfo.IsPerfectParry();
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
            PoolManager.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(2f, () =>
            {
                IsWeakState = false;
            });

        }
    }

    protected override void RecoverStats()
    {
        base.RecoverStats();
        PlayerStatsUIManager.Instance.UpdateSliderValue();
    }
    
    
    
    #region 状态的修改全局事件

    //使用回复物品
    private void OnUseRecoveryItem(float health, float energy, float magic)
    {
        CurHealth = Mathf.Clamp(CurHealth + health, 0, MaxHealth);
        CurEnergy = Mathf.Clamp(CurEnergy + energy, 0, MaxEnergy);
        CurMagic = Mathf.Clamp(CurMagic +magic, 0, MaxMagic);
        PlayerStatsUIManager.Instance.UpdateSliderValue();
    }
    
    //使用增加最大状态的物品
    private void OnUseStatsIncreaseItem(float health, float energy, float magic)
    {
        MaxHealth += health;
        MaxEnergy += energy;
        MaxMagic += magic;
        PlayerStatsUIManager.Instance.UpdateSliderValue();
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
        PlayerStatsUIManager.Instance.UpdateUserUIInfo();
        PlayerStatsUIManager.Instance.UpdateSliderValue();
        PlayerStatsUIManager.Instance.UpdateSliderWidth(maxHealthChange, maxEnergyChange, maxMagicChange);
    }
}
