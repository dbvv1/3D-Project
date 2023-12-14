using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class EnemyController : MonoBehaviour, ICreateFactory, IStateMachineEnemy
{
    [Header("引用")] [HideInInspector] public Animator anim;


    [HideInInspector] public EnemyCharacterStats enemyCharacterStats;

    protected EnemyData_SO enemyData;

    protected CharacterController characterController;

    //每个敌人的当前所属状态
    public StateActionSO CurrentEnemyState { get; set; }


    [Header("敌人属性")] public float curSpeed;

    public float walkSpeed;

    public float chaseSpeed;

    public float rotateSpeed;

    public float leaveChaseStateMinTime;

    private float leaveChaseStateMinTimeCounter;

    //在子类中设置const变量name 和 levelType   注意：C#中的const成员变量 默认是静态/隐式的 和C++不同
    public virtual string EnemyName => "Enemy";
    public virtual EnemyLevelType EnemyLevel => EnemyLevelType.Normal;

    public float AttackDistanceNear => enemyData.attackDistanceNear;
    public float AttackDistanceFar => enemyData.attackDistanceFar;
    public float AttackStateDistance => enemyData.attackStateDistance;

    [HideInInspector] public Vector3 originalPosition;
    [HideInInspector] public PlayerCharacterStats player;
    [HideInInspector] public Vector3 patrolTargetPos; //敌人巡逻时的目标点


    [Header("检测参数")] public float findPlayerRadius;

    public float patrolRadius;

    [SerializeField] protected LayerMask playerLayer;

    [SerializeField] protected LayerMask barrierLayer;

    [SerializeField] protected LayerMask enemyLayer;

    [SerializeField] protected LayerMask groundLayer;

    public Transform focusTransform;

    private readonly Collider[] playerCollider = new Collider[1];

    //动画层级
    protected int animHurtLayer;
    protected int animCombatLayer;

    //动画参数缓存
    private static readonly int Find = Animator.StringToHash("FindPlayer");
    private static readonly int Wait = Animator.StringToHash("IsWait");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Executed = Animator.StringToHash("IsExecuted");
    private static readonly int Hurt = Animator.StringToHash("Hurt");
    private static readonly int AttackNear = Animator.StringToHash("AttackNear");
    private static readonly int AttackFar = Animator.StringToHash("AttackFar");

    #region 敌人状态

    protected bool findPlayer;

    public bool FindPlayer
    {
        get => findPlayer;
        private set
        {
            findPlayer = value;
            if (value) leaveChaseStateMinTimeCounter = leaveChaseStateMinTime;
            else leaveChaseStateMinTimeCounter -= Time.deltaTime;
            anim.SetBool(Find, value);
        }
    }

    private bool IsInit { get; set; } = true;

    protected bool isWait;

    public bool IsWait
    {
        get => isWait;
        set
        {
            isWait = value;
            anim.SetBool(Wait, value);
        }
    }

    public bool IsHurt => anim.GetCurrentAnimatorStateInfo(animHurtLayer).IsTag("Hurt");

    protected bool isDead;

    public bool IsDead
    {
        get => isDead;
        set
        {
            isDead = value;
            FindPlayer = false;
            IsWait = true;
            if (value) anim.SetTrigger(Death);
        }
    }

    protected bool isAttack;
    public bool IsAttack => anim.GetCurrentAnimatorStateInfo(animCombatLayer).IsTag("Attack");

    protected bool isGuard; //当前是否处于格挡状态

    public bool IsGuard
    {
        get => isGuard;
        set
        {
            isGuard = value;
            enemyCharacterStats.IsGuard = value;
        }
    }


    protected bool isExecuted;

    public bool IsExecuted
    {
        get => isExecuted;
        set
        {
            isExecuted = value;
            if (anim != null) anim.SetBool(Executed, value);
            if (value)
            {
                PoolManager.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(4f,
                    () =>
                    {
                        IsExecuted = false;
                        enemyCharacterStats.IsWeakState = false;
                        GlobalEvent.CallEnemyExitWeakState(this);
                        enemyCharacterStats.CurEnergy = enemyCharacterStats.MaxEnergy;
                    });
            }
        }
    }

    #endregion

    protected virtual void Awake()
    {
        //SettingEnemyName();
        enemyCharacterStats = GetComponent<EnemyCharacterStats>();
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        enemyData = enemyCharacterStats.originalCharacterData as EnemyData_SO;
    }

    private void Start()
    {
        curSpeed = walkSpeed;
        player = GameManager.Instance.playerCurrentStats;
        animCombatLayer = anim.GetLayerIndex("Combat Layer");
        animHurtLayer = anim.GetLayerIndex("Hurt Layer");
    }

    private void OnEnable()
    {
        originalPosition = transform.position;
        GameManager.Instance.RegisterEnemy(this);
        RegisterStateMachineEnemy();
    }

    private void OnDisable()
    {
        //GameManager.Instance.UnRegisterEnemy(this); 调用顺序原因，将其先于AfterDeathAnimation的事件触发前执行
        GameManager.Instance.UnRegisterEnemy(this);     //在场景转换后仍然可能需要调用此函数
        TaskManager.Instance.UpdateTaskProgress(EnemyName, 1);
        UnRegisterStateMachineEnemy();
    }


    private void Update()
    {
        //默认受伤会造成僵直 boss可能会改变
        if (IsHurt || IsExecuted) return;
        FindPlayer = CanFindPlayer();
        if (!isDead && !IsWait && !IsHurt && !IsExecuted&&!IsInit)
             Move();
    }

    //设置敌人类型的名字 类型  //可以在prefab界面中进行赋值 或者 用Static成员变量
    //protected abstract void SettingEnemyName();

    public virtual void InitAfterGenerate()
    {
        //随机设置敌人的位置 以及如何判断给定的位置合法（没有其他的敌人） 参数可以在类中设置const字段单独设置
        bool isOccupied = true;
        Vector3 randomPos = Vector3.zero;
        while (isOccupied)
        {
            randomPos = new Vector3(Random.Range(-20f, 20f), 0f, Random.Range(-20f, 20f));
            isOccupied = Physics.CheckSphere(randomPos, 3f, barrierLayer | enemyLayer);
        }

        characterController.enabled = false;
        transform.position = randomPos;
        originalPosition = randomPos;
        characterController.enabled = true;

        IsInit = false;
        //后续可以增加特效的生成 等等额外内容
    }

    //敌人的移动
    protected virtual void Move()
    {
        if (anim.GetCurrentAnimatorStateInfo(animCombatLayer).IsTag("Attack")) return;
        if (Physics.Raycast(focusTransform.position + transform.forward * 2f,
                transform.forward.normalized, 4 * curSpeed * Time.deltaTime * transform.forward.normalized.magnitude,
                playerLayer | barrierLayer)) return;
        characterController.Move(curSpeed * Time.deltaTime * transform.forward.normalized);
    }

    private bool CanFindPlayer()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, findPlayerRadius, playerCollider, playerLayer);
        if (count == 0) return false;
        //从敌人位置向目标位置发射一条射线，判断中间有无障碍物
        float distance = Vector3.Distance(transform.position, playerCollider[0].transform.position);
        if (!Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward.normalized, distance,
                barrierLayer))
        {
            //判断敌人是否是朝向角色的
            return transform.IsFacingTarget(player.transform);
        }

        return false;
    }


    //巡逻等待时间的 协程
    public IEnumerator WaitPatrolTime(float waitTime, Vector3 targetPos)
    {
        IsWait = true;
        curSpeed = 0;
        while (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            yield return null;
        }

        yield return RotateToTargetPos(targetPos);
        IsWait = false;
    }

    //使角色转向目标点的协程
    private IEnumerator RotateToTargetPos(Vector3 targetPos)
    {
        Quaternion toRotation =
            Quaternion.LookRotation(new Vector3(targetPos.x, transform.position.y, targetPos.z) - transform.position);
        while (Quaternion.Angle(transform.rotation, toRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * rotateSpeed);
            yield return null;
        }

        curSpeed = CurrentEnemyState.enemyStateType switch
        {
            EnemyState.PatrolState => walkSpeed,
            EnemyState.ChaseState => chaseSpeed,
            EnemyState.AttackState => 0,
            _ => 0
        };
    }


    //在给定圆内随机生成一个巡逻点
    public Vector3 GetRandomPatrolPoint()
    {
        float randomAngle = Random.value;
        float randomRadius = Random.value;
        randomRadius = Mathf.Sqrt(randomRadius);
        randomAngle *= 2 * Mathf.PI;
        randomRadius *= patrolRadius;
        return new Vector3(originalPosition.x + randomRadius * Mathf.Cos(randomAngle), transform.position.y,
            originalPosition.z + randomRadius * Mathf.Sin(randomAngle));
    }

    //延迟取消被处决
    public void CancelExecution()
    {
        IsExecuted = false;
    }

    public bool CanReturnToPatrolState()
    {
        return !CanFindPlayer() && leaveChaseStateMinTimeCounter <= 0;
    }

    //进行近战攻击
    public virtual void AttackNearF()
    {
        if (!IsHurt)
            anim.SetTrigger(AttackNear);
    }

    //进行远程攻击
    public virtual void AttackFarF()
    {
        if (!IsHurt)
            anim.SetTrigger(AttackFar);
    }

    #region UnityEvent事件

    public virtual void OnHurt()
    {
        anim.SetTrigger(Hurt);
    }

    public void OnDeath()
    {
        IsDead = true;
    }

    #endregion

    #region 动画事件

    public void AfterDeathAnimation()
    {
        //TODO:使用对象池管理敌人
        GameManager.Instance.UnRegisterEnemy(this);
        GlobalEvent.CallOnEnemyDeath(this);
        GlobalEvent.CallEnemyExitWeakState(this);
        if (TryGetComponent(out LootSpawner lootSpawner))
        {
            lootSpawner.SpawnLoot();
        }

        Destroy(transform.gameObject);
    }

    #endregion

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(focusTransform.position + transform.forward * 2, transform.forward.normalized);
        //Gizmos.DrawWireSphere(transform.position, findPlayerRadius);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(originalPosition, patrolRadius);
    }

    //生成工厂接口
    public abstract EnemyFactory CreateFactory(EnemyController enemyPrefab);

    //注册状态机的接口
    public abstract void RegisterStateMachineEnemy();
    public abstract void UnRegisterStateMachineEnemy();
}