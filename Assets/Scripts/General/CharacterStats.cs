using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//存储角色的状态信息
[RequireComponent(typeof(DataDefination))]
public class CharacterStats : MonoBehaviour, ISavable
{
    //角色的模板属性值
    public CharacterData_SO originalCharacterData;

    private CharacterData_SO characterData;

    private AudioSource characterAudioSource;

    private Animator anim;

    #region 获取角色的基础相关数据

    public float MaxHealth
    {
        get => characterData.maxHealth;
        set => characterData.maxHealth = value;
    }

    public float MaxEnergy
    {
        get => characterData.maxEnergy;
        set => characterData.maxEnergy = value;
    }

    public float MaxMagic
    {
        get => characterData.maxMagic;
        set => characterData.maxMagic = value;
    }

    public float BasePhysicalDamage
    {
        get => characterData.basePhysicalDamage;
        set => characterData.basePhysicalDamage = value;
    }

    public float BaseSkillDamage
    {
        get => characterData.baseSkillDamage;
        set => characterData.baseSkillDamage = value;
    }

    public float BasePhysicalDefensive
    {
        get => characterData.basePhysicalDefensive;
        set => characterData.basePhysicalDefensive = value;
    }

    public float BaseMagicalDefensive
    {
        get => characterData.baseMagicalDefensive;
        set => characterData.baseMagicalDefensive = value;
    }

    public int BasePowerPoint
    {
        get => characterData.basePowerPoint;
        set => characterData.basePowerPoint = value;
    }

    public int BaseAgilityPoint
    {
        get => characterData.baseAgilityPoint;
        set => characterData.baseAgilityPoint = value;
    }

    public int BaseIntelligencePoint
    {
        get => characterData.baseIntelligencePoint;
        set => characterData.baseIntelligencePoint = value;
    }

    public float BaseHealthRecover
    {
        get => characterData.baseHealthRecover;
        set => characterData.baseHealthRecover = value;
    }

    public float BaseEnergyRecover
    {
        get => characterData.baseEnergyRecover;
        set => characterData.baseEnergyRecover = value;
    }

    public float BaseMagicRecover
    {
        get => characterData.baseMagicRecover;
        set => characterData.baseMagicRecover = value;
    }

    public float BaseExp
    {
        get => characterData.baseExp;
        set => characterData.baseExp = value;
    }

    public float LevelBuf
    {
        get => characterData.levelBuf;
        set => characterData.levelBuf = value;
    }

    public float InvincibleTimeAfterHit
    {
        get => characterData.invincibleTimeAfterHit;
        set => characterData.invincibleTimeAfterHit = value;
    }

    #endregion

    #region 获取角色当前的真正数据值

    public float CurHealth
    {
        get => characterData.curHealth;
        set => characterData.curHealth = value;
    }

    public float CurEnergy
    {
        get => characterData.curEnergy;
        set => characterData.curEnergy = value;
    }

    public float CurMagic
    {
        get => characterData.curMagic;
        set => characterData.curMagic = value;
    }

    public float CurPhysicalDamage
    {
        get => characterData.curPhysicalDamage;
        set => characterData.curPhysicalDamage = value;
    }

    public float CurMagicDamage
    {
        get => characterData.curSkillDamage;
        set => characterData.curSkillDamage = value;
    }

    public float CurPhysicalDefensive
    {
        get => characterData.curPhysicalDefensive;
        set => characterData.curPhysicalDefensive = value;
    }

    public float CurMagicalDefensive
    {
        get => characterData.curMagicalDefensive;
        set => characterData.curMagicalDefensive = value;
    }

    public int CurPowerPoint
    {
        get => characterData.curPowerPoint;
        set => characterData.curPowerPoint = value;
    }

    public int CurAgilityPoint
    {
        get => characterData.curAgilityPoint;
        set => characterData.curAgilityPoint = value;
    }

    public int CurIntelligencePoint
    {
        get => characterData.curIntelligencePoint;
        set => characterData.curIntelligencePoint = value;
    }

    public int CurLevel
    {
        get => characterData.curLevel;
        set => characterData.curLevel = value;
    }

    public float CurExp
    {
        get => characterData.curExp;
        set => characterData.curExp = value;
    }

    public float CurNeedExp
    {
        get => characterData.curNeedExp;
        set => characterData.curNeedExp = value;
    }

    public float CurHealthRecover
    {
        get => characterData.curHealthRecover;
        set => characterData.curHealthRecover = value;
    }

    public float CurEnergyRecover
    {
        get => characterData.curEnergyRecover;
        set => characterData.curEnergyRecover = value;
    }

    public float CurMagicRecover
    {
        get => characterData.curMagicRecover;
        set => characterData.curMagicRecover = value;
    }

    #endregion

    public virtual bool IsInvincible
    {
        get => InvincibleAfterHit || InvincibleWhenExecution;
    }

    protected bool invincibleAfterHit; //受到伤害之后的短暂无敌

    public bool InvincibleAfterHit
    {
        get => invincibleAfterHit;
        set => invincibleAfterHit = value;
    }

    protected bool InvincibleWhenExecution { get; set; }

    protected float invincibleAfterHitTimeCounter;

    public UnityEvent OnTakeDamage;

