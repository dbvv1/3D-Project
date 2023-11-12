using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("引用")]
    [HideInInspector] public Animator anim;


    [HideInInspector]public EnemyCharacterStats enemyCharacterStats;

    protected EnemyData_SO enemyData;

    protected CharacterController characterController;

    protected StateMachineSystem stateMachineSystem;

    [Header("敌人属性")]
    public float curSpeed;

    public float walkSpeed;

    public float chaseSpeed;

    public float rotateSpeed;

    public float AttackDistanceNear { get => enemyData.attackDistanceNear; }
    public float AttackDistanceFar { get => enemyData.attackDistanceFar; }
    public float AttackStateDistance { get => enemyData.attackStateDistance; }

    public float killExp;

    [HideInInspector] public Vector3 orignalPosition;
    [HideInInspector] public PlayerCharacterStats player;
    [HideInInspector] public Vector3 partolTargetPos; //敌人巡逻时的目标点
    

    [Header("检测参数")]
    public float findPlayerRadius;

    public float patrolRadius;

    [SerializeField]protected LayerMask playerLayer;

    [SerializeField]protected LayerMask barrierLayer;

    private Collider[] playerCollider = new Collider[1];

    //动画层级
    private int animHurtLayer;
    private int animCombatLayer;

    #region 敌人状态
    protected bool findPlayer;
    public bool FindPlayer
    {
        get => findPlayer;
        set
        {
            findPlayer = value;
            anim.SetBool("FindPlayer", value);
        }
    }

    protected bool isWait;
    public bool IsWait
    {
        get => isWait;
        set
        {
            isWait = value;
            anim.SetBool("IsWait", value);
        }
    }

    protected bool isHurt;
    public bool IsHurt
    {
        get => anim.GetCurrentAnimatorStateInfo(animHurtLayer).IsTag("Hurt");
    }

    protected bool isDead;
    public bool IsDead
    {
        get => isDead;
        set
        {
            isDead = value;
            FindPlayer = false;
            IsWait = true;
            if (value) anim.SetTrigger("Death");
        }
    }

    protected bool isAttack;
    public bool IsAttack
    {
        get => anim.GetCurrentAnimatorStateInfo(animCombatLayer).IsTag("Attack");
    }

    protected bool isGuard; //当前是否处于格挡状态
    public bool IsGuard
    {
        get=>isGuard;set
        {
            isGuard = value;
            enemyCharacterStats.IsGuard = value;
        }
    }

    protected bool isExecuted;
    public bool IsExecuted { get => isExecuted;
        set
        {
            isExecuted = value;
            if (anim != null) anim.SetBool("IsExecuted", value);
            if (value)
            {
                GOPoolManager.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(4f,
                () => { IsExecuted = false;enemyCharacterStats.IsWeakState = false; GlobalEvent.CallEnemyExitWeakState(this); }) ;
            }
        }
    }

    #endregion

    protected virtual void Awake()
    {
        enemyCharacterStats = GetComponent<EnemyCharacterStats>();
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        stateMachineSystem = GetComponent<StateMachineSystem>();
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
        orignalPosition = transform.position;
        GameManager.Instance.RegisterEnemy(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.UnRegisterEnemy(this);
    }


    private void Update()
    {
        //默认受伤会造成僵直 boss可能会改变
        if (IsHurt||IsExecuted) return;
        FindPlayer = CanFindPlayer();
        if (!isDead && !IsWait && !IsHurt && !IsExecuted)  
            Move();
    }

    //敌人的移动
    protected virtual void Move()
    {
        if (Physics.Raycast(transform.position + transform.up * 0.5f + transform.forward * 2f, transform.forward.normalized, 2 * curSpeed * Time.deltaTime * transform.forward.normalized.magnitude, playerLayer | barrierLayer)) return;
        characterController.Move(curSpeed * Time.deltaTime * transform.forward.normalized);
    }

    private bool CanFindPlayer()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, findPlayerRadius, playerCollider, playerLayer);
        if (count == 0) return false;
        //从敌人位置向目标位置发射一条射线，判断中间有无障碍物
        float distance = Vector3.Distance(transform.position, playerCollider[0].transform.position);
        if (!Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward.normalized,distance,barrierLayer))
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
        yield return RotateToTatgetPos(targetPos);
        IsWait = false;
    }

    //使角色转向目标点的协程
    private  IEnumerator RotateToTatgetPos(Vector3 targetPos)
    {
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(targetPos.x,transform.position.y,targetPos.z) - transform.position);
        while (Quaternion.Angle(transform.rotation, toRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * rotateSpeed);
            yield return null;
        }
        curSpeed = stateMachineSystem.currentStateType switch
        {
            EnemyState.PatrolState => walkSpeed,
            EnemyState.ChaseState => chaseSpeed,
            EnemyState.AttackState => 0,
            _ => 0
        };
    }


    //在给定圆内随机生成一个巡逻点
    public Vector3 GetRandomPartolPoint()
    {
        float randomAngle = Random.value;
        float randomRadius = Random.value;
        randomRadius = Mathf.Sqrt(randomRadius); 
        randomAngle *= 2 * Mathf.PI;
        randomRadius *= patrolRadius;
        return new Vector3(orignalPosition.x + randomRadius * Mathf.Cos(randomAngle), transform.position.y, orignalPosition.z + randomRadius * Mathf.Sin(randomAngle));
    }

    //延迟取消被处决
    public void CancelExecution()
    {
        IsExecuted = false;
    }

    #region UnityEvent事件
    public void OnHurt()
    {
        //IsHurt = true;
        anim.SetTrigger("Hurt");
    }

    public void OnDeath()
    {
        IsDead = true;
        //TODO：给玩家涨经验

    }
    #endregion

    #region 动画事件
    public void AfterDeathAnimation()
    {
        //TODO:使用对象池管理敌人
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
        Gizmos.DrawRay(transform.position + transform.up * 0.5f+transform.forward*2, transform.forward.normalized);
        //Gizmos.DrawWireSphere(transform.position, findPlayerRadius);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(orignalPosition, patrolRadius);
    }

}
