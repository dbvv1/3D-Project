using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//玩家的控制脚本
public class PlayerController : MonoBehaviour
{
    [Header("引用")]
    [SerializeField] private Transform faceDirection;
    [SerializeField]private ThirdLockEnemyCamera thirdLockEnemyCamera;

    private PlayerInputController inputActions;

    [HideInInspector]public CharacterController characterController;
    [HideInInspector]public PlayerCharacterStats playerCurrentStats;

    private Animator anim;

    private PlayerAnimationController playerAnimationInf;

    private int animatorCombatLayer;

    //public FirstPersonCamera firstPersonCamera;

    [Header("基本属性")]
    [SerializeField]private float curSpeed;

    [SerializeField, Header("行走速度")]              private float walkSpeed = 3;          

    [SerializeField, Header("奔跑速度")]              private float runSpeed = 5;   
    

    [SerializeField, Header("奔跑消耗精力速度速度")]    private float runCostEnergy = 1;     

    [SerializeField, Header("翻滚速度")]              private float rollSpeed = 20;     

    [SerializeField, Header("翻滚消耗的精力值")]        private float rollCost = 10;          

    [SerializeField, Header("人物旋转角度速度")]        private float rotationSpeed = 5;      

    [SerializeField, Header("重力加速度")]             private float gravity = -9.8f;        

    [SerializeField, Header("跳跃高度")]               private float jumpHeight = 10f;

    public Transform focusPoint;

    private Vector3 velocityByGravity;            //重力所产生的向下的速度

    private Vector2 inputDirection;               //WASD按键的输入信息

    private Vector3 moveDirection;                //人物移动的方向信息

    private PhysicalCheck physicalCheck;

    private bool isLoading;


    private bool isLock;

    private bool jumpButton;
    
        
    private static readonly int IsLightSword = Animator.StringToHash("IsLightSword");
    private static readonly int IsGreatSword = Animator.StringToHash("IsGreatSword");
    private static readonly int RAttackHold = Animator.StringToHash("RAttack Hold");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Execution = Animator.StringToHash("Execution");
    private static readonly int LAttack = Animator.StringToHash("LAttack");
    private static readonly int ChangeWeapon = Animator.StringToHash("Change Weapon");

    private EnemyController curLockedEnemy;
    public bool IsLock
    {
        get;set;
    }

    private PlayerWeaponType currentWeaponType = PlayerWeaponType.LightSword;
    private PlayerWeaponType CurrentWeaponType
    {
        get => currentWeaponType;
        set
        {
            switch (currentWeaponType)
            {
                case PlayerWeaponType.LightSword:
                    anim.SetBool(IsLightSword, false);
                    break;
                case PlayerWeaponType.GreatSword:
                    anim.SetBool(IsGreatSword, false);
                    break;
            }

            currentWeaponType = value;
            switch (currentWeaponType)
            {
                case PlayerWeaponType.LightSword:
                    anim.SetBool(IsLightSword, true);
                    break;
                case PlayerWeaponType.GreatSword:
                    anim.SetBool(IsGreatSword, true);
                    break;
            }
        }
    }
    
    
    //是否允许攻击的输入
    private bool applyAttackInput;
    public bool ApplyAttackInput { get => applyAttackInput; set => applyAttackInput = value; }

    //是否允许格挡
    public bool CanParry { get => !playerAnimationInf.IsAttack; }
    

    private void Awake()
    {
        inputActions = new PlayerInputController();
        characterController = GetComponent<CharacterController>();
        physicalCheck = GetComponent<PhysicalCheck>();
        playerCurrentStats = GetComponent<PlayerCharacterStats>();
        playerAnimationInf = GetComponent<PlayerAnimationController>();
        anim = GetComponent<Animator>();
       
    }

