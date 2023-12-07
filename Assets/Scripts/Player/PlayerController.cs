using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//��ҵĿ��ƽű�
public class PlayerController : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private Transform faceDirection;
    [SerializeField]private ThirdLockEnemyCamera thirdLockEnemyCamera;

    private PlayerInputController inputActions;

    [HideInInspector]public CharacterController characterController;
    [HideInInspector]public PlayerCharacterStats playerCurrentStats;

    private Animator anim;

    private PlayerAnimationController playerAnimationInf;

    private int animatorCombatLayer;

    //public FirstPersonCamera firstPersonCamera;

    [Header("��������")]
    [SerializeField]private float curSpeed;

    [SerializeField, Header("�����ٶ�")]              private float walkSpeed = 3;          

    [SerializeField, Header("�����ٶ�")]              private float runSpeed = 5;   
    

    [SerializeField, Header("�������ľ����ٶ��ٶ�")]    private float runCostEnergy = 1;     

    [SerializeField, Header("�����ٶ�")]              private float rollSpeed = 20;     

    [SerializeField, Header("�������ĵľ���ֵ")]        private float rollCost = 10;          

    [SerializeField, Header("������ת�Ƕ��ٶ�")]        private float rotationSpeed = 5;      

    [SerializeField, Header("�������ٶ�")]             private float gravity = -9.8f;        

    [SerializeField, Header("��Ծ�߶�")]               private float jumpHeight = 10f;

    public Transform focusPoint;

    private Vector3 velocityByGravity;            //���������������µ��ٶ�

    private Vector2 inputDirection;               //WASD������������Ϣ

    private Vector3 moveDirection;                //�����ƶ��ķ�����Ϣ

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
    
    
    //�Ƿ�������������
    private bool applyAttackInput;
    public bool ApplyAttackInput { get => applyAttackInput; set => applyAttackInput = value; }

    //�Ƿ������
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
        
        
        //���﷭�� Release Only
        inputActions.GamePlay.Roll.performed += StartRoll;

        //�������� Hold
        inputActions.GamePlay.Move.performed += StartWalk;
        inputActions.GamePlay.Move.canceled += CancelWalk;

        //���ﱼ�� Hold
        inputActions.GamePlay.Run.performed += StartRun;
        inputActions.GamePlay.Run.canceled += CancelRun;

        //������Ծ Press
        inputActions.GamePlay.Jump.started += StartJump;
        inputActions.GamePlay.Jump.canceled += CancelJump;

        //����������� Press
        inputActions.GamePlay.LAttack.started += StartLAttack;

        inputActions.GamePlay.RAttack.started += StartFocusAttack;
        inputActions.GamePlay.RAttack.canceled += StopFocusAttack;

        //�����
        inputActions.GamePlay.Parry.started += StartParry;
        inputActions.GamePlay.Parry.canceled += StopParry;
        //�������� Press
        inputActions.GamePlay.LockEnemy.started += SwitchLockState;

        //�л�����
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

    #region  ȫ���¼�����
    //�л�����һ�ӽǣ�����faceDirectionΪ����ĳ���
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

    //����Player������״̬
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
        //������ڷ����������ڹ��� �򷵻�
        if (!playerAnimationInf.rollAnimationOver ||playerAnimationInf.IsAttack) return;
        //������ڴ�������״̬ Ҳ����
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
        //�������ڱ��ܣ���������
        if (playerAnimationInf.IsWalk && playerAnimationInf.IsRun) playerCurrentStats.ExpendEnergy(Time.deltaTime * runCostEnergy);
        //����̫�٣�ֹͣ����״̬ 
        if (playerAnimationInf.IsRun && playerCurrentStats.CurEnergy < 3f) playerAnimationInf.IsRun = false;
    }

    #region �������ת���ƶ��������˳ƺ͵�һ�˳ƣ�

    //�����˳Ʒ�ʽ�ƶ�����
    private void MovePlayerInThirdPerson()
    {
        if (IsLock) MovePlayerInThirdPersonLock();
        else MovePlayerInThirdPersonFree();
        
    }

    //�����˳� �����ӽ� �ƶ�����
    private void MovePlayerInThirdPersonFree()
    {
        if (playerAnimationInf.IsWalk == false) return;
        //�����ʵ���ƶ�:ʼ�ճ����泯�ķ�������ƶ�
        if (physicalCheck.HaveBarrierInMoveDirection(transform.position + transform.up * 0.5f, transform.forward, moveDirection.normalized.magnitude * curSpeed * Time.deltaTime * 2)) return;
        characterController.Move(transform.forward.normalized * curSpeed * Time.deltaTime);
    }

    //�����˳� �����ӽ� �ƶ�����
    private void MovePlayerInThirdPersonLock()
    {
        if (playerAnimationInf.IsWalk == false) return;
        //�����ʵ���ƶ���ͬʱ��������ĳ��� �� �����WASD����
        moveDirection = transform.forward * inputDirection.y + transform.right * inputDirection.x;
        moveDirection.y = 0;
        if (physicalCheck.HaveBarrierInMoveDirection(transform.position + transform.up * 0.5f, moveDirection, moveDirection.normalized.magnitude * curSpeed * Time.deltaTime * 2)) return;
        characterController.Move(moveDirection.normalized * curSpeed * Time.deltaTime);
    }

    //�����˳Ʒ�ʽ��ת����
    private void RotatePlayerInThirdPerson()
    {
        if (IsLock) RotatePlayerInThirdPersonLockEnemy();
        else RotatePlayerInThirdPersonFree();
    }

    //�����˳� �����ӽǷ�ʽ����ת
    private void RotatePlayerInThirdPersonFree()
    {
        moveDirection = faceDirection.forward * inputDirection.y + faceDirection.right * inputDirection.x;
        if(moveDirection!=Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);
        }
    }

    //�����˳� �����ӽǷ�ʽ����ת
    private void RotatePlayerInThirdPersonLockEnemy()
    {
        //�����forward��Զ�ǵ��ˣ������Ŀ�����򣺵������� - �Լ�����
        moveDirection = new Vector3(curLockedEnemy.transform.position.x - transform.position.x,transform.forward.y, curLockedEnemy.transform.position.z - transform.position.z);
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);
    }

    //��һ�˳Ʒ�ʽ�ƶ�����
    private void MovePlayerInFirstPerson()
    {
        //����ƶ��������谭��ϰ������˵� ����
        if (playerAnimationInf.IsWalk == false) return;
        moveDirection = faceDirection.forward * inputDirection.y + faceDirection.right * inputDirection.x;
        if (physicalCheck.HaveBarrierInMoveDirection(transform.position + transform.up * 0.5f, moveDirection, moveDirection.normalized.magnitude * curSpeed * Time.deltaTime * 2)) return;
        characterController.Move(moveDirection.normalized * (curSpeed * Time.deltaTime));
    }

    //��һ�˳Ʒ�ʽ��ת����
    private void RotatePlayerInFirstPerson()
    {
        transform.forward = faceDirection.forward;
    }

    private void Gravitation()
    {
        //��������
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

    #region ���ﰴ������
    //��ʼ����
    private void StartWalk(InputAction.CallbackContext obj)
    {
        playerAnimationInf.IsWalk = true;
        if(!playerAnimationInf.IsRoll &&!playerAnimationInf.IsAttack)StopAllCoroutines();
        float targetSpeed = playerAnimationInf.IsRun ? runSpeed : walkSpeed;
        StartCoroutine(ChangeToTargetSpeed(targetSpeed, 0.3f));
    }
    //��������
    private void CancelWalk(InputAction.CallbackContext obj)
    {
        playerAnimationInf.IsWalk = false;
        if (!playerAnimationInf.IsRoll && !playerAnimationInf.IsAttack) StopAllCoroutines();
        StartCoroutine(ChangeToTargetSpeed(0, 0.1f));
    }
    //��ʼ�ܲ�
    private void StartRun(InputAction.CallbackContext obj)
    {
        playerAnimationInf.IsRun = true;
        if (!playerAnimationInf.IsWalk || playerAnimationInf.IsGuard) return;
        if (!playerAnimationInf.IsRoll && !playerAnimationInf.IsAttack) StopAllCoroutines();
        StartCoroutine(ChangeToTargetSpeed(runSpeed, 0.3f));
    }
    //�����ܲ�
    private void CancelRun(InputAction.CallbackContext obj)
    {
        //�ӳٵ��ã� ������Roll��Run��ͬһ�������Ĳ�ͬ�������µģ�ȷ���ɿ�Shift�󲻻ᷭ����
        Invoke(nameof(CancelRun), 0f);
    }
    private void CancelRun()
    {
        playerAnimationInf.IsRun = false;
        if (!playerAnimationInf.IsRoll) StopAllCoroutines();
        float targetSpeed = playerAnimationInf.IsWalk ? walkSpeed : 0;
        StartCoroutine(ChangeToTargetSpeed(targetSpeed, 0.2f));
    }
    //��ʼ��Ծ
    private void StartJump(InputAction.CallbackContext obj)
    {
        jumpButton = true;
    }
    //������Ծ
    private void CancelJump(InputAction.CallbackContext obj)
    {
        jumpButton = false;
    }
    //��ʼ����
    private bool CanRoll()
    {
        return physicalCheck.IsOnGround && playerCurrentStats.CurEnergy >= rollCost && !playerAnimationInf.IsRoll && !playerAnimationInf.IsRun && playerAnimationInf.rollAnimationOver && !playerAnimationInf.IsFocusAttack && !playerAnimationInf.IsChangeWeapon;
    }
    private void StartRoll(InputAction.CallbackContext obj)
    {
        //��������ڵ����ϲ�������������㹻 �Ž��з���
        if (CanRoll())     
        {
            //�������� ���Ϊ�޵�״̬
            StopAllCoroutines();playerAnimationInf.StopAllCoroutines();
            playerCurrentStats.ExpendEnergy(rollCost);
            playerCurrentStats.InvincibleWhenRoll = true;
            playerAnimationInf.IsRoll = true;
            playerAnimationInf.rollAnimationOver = false;
            //StartCoroutine(RollForward());  //ת��Ϊ�ɶ��������ƶ�
        }
    }
    //��ʼ��ͨ����
    private void StartLAttack(InputAction.CallbackContext obj)
    {
        if (!playerAnimationInf.IsAttack)
            ApplyAttackInput = true;
        if (!ApplyAttackInput || playerAnimationInf.IsRoll||playerAnimationInf.IsGuard) return;
        ApplyAttackInput = false;
        //�ж��Ƿ��ܹ����ڴ�������״̬�ĵ��˽��д���
        EnemyController weakEnemy = SelectWeakEnemy();
        if (weakEnemy!= null) 
        {
            //ִ��ʵ�ʵĴ���
            Vector3 toDirection = new Vector3(weakEnemy.transform.position.x - transform.position.x, transform.forward.y, weakEnemy.transform.position.z - transform.position.z);
            StartCoroutine(RotateForwardWithSpeed(toDirection, 5));
            weakEnemy.IsExecuted = true;
            GlobalEvent.CallEnemyExitWeakState(weakEnemy);
            anim.SetTrigger(Execution);
        }
        else
        {
            //����������ͨ����
            if (curLockedEnemy != null)
            {
                Vector3 toDirection = new Vector3(curLockedEnemy.transform.position.x - transform.position.x, transform.forward.y, curLockedEnemy.transform.position.z - transform.position.z);
                StartCoroutine(RotateForwardWithSpeed(toDirection, 5));
            }
            anim.SetTrigger(LAttack);
        }
    }

    //��ʼ��������
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

    //�����˳��л�����״̬
    private void SwitchLockState(InputAction.CallbackContext obj)
    {
        if (CameraManager.Instance.currentCameraStyle == CameraStyle.FirstPerson) return;
        IsLock = !IsLock;
        if (IsLock)
        {
            //ѡ��һ�����˽�������
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
            //ȡ���۽���  ���ת��
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

    //���и�
    private void StartParry(InputAction.CallbackContext obj)
    {
        if (CanParry) 
        {
            playerAnimationInf.IsGuard = true;
            CancelRun();
        }
    }

    //ȡ����
    private void StopParry(InputAction.CallbackContext obj)
    {
        playerAnimationInf.IsGuard = false;
    }


    #endregion

    //ѡ�������ĵ���(�����������)
    private EnemyController SelectLockEnemy()
    {
       //return SelectEnemyCloseToPlayer(GameManager.Instance.enemies, float.MaxValue, Mathf.Acos(0f));;
       return SelectEnemyCloseToMouse(GameManager.Instance.enemies, float.MaxValue, Mathf.Acos(0f));
    }
    
    //���������ĵ���
    public EnemyController GetCurrentLockEnemy => curLockedEnemy;

    
    //�ۺ��ж��Ƿ��е��˿��Ա����� (�������ﵱǰ���򴦾�)
    private EnemyController SelectWeakEnemy()
    {
        return SelectEnemyCloseToPlayer(GameManager.Instance.weakEnemies, 4f, Mathf.Acos(0.35f));
    }

    //��ָ���ļ����и��ݵ�ǰ����ĳ���ѡ��һ������
    private EnemyController SelectEnemyCloseToPlayer(HashSet<EnemyController> enemies, float minDistance,float facing)
    {
        //����Ҫ�󣺾���С��ĳ��ֵ + �Ƕ�Ϊ�����ǰ��   (���ݽǶ�ѡ����ѵ�Ŀ��) 
        EnemyController currentEnemy = null;
        float minAngle = facing / Mathf.PI * 180;
        foreach (var enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < minDistance)
            {
                //enemy����Ҫ�� �Ƚϵõ����õĵ���
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
        //����Ҫ�󣺾���С��ĳ��ֵ + �Ƕ�Ϊ�����ǰ��   (���ݽǶ�ѡ����ѵ�Ŀ��) 
        EnemyController currentEnemy = null;
        //Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Vector3.Distance(Camera.main.transform.position, transform.position));
        //Vector3 startPos = Camera.main.ScreenToWorldPoint(screenPoint);
        Transform startTransform = Camera.main.transform;
        float minAngle = facing / Mathf.PI * 180;
        foreach (var enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < minDistance)
            {
                //enemy����Ҫ�� �Ƚϵõ����õĵ���
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


    #region Э��
    //ʹ��Lerpʵ�ּ��ٺͼ���Э��
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
    
    //��ǰ������Э��
    IEnumerator RollForward()
    {
        while(playerAnimationInf.IsRoll)
        {
            characterController.Move(transform.forward.normalized * rollSpeed * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// ��ת�����泯�ķ���
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

    #region UnityEvent�¼�
    public void OnEnemyDeath(EnemyController enemy)
    {
        //�����ǰ�����ĵ�����������������״̬
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
