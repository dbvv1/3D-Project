using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//存储角色的状态信息
[RequireComponent(typeof(DataDefination))]
public class CharacterStats : MonoBehaviour,ISavable
{
    //角色的模板属性值
    public CharacterData_SO  originalCharacterData;

    private CharacterData_SO characterData;
        
    private AudioSource characterAudioSource;

    private Animator anim;

    #region 人物目前的属性
    private float curHealth;                 //当前血量

    private float curHealthRecover;          //当前生命回复

    private float curEnergy;                 //当前能量

    private float curEnergyRecover;          //当前能量回复

    private float curMagic;                  //当前魔法

    private float curMagicRecover;           //当前魔法回复

    private float curPhysicalDamage;         //当前物理伤害

    private float curSkillDamage;            //当前技能伤害

    private float curPhysicalDefensive;      //当前物理防御

    private float curMagicalDefensive;       //当前魔法防御

    private int curPowerPoint;               //当前力量值

    private int curAgilityPoint;             //当前敏捷值

    private int curIntelligencePoint;        //当前智力值

    private int curLevel;                    //当前等级

    private float curNeedExp;                //当前升级所需的经验值

    private float curExp;                    //当前经验值

    #endregion
    
    #region 获取角色的基础相关数据
    public float MaxHealth
    {
        get => originalCharacterData.maxHealth; set => originalCharacterData.maxHealth = value;
    }

    public float MaxEnergy
    {
        get => originalCharacterData.maxEnergy; set => originalCharacterData.maxEnergy = value;
    }

    public float MaxMagic
    {
        get => originalCharacterData.maxMagic; set => originalCharacterData.maxMagic = value;
    }

    public float BasePhysicalDamage
    {
        get => originalCharacterData.basePhysicalDamage; set => originalCharacterData.basePhysicalDamage = value;
    }

    public float BaseSkillDamage
    {
        get => originalCharacterData.baseSkillDamage; set => originalCharacterData.baseSkillDamage = value;
    }

    public float BasePhysicalDefensive
    {
        get => originalCharacterData.basePhysicalDefensive; set => originalCharacterData.basePhysicalDefensive = value;
    }

    public float BaseMagicalDefensive
    {
        get => originalCharacterData.baseMagicalDefensive; set => originalCharacterData.baseMagicalDefensive = value;
    }

    public int BasePowerPoint
    {
        get => originalCharacterData.basePowerPoint; set => originalCharacterData.basePowerPoint = value;
    }

    public int BaseAgilityPoint
    {
        get => originalCharacterData.baseAgilityPoint; set => originalCharacterData.baseAgilityPoint = value;
    }

    public int BaseIntelligencePoint
    {
        get => originalCharacterData.baseIntelligencePoint; set => originalCharacterData.baseIntelligencePoint = value;
    }

    public float BaseHealthRecover
    {
        get => originalCharacterData.baseHealthRecover; set => originalCharacterData.baseHealthRecover = value;
    }

    public float BaseEnergyRecover
    {
        get => originalCharacterData.baseEnergyRecover; set => originalCharacterData.baseEnergyRecover = value;
    }

    public float BaseMagicRecover
    {
        get => originalCharacterData.baseMagicRecover; set => originalCharacterData.baseMagicRecover = value;
    }

    public float BaseExp
    {
        get => originalCharacterData.baseExp; set => originalCharacterData.baseExp = value;
    }

    public float LevelBuf
    {
        get => originalCharacterData.levelBuf; set => originalCharacterData.levelBuf = value;
    }

    public float InvincibleTimeAfterHit
    {
        get => originalCharacterData.invincibleTimeAfterHit; set => originalCharacterData.invincibleTimeAfterHit = value;
    }

    #endregion

    #region 获取角色当前的真正数据值
    public float CurHealth
    {
        get => curHealth; set => curHealth = value;
    }

    public float CurEnergy
    {
        get => curEnergy; set => curEnergy = value;
    }

    public float CurMagic
    {
        get => curMagic; set => curMagic = value;
    }

    public float CurPhysicalDamage
    {
        get => curPhysicalDamage; set => curPhysicalDamage = value;
    }

    public float CurMagicDamage
    {
        get => curSkillDamage; set => curSkillDamage = value;
    }

    public float CurPhysicalDefenisve
    {
        get => curPhysicalDefensive; set => curPhysicalDefensive = value;
    }

    public float CurMagicalDefensive
    {
        get => curMagicalDefensive; set => curMagicalDefensive = value;
    }

    public int CurPowerPoint
    {
        get => curPowerPoint; set => curPowerPoint = value;
    }

    public int CurAgilityPoint
    {
        get => curAgilityPoint; set => curAgilityPoint = value;
    }

    public int CurIntelligencePoint
    {
        get => curIntelligencePoint; set => curIntelligencePoint = value;
    }

    public int CurLevel
    {
        get => curLevel; set => curLevel = value;
    }

    public float CurExp
    {
        get => curExp; set => curExp = value;
    }

    public float CurNeedExp
    {
        get => curNeedExp; set => curNeedExp = value;
    }

    public float CurHealthRecover
    {
        get => curHealthRecover; set => curHealthRecover = value;
    }

    public float CurEnergyRecover
    {
        get => curEnergyRecover; set => curEnergyRecover = value;
    }

    public float CurMagicRecover
    {
        get => curMagicRecover; set => curMagicRecover = value;
    }

    #endregion

    public virtual bool IsInvincible { get => InvincibleAfterHit || InvincibleWhenExecution; }

    protected bool invincibleAfterHit;             //受到伤害之后的短暂无敌
    public bool InvincibleAfterHit { get => invincibleAfterHit; set => invincibleAfterHit = value; }

    private bool invincibleWhenExecution;        //当执行处决的时候角色无敌
    protected bool InvincibleWhenExecution { get => invincibleWhenExecution; set => invincibleWhenExecution = value; }

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
        get=>isWeakState; 
        set
        {
            isWeakState = value;
            if (anim != null) anim.SetBool("IsWeak", value);
        } 
    }        

    protected bool IsExecuted;                   //是否正在被处决



    protected virtual void Awake()
    {
        characterAudioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        if (characterData == null)
            characterData = Instantiate(originalCharacterData);
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

    private void OnEnable()
    {
        ((ISavable)this).RegisterSaveData();
    }

    private void OnDisable()
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
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(attackerPos.x, transform.position.y, attackerPos.z) - transform.position);
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

   //TODO:状态的修改

   
    public string GetDataID()
    {
        return GetComponent<DataDefination>().id;
    }

    public void SaveData(Data data)
    {
        if (!data.characterStatsData.ContainsKey(GetDataID()))
            data.characterStatsData.Add(GetDataID(), characterData);
    }

    public void LoadData(Data data)
    {
        if (data.characterStatsData.ContainsKey(GetDataID()))
            data.characterStatsData.Remove(GetDataID());
    }
}