    private void Start()
    {
        velocityByGravity = Vector2.zero;
        currentWeaponType = PlayerWeaponType.LightSword;
        applyAttackInput = true;
        animatorCombatLayer = anim.GetLayerIndex("Combat Layer");
        
        
        //人物翻滚 Release Only
        inputActions.GamePlay.Roll.performed += StartRoll;

        //人物行走 Hold
        inputActions.GamePlay.Move.performed += StartWalk;
        inputActions.GamePlay.Move.canceled += CancelWalk;

        //人物奔跑 Hold
        inputActions.GamePlay.Run.performed += StartRun;
        inputActions.GamePlay.Run.canceled += CancelRun;

        //人物跳跃 Press
        inputActions.GamePlay.Jump.started += StartJump;
        inputActions.GamePlay.Jump.canceled += CancelJump;

        //人物左键攻击 Press
        inputActions.GamePlay.LAttack.started += StartLAttack;

        inputActions.GamePlay.RAttack.started += StartFocusAttack;
        inputActions.GamePlay.RAttack.canceled += StopFocusAttack;

        //人物格挡
        inputActions.GamePlay.Parry.started += StartParry;
        inputActions.GamePlay.Parry.canceled += StopParry;
        //锁定敌人 Press
        inputActions.GamePlay.LockEnemy.started += SwitchLockState;

        //切换武器
        inputActions.GamePlay.ChangeWeapon.started += SwitchWeapon;
        
    }

    private void OnEnable()
    {
        inputActions.Enable();
        GlobalEvent.switchToFirstPersonEvent += OnSwitchToFirstPerson;
        GlobalEvent.enemyDeathEvent += OnEnemyDeath;
        GlobalEvent.afterSceneLoadEvent += OnAfterSceneLoad;
        GlobalEvent.beforeSceneLoadEvent += OnBeforeSceneLoad;
        GlobalEvent.stopTheWorldEvent += StopTheGame;
        GlobalEvent.continueTheWorldEvent += ContinueTheGame;
        GlobalEvent.onEnterDialogue += DisablePlayerGamePlayInput;
        GlobalEvent.onExitDialogue += EnablePlayerGamePlayInput;
        GlobalEvent.enterMenuSceneEvent += DisablePlayerGamePlayInput;
        GlobalEvent.exitMenuSceneEvent += EnablePlayerGamePlayInput;

    }
    
    private void OnDisable()
    {
        inputActions.Disable();
        GlobalEvent.switchToFirstPersonEvent -= OnSwitchToFirstPerson;
        GlobalEvent.enemyDeathEvent -= OnEnemyDeath;
        GlobalEvent.afterSceneLoadEvent -= OnAfterSceneLoad;
        GlobalEvent.beforeSceneLoadEvent -= OnBeforeSceneLoad;
        GlobalEvent.stopTheWorldEvent -= StopTheGame;
        GlobalEvent.continueTheWorldEvent -= ContinueTheGame;
        GlobalEvent.onEnterDialogue -= DisablePlayerGamePlayInput;
        GlobalEvent.onExitDialogue -= EnablePlayerGamePlayInput;
        GlobalEvent.enterMenuSceneEvent -= DisablePlayerGamePlayInput;
        GlobalEvent.exitMenuSceneEvent -= EnablePlayerGamePlayInput;
    }

    #region  全局事件函数
    //切换到第一视角：调整faceDirection为人物的朝向
    private void OnSwitchToFirstPerson()
    {
        IsLock = false;
        faceDirection.forward = transform.forward;
    }
    
    private void OnAfterSceneLoad(Vector3 pos)
    {
        inputActions.Enable();
        transform.position = pos;
        Invoke(nameof(SetIsLoadingFalse), 0.02f);
    }

    private void SetIsLoadingFalse() => isLoading = false;

    private void OnBeforeSceneLoad()
    {
        InitPlayer();
        isLoading = true;
        inputActions.Disable();
        
    }

    private void StopTheGame()
    {
        inputActions.GamePlay.Jump.Disable();
        inputActions.GamePlay.LAttack.Disable();
        inputActions.GamePlay.RAttack.Disable();
        inputActions.GamePlay.Roll.Disable();
        inputActions.GamePlay.Parry.Disable();
    }

    private void ContinueTheGame()
    {
        inputActions.GamePlay.Jump.Enable();
        inputActions.GamePlay.LAttack.Enable();
        inputActions.GamePlay.RAttack.Enable();
        inputActions.GamePlay.Roll.Enable();
        inputActions.GamePlay.Parry.Enable();
    }

    private void DisablePlayerGamePlayInput()
    {
        InitPlayer();
        inputActions.GamePlay.Disable();
    }
    
    private void EnablePlayerGamePlayInput()
    {
        playerAnimationInf.IsGuard = false;
        inputActions.GamePlay.Enable();
    }

