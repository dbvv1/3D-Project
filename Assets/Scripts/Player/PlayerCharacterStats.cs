using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterStats : CharacterStats
{

    public override bool IsInvincible => InvincibleAfterHit || InvincibleWhenRoll||InvincibleWhenExecution; //��ǰ�Ƿ����޵�״̬

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
        //��ǰ��������ľ���ֵС�������ľ;��飬��Ҫ����
        if (CurNeedExp - CurExp <= expChangeAmount)
        {
            CurExp = expChangeAmount - (CurNeedExp - CurExp);
            LevelUp();
        }
        else
        {
            CurExp+=expChangeAmount;
        }

        Debug.Log("��ɫ����ֵ��" + CurExp);
        PlayerStatsUIManager.Instance.UpdateLevelInfo();
    }

    public void ChangeMoney(int moneyChangeAmount)
    {
        Money += moneyChangeAmount;
        PlayerStatsUIManager.Instance.UpdateMoneyAmountInfo();
    }


    public override void TakeDamage(AttackDefinition attacker)
    {
        //�����ǰ���ﲻ�Ǵ�������״̬ ��ת��
        if (playerController.GetCurrentLockEnemy == null)
        {
            Vector3 attackerPos = attacker.attacker.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(
                    new Vector3(attackerPos.x, transform.position.y, attackerPos.z) - transform.position);
            StartCoroutine(RotateToAttacker(toRotation));
        }
        
        //ͨ����ҵĶ����ж��Ƿ���������
        IsPerfectGuard = playerAnimationInfo.IsPerfectParry();
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
    
    
    
    #region ״̬���޸�ȫ���¼�

    //ʹ�ûظ���Ʒ
    private void OnUseRecoveryItem(float health, float energy, float magic)
    {
        CurHealth = Mathf.Clamp(CurHealth + health, 0, MaxHealth);
        CurEnergy = Mathf.Clamp(CurEnergy + energy, 0, MaxEnergy);
        CurMagic = Mathf.Clamp(CurMagic +magic, 0, MaxMagic);
        PlayerStatsUIManager.Instance.UpdateSliderValue();
    }
    
    //ʹ���������״̬����Ʒ
    private void OnUseStatsIncreaseItem(float health, float energy, float magic)
    {
        MaxHealth += health;
        MaxEnergy += energy;
        MaxMagic += magic;
        PlayerStatsUIManager.Instance.UpdateSliderValue();
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
        PlayerStatsUIManager.Instance.UpdateUserUIInfo();
        PlayerStatsUIManager.Instance.UpdateSliderValue();
        PlayerStatsUIManager.Instance.UpdateSliderWidth(maxHealthChange, maxEnergyChange, maxMagicChange);
    }
}
