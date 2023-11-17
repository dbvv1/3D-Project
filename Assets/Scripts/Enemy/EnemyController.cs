using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("����")]
    [HideInInspector] public Animator anim;


    [HideInInspector]public EnemyCharacterStats enemyCharacterStats;

    protected EnemyData_SO enemyData;

    protected CharacterController characterController;

    protected StateMachineSystem stateMachineSystem;

    [Header("��������")]
    public float curSpeed;

    public float walkSpeed;

    public float chaseSpeed;

    public float rotateSpeed;

    public float AttackDistanceNear => enemyData.attackDistanceNear;
    public float AttackDistanceFar => enemyData.attackDistanceFar;
    public float AttackStateDistance => enemyData.attackStateDistance;

    public float killExp;

    [HideInInspector] public Vector3 originalPosition;
    [HideInInspector] public PlayerCharacterStats player;
    [HideInInspector] public Vector3 patrolTargetPos; //����Ѳ��ʱ��Ŀ���
    

    [Header("������")]
    public float findPlayerRadius;

    public float patrolRadius;

    [SerializeField]protected LayerMask playerLayer;

    [SerializeField]protected LayerMask barrierLayer;

    private Collider[] playerCollider = new Collider[1];

    //�����㼶
    private int animHurtLayer;
    private int animCombatLayer;

    #region ����״̬
    protected bool findPlayer;
    public bool FindPlayer
    {
        get => findPlayer;
        set
        {
            findPlayer = value;
            anim.SetBool(Find, value);
        }
    }

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

    protected bool isHurt;
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

    protected bool isGuard; //��ǰ�Ƿ��ڸ�״̬
    public bool IsGuard
    {
        get=>isGuard;set
        {
            isGuard = value;
            enemyCharacterStats.IsGuard = value;
        }
    }

    protected bool isExecuted;
    private static readonly int Find = Animator.StringToHash("FindPlayer");
    private static readonly int Wait = Animator.StringToHash("IsWait");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Executed = Animator.StringToHash("IsExecuted");
    private static readonly int Hurt = Animator.StringToHash("Hurt");

    public bool IsExecuted { get => isExecuted;
        set
        {
            isExecuted = value;
            if (anim != null) anim.SetBool(Executed, value);
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
        originalPosition = transform.position;
        GameManager.Instance.RegisterEnemy(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.UnRegisterEnemy(this);
    }


    private void Update()
    {
        //Ĭ�����˻���ɽ�ֱ boss���ܻ�ı�
        if (IsHurt||IsExecuted) return;
        FindPlayer = CanFindPlayer();
        if (!isDead && !IsWait && !IsHurt && !IsExecuted)  
            Move();
    }

    //���˵��ƶ�
    protected virtual void Move()
    {
        if (Physics.Raycast(transform.position + transform.up * 0.5f + transform.forward * 2f, 
                transform.forward.normalized, 4 * curSpeed * Time.deltaTime * transform.forward.normalized.magnitude, playerLayer | barrierLayer)) return;
        characterController.Move(curSpeed * Time.deltaTime * transform.forward.normalized);
    }

    private bool CanFindPlayer()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, findPlayerRadius, playerCollider, playerLayer);
        if (count == 0) return false;
        //�ӵ���λ����Ŀ��λ�÷���һ�����ߣ��ж��м������ϰ���
        float distance = Vector3.Distance(transform.position, playerCollider[0].transform.position);
        if (!Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward.normalized,distance,barrierLayer))
        {
            //�жϵ����Ƿ��ǳ����ɫ��
            return transform.IsFacingTarget(player.transform);
        }
        return false;
    }


    //Ѳ�ߵȴ�ʱ��� Э��
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

    //ʹ��ɫת��Ŀ����Э��
    private  IEnumerator RotateToTargetPos(Vector3 targetPos)
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


    //�ڸ���Բ���������һ��Ѳ�ߵ�
    public Vector3 GetRandomPatrolPoint()
    {
        float randomAngle = Random.value;
        float randomRadius = Random.value;
        randomRadius = Mathf.Sqrt(randomRadius); 
        randomAngle *= 2 * Mathf.PI;
        randomRadius *= patrolRadius;
        return new Vector3(originalPosition.x + randomRadius * Mathf.Cos(randomAngle), transform.position.y, originalPosition.z + randomRadius * Mathf.Sin(randomAngle));
    }

    //�ӳ�ȡ��������
    public void CancelExecution()
    {
        IsExecuted = false;
    }

    #region UnityEvent�¼�
    public void OnHurt()
    {
        //IsHurt = true;
        anim.SetTrigger(Hurt);
    }

    public void OnDeath()
    {
        IsDead = true;
        //TODO��������Ǿ���

    }
    #endregion

    #region �����¼�
    public void AfterDeathAnimation()
    {
        //TODO:ʹ�ö���ع������
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
        //Gizmos.DrawWireSphere(originalPosition, patrolRadius);
    }

}