    //重置Player的所有状态
    private void InitPlayer()
    {
        playerAnimationInf.IsWalk = false;
        playerAnimationInf.IsRun = false;
        anim.SetBool(RAttackHold, false);
        inputActions.GamePlay.Move.Dispose();
        inputActions.GamePlay.Run.Dispose();
        inputActions.GamePlay.RAttack.Dispose();
    }
    

    #endregion



    private void Update()
    {
        if (SceneLoader.Instance.GetCurrentSceneType == SceneType.Menu) return;
        
        inputDirection = inputActions.GamePlay.Move.ReadValue<Vector2>();
        if(!isLoading)MovePlayer();
        if(!isLoading)Gravitation();
        if(CanJump()) StartJump(); 
        StatsCheckAndCost();
    }


    private bool CanJump()
    {
        return jumpButton && physicalCheck.IsOnGround && !playerAnimationInf.IsRoll && !playerAnimationInf.IsAttack &&
               !playerAnimationInf.IsGuard && playerAnimationInf.landAnimationOver;
    }

    private void MovePlayer()
    {
        //如果正在翻滚或者正在攻击 则返回
        if (!playerAnimationInf.rollAnimationOver ||playerAnimationInf.IsAttack) return;
        //如果正在处于受伤状态 也返回
        if (playerAnimationInf.IsHurt) return;
        if (CameraManager.Instance.currentCameraStyle == CameraStyle.FirstPerson)
        {
            RotatePlayerInFirstPerson();
            MovePlayerInFirstPerson();
        }
        else
        {
            RotatePlayerInThirdPerson();
            MovePlayerInThirdPerson();
        }
    }

    private void StatsCheckAndCost()
    {
        //人物正在奔跑：消耗体力
        if (playerAnimationInf.IsWalk && playerAnimationInf.IsRun) playerCurrentStats.ExpendEnergy(Time.deltaTime * runCostEnergy);
        //体力太少：停止奔跑状态 
        if (playerAnimationInf.IsRun && playerCurrentStats.CurEnergy < 3f) playerAnimationInf.IsRun = false;
    }

    #region 人物的旋转和移动（第三人称和第一人称）

    //第三人称方式移动人物
    private void MovePlayerInThirdPerson()
    {
        if (IsLock) MovePlayerInThirdPersonLock();
        else MovePlayerInThirdPersonFree();
        
    }

    //第三人称 自由视角 移动人物
    private void MovePlayerInThirdPersonFree()
    {
        if (playerAnimationInf.IsWalk == false) return;
        //人物的实际移动:始终朝着面朝的方向进行移动
        if (physicalCheck.HaveBarrierInMoveDirection(transform.position + transform.up * 0.5f, transform.forward, moveDirection.normalized.magnitude * curSpeed * Time.deltaTime * 2)) return;
        characterController.Move(transform.forward.normalized * curSpeed * Time.deltaTime);
    }

    //第三人称 锁定视角 移动人物
    private void MovePlayerInThirdPersonLock()
    {
        if (playerAnimationInf.IsWalk == false) return;
        //人物的实际移动：同时根据人物的朝向 和 人物的WASD输入
        moveDirection = transform.forward * inputDirection.y + transform.right * inputDirection.x;
        moveDirection.y = 0;
        if (physicalCheck.HaveBarrierInMoveDirection(transform.position + transform.up * 0.5f, moveDirection, moveDirection.normalized.magnitude * curSpeed * Time.deltaTime * 2)) return;
        characterController.Move(moveDirection.normalized * curSpeed * Time.deltaTime);
    }

    //第三人称方式旋转人物
    private void RotatePlayerInThirdPerson()
    {
        if (IsLock) RotatePlayerInThirdPersonLockEnemy();
        else RotatePlayerInThirdPersonFree();
    }

