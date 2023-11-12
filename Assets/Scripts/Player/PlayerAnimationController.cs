using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("引用")]
    [SerializeField] private Transform greatSwordInHand;

    [SerializeField] private Transform lightSwordInHand;

    [SerializeField] private Transform greatSwordOnBack;

    [SerializeField] private Transform lightSwordOnBack;

    private Animator anim;

    private int animatorCombatLayer;

    private PlayerController playerController;

    private PhysicalCheck physicalCheck;

    private AudioSource playerAudioSource;

    [SerializeField]private MultiAudioDefination lightSwordAudio;

    [SerializeField]private MultiAudioDefination greatSwordAudio;

    #region 人物的状态信息
    private bool isWalk;
    public bool IsWalk
    {
        get => isWalk;
        set
        {
            isWalk = value;
            anim.SetBool("IsWalk", value);
        }
    }

    private bool isRun;
    public bool IsRun
    {
        get => isRun;
        set
        {
            isRun = value;
            anim.SetBool("IsRun", value);
        }
    }

    private bool isRoll;
    public bool IsRoll
    {
        get => isRoll;
        set
        {
            if (value == true) anim.SetTrigger("Roll");
            anim.SetBool("IsRoll", value);
            isRoll = value;
        }
    }

    [HideInInspector] public bool rollAnimationOver;

    public bool IsLeftAttack
    {
        get => anim.GetCurrentAnimatorStateInfo(animatorCombatLayer).IsTag("Normal Attack");
    }
    public bool IsFocusAttack
    {
        get => anim.GetCurrentAnimatorStateInfo(animatorCombatLayer).IsTag("Focus Attack");
    }
    public bool IsAttack
    {
        get => IsLeftAttack || IsFocusAttack;
    }

    private bool isHurt;
    public bool IsHurt
    {
        get; set;
    }

    private bool isChangeWeapon;
    public bool IsChangeWeapon { get; set; }

    private bool isGuard;
    public bool IsGuard
    {
        get => isGuard;
        set
        {
            isGuard = value;
            playerController.playerCurrentStats.IsGuard = value;
            if (value) anim.SetTrigger("Guard");
            anim.SetBool("IsGuard", value);
        }
    }

    [HideInInspector]public bool landAnimationOver;

    #endregion

    private void Awake()
    {
        anim = GetComponent<Animator>();
        animatorCombatLayer = anim.GetLayerIndex("Combat Layer");
        playerController = GetComponent<PlayerController>();
        playerAudioSource = GetComponent<AudioSource>();
        lightSwordAudio = GetComponent<MultiAudioDefination>();
        physicalCheck = GetComponent<PhysicalCheck>();
    }

    private void Start()
    {
        IsWalk = false;
        IsRun = false;
        rollAnimationOver = true;
        landAnimationOver = true;
        anim.SetBool("IsLightSword", true);
        anim.SetBool("IsGreatSword", false);
    }

    private void Update()
    {
    }

    private void OnEnable()
    {
        anim.SetBool("IsLightSword", true);
        GlobalEvent.RollAnimationOverEvent += OnAfterRollAnimationOverEvent;
        GlobalEvent.LandAnimationOverEvent += OnAfterLandAnimationOverEvent;
    }

    private void OnDisable()
    {
        GlobalEvent.RollAnimationOverEvent -= OnAfterRollAnimationOverEvent;
        GlobalEvent.LandAnimationOverEvent -= OnAfterLandAnimationOverEvent;
    }

    public bool IsPerfectParry()
    {
        return anim.GetCurrentAnimatorStateInfo(animatorCombatLayer).IsName("Enter Guard");
    }

    #region 动画事件
    public void AfterDeathAnimation()
    {
        gameObject.SetActive(false);
    }

    public void AfterHurtAnimation()
    {
        IsHurt = false;
    }

    public void AfterRoll()
    {
        IsRoll = false;
        playerController.playerCurrentStats.InvincibleWhenRoll = false;
    }

    private void OnAfterRollAnimationOverEvent()
    {
        rollAnimationOver = true;
    }

    private void OnAfterLandAnimationOverEvent()
    {
        landAnimationOver = true;
    }

    ////攻击动画进入时的事件
    //private void OnEnterNormalAttackEvent()
    //{
    //    IsLeftAttack = true;
    //}

    //private void OnEnterFocusAttackEvent()
    //{
    //    IsFocusAttack = true;
    //}

    ////攻击动画结束时的事件
    //private void OnAfterAttackFinishEvent()
    //{
    //    IsLeftAttack = false;
    //    IsFocusAttack = false;
    //}


    //攻击时的移动修正
    private Coroutine currentMoveAttackCoroutine;

    //翻滚时的移动修正
    private Coroutine currentMoveRollCoroutine;

    public void StartAttackMove(float speed)
    {
        if (currentMoveAttackCoroutine != null) StopCoroutine(currentMoveAttackCoroutine);
        if (IsRoll) return;
        currentMoveAttackCoroutine = StartCoroutine(MoveForward(speed));
    }

    public void StopAttackMove()
    {
        StopCoroutine(currentMoveAttackCoroutine);
    }

    public void StartRollMove(float speed)
    {
        if (currentMoveAttackCoroutine != null) StopCoroutine(currentMoveAttackCoroutine);
        if (currentMoveRollCoroutine != null) StopCoroutine(currentMoveRollCoroutine);
        currentMoveRollCoroutine = StartCoroutine(MoveForward(speed));
    }

    public void StopRollMove()
    {
        StopCoroutine(currentMoveRollCoroutine);
    }

    private IEnumerator MoveForward(float speed)
    {
        while (true)
        {
            if (!physicalCheck.haveBarrierInMoveDirectino(transform.position+transform.up*0.5f, transform.forward, transform.forward.normalized.magnitude * speed * 2.5f * Time.deltaTime))
            {
                playerController.characterController.Move(transform.forward.normalized * speed * Time.deltaTime);
            }
            yield return null;
        }
    }

    //攻击的音效事件
    public void PlayAttackClip()
    {
        if (lightSwordInHand.gameObject.activeSelf)
            AudioManager.Instance.PlayClipBySpecifiedSource(lightSwordAudio.PlayClipRandom(), playerAudioSource);
        else if(greatSwordInHand.gameObject.activeSelf)
            AudioManager.Instance.PlayClipBySpecifiedSource(greatSwordAudio.PlayClipRandom(), playerAudioSource);
    }

    //切换武器相关的事件
    public void SwitchLightSwordToGreatSword()
    {
        lightSwordOnBack.gameObject.SetActive(true);    
        lightSwordInHand.gameObject.SetActive(false);
        greatSwordInHand.gameObject.SetActive(true);
        greatSwordOnBack.gameObject.SetActive(false);
    }

    public void SwitchGreatSwordToLightSword()
    {
        lightSwordOnBack.gameObject.SetActive(false);
        lightSwordInHand.gameObject.SetActive(true);
        greatSwordInHand.gameObject.SetActive(false);
        greatSwordOnBack.gameObject.SetActive(true);
    }

    public void OnHurt()
    {
        if(!IsGuard)
        {
            IsHurt = true;
            anim.SetTrigger("Hurt");
        }
        else
        {
            anim.SetTrigger("Parry");
        }
    }

    public void OnDeath()
    {
        anim.SetTrigger("Death");
    }

    #endregion

}
