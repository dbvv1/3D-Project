using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//�洢��ɫ��״̬��Ϣ
[RequireComponent(typeof(DataDefination))]
public class CharacterStats : MonoBehaviour,ISavable
{
    //��ɫ��ģ������ֵ
    public CharacterData_SO  originalCharacterData;

    private CharacterData_SO characterData;
        
    private AudioSource characterAudioSource;

    private Animator anim;

    #region ����Ŀǰ������
    private float curHealth;                 //��ǰѪ��

    private float curHealthRecover;          //��ǰ�����ظ�

    private float curEnergy;                 //��ǰ����

    private float curEnergyRecover;          //��ǰ�����ظ�

    private float curMagic;                  //��ǰħ��

    private float curMagicRecover;           //��ǰħ���ظ�

    private float curPhysicalDamage;         //��ǰ�����˺�

    private float curSkillDamage;            //��ǰ�����˺�

    private float curPhysicalDefensive;      //��ǰ�������

    private float curMagicalDefensive;       //��ǰħ������

    private int curPowerPoint;               //��ǰ����ֵ

    private int curAgilityPoint;             //��ǰ����ֵ

    private int curIntelligencePoint;        //��ǰ����ֵ

    private int curLevel;                    //��ǰ�ȼ�

    private float curNeedExp;                //��ǰ��������ľ���ֵ

    private float curExp;                    //��ǰ����ֵ

    #endregion
    
    #region ��ȡ��ɫ�Ļ����������
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

    #region ��ȡ��ɫ��ǰ����������ֵ
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

    protected bool invincibleAfterHit;             //�ܵ��˺�֮��Ķ����޵�
    public bool InvincibleAfterHit { get => invincibleAfterHit; set => invincibleAfterHit = value; }

    private bool invincibleWhenExecution;        //��ִ�д�����ʱ���ɫ�޵�
    protected bool InvincibleWhenExecution { get => invincibleWhenExecution; set => invincibleWhenExecution = value; }

    protected float invincibleAfterHitTimeCounter;

    public UnityEvent OnTakeDamage;

    public UnityEvent OnDie;

    protected bool isDead;

    public bool IsGuard { get; set; }
    protected bool IsPerfectGuard { get; set; }


    //�ܷ�������״̬
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

    protected bool IsExecuted;                   //�Ƿ����ڱ�����



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


    //��ɫ�ܵ��˺� ���������ǹ����ߵ���Ϣ (�����д���ͨ�õ����ݣ������������Ǻ͵��˵������зֱ���д)
    public virtual void TakeDamage(AttackDefinition attacker)
    {
        if (isDead) return;
        if (IsInvincible) return;
        //�����޵�
        InvincibleAfterHit = true;
        invincibleAfterHitTimeCounter = InvincibleTimeAfterHit;
        //������Ч���ܻ���Ч ���� ����Ч
        PlayHitAudio();
        //����ת��
        Vector3 attackerPos = attacker.attacker.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(attackerPos.x, transform.position.y, attackerPos.z) - transform.position);
        StartCoroutine(RotateToAttacker(toRotation));

        //�������ݣ�ʵ���ܵ��˺����񵲵�Ӱ�죬�Ƿ��������״̬
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

    //��������
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

    //�ָ�״̬
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

   //TODO:״̬���޸�

   
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