    //第三人称 自由视角方式的旋转
    private void RotatePlayerInThirdPersonFree()
    {
        moveDirection = faceDirection.forward * inputDirection.y + faceDirection.right * inputDirection.x;
        if(moveDirection!=Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);
        }
    }

    //第三人称 锁定视角方式的旋转
    private void RotatePlayerInThirdPersonLockEnemy()
    {
        //人物的forward永远是敌人，人物的目标面向：敌人坐标 - 自己坐标
        moveDirection = new Vector3(curLockedEnemy.transform.position.x - transform.position.x,transform.forward.y, curLockedEnemy.transform.position.z - transform.position.z);
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);
    }

    //第一人称方式移动人物
    private void MovePlayerInFirstPerson()
    {
        //如果移动方向有阻碍物：障碍，敌人等 返回
        if (playerAnimationInf.IsWalk == false) return;
        moveDirection = faceDirection.forward * inputDirection.y + faceDirection.right * inputDirection.x;
        if (physicalCheck.HaveBarrierInMoveDirection(transform.position + transform.up * 0.5f, moveDirection, moveDirection.normalized.magnitude * curSpeed * Time.deltaTime * 2)) return;
        characterController.Move(moveDirection.normalized * (curSpeed * Time.deltaTime));
    }

    //第一人称方式旋转人物
    private void RotatePlayerInFirstPerson()
    {
        transform.forward = faceDirection.forward;
    }

    private void Gravitation()
    {
        //考虑重力
        if (physicalCheck.IsOnGround && velocityByGravity.y < 0) velocityByGravity.y = 0;
        else velocityByGravity.y += gravity * Time.deltaTime;
        characterController.Move(velocityByGravity * Time.deltaTime);
    }

    private void StartJump()
    {
        anim.SetTrigger(Jump);
        playerAnimationInf.landAnimationOver = false;
        velocityByGravity.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
    }

    #endregion

    #region 人物按键控制
    //开始行走
    private void StartWalk(InputAction.CallbackContext obj)
    {
        playerAnimationInf.IsWalk = true;
        if(!playerAnimationInf.IsRoll &&!playerAnimationInf.IsAttack)StopAllCoroutines();
        float targetSpeed = playerAnimationInf.IsRun ? runSpeed : walkSpeed;
        StartCoroutine(ChangeToTargetSpeed(targetSpeed, 0.3f));
    }
    //结束行走
    private void CancelWalk(InputAction.CallbackContext obj)
    {
        playerAnimationInf.IsWalk = false;
        if (!playerAnimationInf.IsRoll && !playerAnimationInf.IsAttack) StopAllCoroutines();
        StartCoroutine(ChangeToTargetSpeed(0, 0.1f));
    }
    //开始跑步
    private void StartRun(InputAction.CallbackContext obj)
    {
        playerAnimationInf.IsRun = true;
        if (!playerAnimationInf.IsWalk || playerAnimationInf.IsGuard) return;
        if (!playerAnimationInf.IsRoll && !playerAnimationInf.IsAttack) StopAllCoroutines();
        StartCoroutine(ChangeToTargetSpeed(runSpeed, 0.3f));
    }
    //结束跑步
    private void CancelRun(InputAction.CallbackContext obj)
    {
        //延迟调用？ （由于Roll和Run是同一个按键的不同交互导致的，确保松开Shift后不会翻滚）
        Invoke(nameof(CancelRun), 0f);
    }
    private void CancelRun()
    {
        playerAnimationInf.IsRun = false;
        if (!playerAnimationInf.IsRoll) StopAllCoroutines();
        float targetSpeed = playerAnimationInf.IsWalk ? walkSpeed : 0;
        StartCoroutine(ChangeToTargetSpeed(targetSpeed, 0.2f));
    }
    //开始跳跃
    private void StartJump(InputAction.CallbackContext obj)
    {
        jumpButton = true;
    }
    //结束跳跃
    private void CancelJump(InputAction.CallbackContext obj)
    {
        jumpButton = false;
    }
    //开始翻滚
    private bool CanRoll()
    {
        return physicalCheck.IsOnGround && playerCurrentStats.CurEnergy >= rollCost && !playerAnimationInf.IsRoll && !playerAnimationInf.IsRun && playerAnimationInf.rollAnimationOver && !playerAnimationInf.IsFocusAttack && !playerAnimationInf.IsChangeWeapon;
    }
    private void StartRoll(InputAction.CallbackContext obj)
    {
        //如果人物在地面上并且人物的能量足够 才进行翻滚
        if (CanRoll())     
        {
            //消耗能量 变更为无敌状态
            StopAllCoroutines();playerAnimationInf.StopAllCoroutines();
            playerCurrentStats.ExpendEnergy(rollCost);
            playerCurrentStats.InvincibleWhenRoll = true;
            playerAnimationInf.IsRoll = true;
            playerAnimationInf.rollAnimationOver = false;
            //StartCoroutine(RollForward());  //转变为由动画控制移动
        }
    }
    //开始普通攻击
    private void StartLAttack(InputAction.CallbackContext obj)
    {
        if (!playerAnimationInf.IsAttack)
            ApplyAttackInput = true;
        if (!ApplyAttackInput || playerAnimationInf.IsRoll||playerAnimationInf.IsGuard) return;
        ApplyAttackInput = false;
        //判定是否能够对于处于虚弱状态的敌人进行处决
        EnemyController weakEnemy = SelectWeakEnemy();
        if (weakEnemy!= null) 
        {
            //执行实际的处决
            Vector3 toDirection = new Vector3(weakEnemy.transform.position.x - transform.position.x, transform.forward.y, weakEnemy.transform.position.z - transform.position.z);
            StartCoroutine(RotateForwardWithSpeed(toDirection, 5));
            weakEnemy.IsExecuted = true;
            GlobalEvent.CallEnemyExitWeakState(weakEnemy);
            anim.SetTrigger(Execution);
        }
        else
        {
            //正常进行普通攻击
            if (curLockedEnemy != null)
            {
                Vector3 toDirection = new Vector3(curLockedEnemy.transform.position.x - transform.position.x, transform.forward.y, curLockedEnemy.transform.position.z - transform.position.z);
                StartCoroutine(RotateForwardWithSpeed(toDirection, 5));
            }
            anim.SetTrigger(LAttack);
        }
    }

    //开始蓄力攻击
    private void StartFocusAttack(InputAction.CallbackContext obj)
    {
        if (playerAnimationInf.IsAttack) return;
        if (playerAnimationInf.IsRoll||playerAnimationInf.IsGuard) return;
        ApplyAttackInput = false;
        if (curLockedEnemy != null)
        {
            Vector3 toDirection = new Vector3(curLockedEnemy.transform.position.x - transform.position.x, transform.forward.y, curLockedEnemy.transform.position.z - transform.position.z);
            StartCoroutine(RotateForwardWithSpeed(toDirection, 5));
        }
        anim.SetBool(RAttackHold, true);
    }

    private void StopFocusAttack(InputAction.CallbackContext obj)
    {
        anim.SetBool(RAttackHold, false);
    }

    //第三人称切换锁定状态
    private void SwitchLockState(InputAction.CallbackContext obj)
    {
        if (CameraManager.Instance.currentCameraStyle == CameraStyle.FirstPerson) return;
        IsLock = !IsLock;
        if (IsLock)
        {
            //选择一个敌人进行锁定
            //curLockedEnemy = SelectLockEnemyByMousePos();
            curLockedEnemy = SelectLockEnemy();
            if (curLockedEnemy != null)
            {
                CameraManager.Instance.SwitchToThirdPersonLockEnemy(curLockedEnemy);
                GlobalEvent.CallEnterFocusOnEnemy(curLockedEnemy);
            }
            else IsLock = false;
        }
        else
        {
            curLockedEnemy = null;
            //取消聚焦点  相机转换
            GlobalEvent.CallExitFocusOnEnemy();
            CameraManager.Instance.SwitchCameraStyle(CameraStyle.ThirdPersonNormal);
        }
    }

    private void SwitchWeapon(InputAction.CallbackContext obj)
    {
        if (playerAnimationInf.IsRoll || playerAnimationInf.IsAttack) return;
        //curSpeed = 0;
        switch (currentWeaponType)
        {
            case PlayerWeaponType.LightSword:
                CurrentWeaponType = PlayerWeaponType.GreatSword;
                break;
            case PlayerWeaponType.GreatSword:
                CurrentWeaponType = PlayerWeaponType.LightSword;
                break;
        }
        anim.SetTrigger(ChangeWeapon);
    }

    //进行格挡
    private void StartParry(InputAction.CallbackContext obj)
    {
        if (CanParry) 
        {
            playerAnimationInf.IsGuard = true;
            CancelRun();
        }
    }

    //取消格挡
    private void StopParry(InputAction.CallbackContext obj)
    {
        playerAnimationInf.IsGuard = false;
    }


    #endregion

    //选择锁定的敌人(根据鼠标锁定)
    private EnemyController SelectLockEnemy()
    {
       //return SelectEnemyCloseToPlayer(GameManager.Instance.enemies, float.MaxValue, Mathf.Acos(0f));;
       return SelectEnemyCloseToMouse(GameManager.Instance.enemies, float.MaxValue, Mathf.Acos(0f));
    }
    
    //返回锁定的敌人
    public EnemyController GetCurrentLockEnemy => curLockedEnemy;

    
    //综合判断是否有敌人可以被处决 (根据人物当前方向处决)
    private EnemyController SelectWeakEnemy()
    {
        return SelectEnemyCloseToPlayer(GameManager.Instance.weakEnemies, 4f, Mathf.Acos(0.35f));
    }

    //在指定的集合中根据当前人物的朝向选择一个敌人
    private EnemyController SelectEnemyCloseToPlayer(HashSet<EnemyController> enemies, float minDistance,float facing)
    {
        //具体要求：距离小于某个值 + 角度为人物的前方   (根据角度选择最佳的目标) 
        EnemyController currentEnemy = null;
        float minAngle = facing / Mathf.PI * 180;
        foreach (var enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < minDistance)
            {
                //enemy满足要求 比较得到更好的敌人
                Vector3 targetPos = enemy.transform.position - transform.position;
                float curAngle = Vector3.Angle(transform.forward, targetPos);
                if (curAngle < minAngle)
                {
                    minAngle = curAngle;
                    currentEnemy = enemy;
                }
            }
        }
        return currentEnemy;
    }

    private EnemyController SelectEnemyCloseToMouse(HashSet<EnemyController> enemies, float minDistance, float facing)
    {
        //具体要求：距离小于某个值 + 角度为人物的前方   (根据角度选择最佳的目标) 
        EnemyController currentEnemy = null;
        //Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Vector3.Distance(Camera.main.transform.position, transform.position));
        //Vector3 startPos = Camera.main.ScreenToWorldPoint(screenPoint);
        Transform startTransform = Camera.main.transform;
        float minAngle = facing / Mathf.PI * 180;
        foreach (var enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < minDistance)
            {
                //enemy满足要求 比较得到更好的敌人
                Vector3 targetPos = enemy.transform.position - startTransform.position;
                float curAngle = Vector3.Angle(startTransform.forward, targetPos);
                if (curAngle < minAngle)
                {
                    minAngle = curAngle;
                    currentEnemy = enemy;
                }
            }
        }
        return currentEnemy;
    }


    #region 协程
    //使用Lerp实现加速和减速协程
    IEnumerator ChangeToTargetSpeed(float targetSpeed,float needTime)
    {
        float time = 0;
        while(time<needTime)
        {
            curSpeed = Mathf.Lerp(curSpeed, targetSpeed, time / needTime);
            time += Time.deltaTime;
            yield return null;
        }
        curSpeed = targetSpeed;
    }
    
    //向前翻滚的协程
    IEnumerator RollForward()
    {
        while(playerAnimationInf.IsRoll)
        {
            characterController.Move(transform.forward.normalized * rollSpeed * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// 旋转人物面朝的方向
    /// </summary>
    /// <param name="toRotation"></param>
    /// <param name="multi"></param>
    /// <returns></returns>
    private IEnumerator RotateForwardWithSpeed(Vector3 toRotation,float multi)
    {
        while (Vector3.Angle(transform.forward, toRotation) > 3f)
        {
            transform.forward = Vector3.Slerp(transform.forward, toRotation, rotationSpeed * multi * Time.deltaTime);
            yield return null;
        }
    }

    #endregion

    #region UnityEvent事件
    public void OnEnemyDeath(EnemyController enemy)
    {
        //如果当前锁定的敌人死亡，则解除死亡状态
        if(curLockedEnemy!=null&&curLockedEnemy==enemy)
        {
            IsLock = false;
            curLockedEnemy = null;
            GlobalEvent.CallExitFocusOnEnemy();
            CameraManager.Instance.SwitchCameraStyle(CameraStyle.ThirdPersonNormal);
            
        }
    }

    #endregion
}