    public UnityEvent OnDie;

    protected bool isDead;

    public bool IsGuard { get; set; }
    protected bool IsPerfectGuard { get; set; }


    //能否处于虚弱状态
    protected bool isWeakState;

    public bool IsWeakState
    {
        get => isWeakState;
        set
        {
            isWeakState = value;
            if (anim != null) anim.SetBool(IsWeak, value);
        }
    }

    protected bool IsExecuted; //是否正在被处决
    private static readonly int IsWeak = Animator.StringToHash("IsWeak");


    protected virtual void Awake()
    {
        characterAudioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        if (characterData == null)
        {
            characterData = ScriptableObject.CreateInstance<CharacterData_SO>();
            characterData.InitCharacterData(originalCharacterData);
        }
    }

    private void Start()
    {
        CurHealth = MaxHealth;
        CurEnergy = MaxEnergy;
        CurMagic = MaxMagic;
        CurHealthRecover = BaseHealthRecover;
        CurEnergyRecover = BaseEnergyRecover;
        CurMagicRecover = BaseMagicRecover;
        InvincibleAfterHit = false;
        IsWeakState = false;
    }

    protected virtual void OnEnable()
    {
        ((ISavable)this).RegisterSaveData();
    }

    protected virtual void OnDisable()
    {
        ((ISavable)this).UnRegisterSaveData();
    }

    protected virtual void Update()
    {
        RecoverStats();
        UpdateInvincibleAfterHurt();
    }


    //角色受到伤害 传进来的是攻击者的信息 (基类中处理通用的内容，还会在在主角和敌人的子类中分别重写)
    public virtual void TakeDamage(AttackDefinition attacker)
    {
        if (isDead) return;
        if (IsInvincible) return;
        //设置无敌
        InvincibleAfterHit = true;
        invincibleAfterHitTimeCounter = InvincibleTimeAfterHit;
        //播放音效：受击音效 或者 格挡音效
        PlayHitAudio();
        //人物转向
        Vector3 attackerPos = attacker.attacker.transform.position;
        Quaternion toRotation =
            Quaternion.LookRotation(
                new Vector3(attackerPos.x, transform.position.y, attackerPos.z) - transform.position);
        StartCoroutine(RotateToAttacker(toRotation));

        //后续内容：实际受到伤害，格挡的影响，是否进入虚弱状态
    }

    public void PlayHitAudio()
    {
        if (!IsGuard)
            AudioManager.Instance.PlayCharacterHit(characterAudioSource);
        else if (!IsPerfectGuard)
            AudioManager.Instance.PlayNormalParry(characterAudioSource);
        else
            AudioManager.Instance.PlayPerfectParry(characterAudioSource);
    }

    //消耗体力
    public void ExpendEnergy(float costAmount)
    {
        CurEnergy = Mathf.Clamp(CurEnergy - costAmount, 0, MaxEnergy);
    }

    private void UpdateInvincibleAfterHurt()
    {
        if (invincibleAfterHitTimeCounter > 0)
        {
            invincibleAfterHitTimeCounter -= Time.deltaTime;
        }

        if (invincibleAfterHitTimeCounter <= 0) InvincibleAfterHit = false;
    }

    //恢复状态
    protected virtual void RecoverStats()
    {
        CurHealth = Mathf.Clamp(CurHealth + Time.deltaTime * CurHealthRecover, 0, MaxHealth);
        CurEnergy = Mathf.Clamp(CurEnergy + Time.deltaTime * CurEnergyRecover, 0, MaxEnergy);
        CurMagic = Mathf.Clamp(CurMagic + Time.deltaTime * CurHealthRecover, 0, MaxMagic);
    }

    protected IEnumerator RotateToAttacker(Quaternion toRotation)
    {
        while (Quaternion.Angle(transform.rotation, toRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 20);
            yield return null;
        }
    }


    #region Isavable 接口

    public string GetDataID()
    {
        return GetComponent<DataDefination>().id;
    }

    public void SaveData(Data data)
    {
        var saveCharacterData = ScriptableObject.CreateInstance<CharacterData_SO>();
        saveCharacterData.InitCharacterData(characterData);
        if (data.characterStatsData.ContainsKey(GetDataID()))
            data.characterStatsData[GetDataID()] = saveCharacterData;
        else
            data.characterStatsData.Add(GetDataID(), saveCharacterData);
    }

    public void LoadData(Data data)
    {
        if (data.characterStatsData.ContainsKey(GetDataID()))
        {
            CharacterData_SO loadCharacterData = data.characterStatsData[GetDataID()];
            float maxHealthChange = loadCharacterData.maxHealth - characterData.maxHealth;
            float maxEnergyChange = loadCharacterData.maxEnergy - characterData.maxEnergy;
            float maxMagicChange = loadCharacterData.maxMagic - characterData.maxMagic;
            characterData.InitCharacterData(loadCharacterData);
            UpdateUIInfo(maxHealthChange, maxEnergyChange, maxMagicChange);
        }
    }

    #endregion


    protected virtual void UpdateUIInfo(float maxHealthChange, float maxEnergyChange, float maxMagicChange)
    {
        //通知UI进行更新操作 在子类中进行重载实现
    }
}