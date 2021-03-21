using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BT;

public class EnemyBase : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStatus
    {
        [SerializeField] float hp = 100;
        public float HP { get => hp; set => hp = value; }

        [Range(0.0f, 10.0f)]
        [SerializeField] float moveSpeed = 5.0f;
        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    }

    [System.Serializable]
    public class EnemyBehaviour
    {
        [SerializeField] float traceRange = -5.0f;
        public float TraceRange => traceRange;
        
        [SerializeField] float attackRange = -4.0f;
        public float AttackRange => attackRange;
        
        [SerializeField] float detectAttackRange = 2.0f;
        public float DetectAttackRange => detectAttackRange;

        [SerializeField] float jumpForce = 20.0f;
        public float JumpForce => jumpForce;

        [SerializeField] float jumpCoolTime = 0.1f;
        public float JumpCoolTime => jumpCoolTime;

        private float jumpAccumulate = 0.0f;
        public float JumpAccumulate { get => jumpAccumulate; set => jumpAccumulate = value; }

        private float backDashAccumulate = 0.0f;
        public float BackDashAccumulate { get => backDashAccumulate; set => backDashAccumulate = value; }

        [SerializeField] float backDashTime = 5.0f;
        public float BackDashTime { get => backDashTime; set => backDashTime = value; }

        [SerializeField] float backDashForce = 10.0f;
        public float BackDashForce { get => backDashForce; set => backDashForce = value; }

        [SerializeField] float attackCoolTime = 3.0f;
        public float AttackCoolTime => attackCoolTime;

        private float attackAccumulateTime = 0f;
        public float AttackAccumulateTime { get => attackAccumulateTime; set => attackAccumulateTime = value; }
    }
    
    [SerializeField] EnemyBehaviour enemyBehaviour;

    [SerializeField] EnemyStatus enemyStatus;

    

    private Sequence root = new Sequence();
    private Sequence patrol = new Sequence();
    private Selector detect = new Selector();



    private Sequence trace = new Sequence();

    private Sequence attack = new Sequence();
    private Selector attackSelector = new Selector();

    private Transform player = null;
    private Vector3 moveDest = Vector3.zero;
    private Vector3 attackDest = Vector3.zero;
    private int attackState = 0;

    private Rigidbody rigidbody;
    public Rigidbody Rigidbody
    {
        get
        {
            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody>();
            return rigidbody;
        }
    }

    public event System.Action OnEnemyDead;

    private void Awake()
    {
        OnEnemyDead += EnemyBase_OnEnemyDead;
    }

    private void EnemyBase_OnEnemyDead()
    {
        AnimateFactory(EEnemyAnimState.Dead);
        Destroy(gameObject, 5.0f);
    }
    public virtual void PreInit()
    {
        #region ATTACK
        attackSelector.AddChild(new BTCondition(Selector_Attack02));
        attackSelector.AddChild(new BTCondition(Selector_Attack01));

        attackSelector.AddChild(new BTCondition(AttackState));

        attack.AddChild(attackSelector);

        attack.AddChild(new BTCondition(() => {
            enemyBehaviour.AttackAccumulateTime += Time.deltaTime;

            if (enemyBehaviour.AttackAccumulateTime > enemyBehaviour.AttackCoolTime)
            {
                enemyBehaviour.AttackAccumulateTime = 0f;
                return true;
            }
            return false;
        }));
        attack.AddChild(new BTCondition(AttackArrive));
        #endregion
        #region TRACE
        trace.AddChild(new BTAction(TraceAction));
        trace.AddChild(new BTCondition(TraceArrive));
        trace.AddChild(new BTAction(TraceBackDash));
        #endregion
        #region DETECT
        detect.AddChild(new BTAction(DetectEvashionAction));
        detect.AddChild(new BTCondition(DetectJumpAccumulate));
        detect.AddChild(new BTCondition(DetectPatrol));
        #endregion

        #region PATROL
        patrol.AddChild(new BTCondition(Patrol));
        #endregion

        
    }

    public virtual void Init()
    {
        root.AddChild(detect);
        root.AddChild(attack);
        root.AddChild(trace);
        root.AddChild(patrol);
    }
    void Start()
    {
        PreInit();
        Init();
    }

    protected enum EEnemyAnimState
    {
        Idle,
        Trace,
        AttackState_1,
        AttackState_2,
        Dead,
        Hit,
        Jump,
    }

    protected virtual void AnimateFactory(EEnemyAnimState eEnemyAnimState)
    {
        switch (eEnemyAnimState)
        {
            case EEnemyAnimState.Trace:
                break;
            case EEnemyAnimState.AttackState_1:
                break;
            case EEnemyAnimState.AttackState_2:
                break;
            default:
                break;
        }
    }

    protected virtual bool DetectJumpAccumulate()
    {
        if (enemyBehaviour.JumpAccumulate > enemyBehaviour.JumpCoolTime)
        {
            return false;
        }

        return true;
    }

    protected virtual bool DetectEvashionAction()
    {
        //Debug.Log("회피");
        Rigidbody.AddForce(Vector2.left * enemyBehaviour.JumpForce, ForceMode.Impulse);
        enemyBehaviour.JumpAccumulate = 0f;
        return true;
    }

    protected virtual bool AttackState()
    {
        //Debug.Log("choice : " + attackState);
        attackState = Random.Range(0, 3);
        return false;
    }

    protected virtual bool TraceBackDash()
    {
        enemyBehaviour.BackDashAccumulate += Time.deltaTime;
        if (enemyBehaviour.BackDashAccumulate > enemyBehaviour.BackDashTime)
        {
            Dash(new Vector2(-1, 1), enemyBehaviour.BackDashForce);
            enemyBehaviour.BackDashTime = Random.Range(3, enemyBehaviour.BackDashTime);
            enemyBehaviour.BackDashAccumulate = 0f;
        }
        return true;
    }

    protected virtual bool TraceAction()
    {
        Trace(transform.position, moveDest, enemyStatus.MoveSpeed);
        return true;
    }

    protected virtual bool DetectPatrol()
    {
        enemyBehaviour.JumpAccumulate += Time.deltaTime;
        var overlapped = Physics.OverlapSphere(transform.position
            , enemyBehaviour.DetectAttackRange
            , LayerMask.GetMask("Attack"));
        if (overlapped.Length == 0)
        {
            Debug.Log("탐색중..");
            return true;
        }
        //player = overlapped[0].transform;
        return false;
    }
    protected virtual bool Patrol()
    {
        var overlapped = Physics.OverlapSphere(transform.position
                , float.MaxValue
                , LayerMask.GetMask("Player"));
        if (overlapped.Length == 0)
        {
            Debug.Log("플레이어 탐색중..");
            return false;
        }
        player = overlapped[0].transform;
        return true;
    }


    protected virtual bool IsArrive(Vector3 arg01, Vector3 arg02)
    {
        return Mathf.Approximately(arg01.sqrMagnitude, arg02.sqrMagnitude);
    }
    protected virtual void Trace(Vector3 from, Vector3 to, in float speed)
    {
        AnimateFactory(EEnemyAnimState.Trace);
        Rigidbody.AddForce((to - from) * speed, ForceMode.Force);
    }
    protected virtual void Dash(Vector2 dir, in float force)
    {
        Rigidbody.AddForce(dir * force, ForceMode.Impulse);
        AnimateFactory(EEnemyAnimState.Jump);
    }
    protected virtual bool Selector_Attack02()
    {
        //Attack
        if (attackState != 2)
            return false;
        AnimateFactory(EEnemyAnimState.AttackState_2);


        return true;
    }
    protected virtual bool Selector_Attack01()
    {
        //Attack
        if (attackState != 1)
            return false;

        AnimateFactory(EEnemyAnimState.AttackState_1);
        return true;
    }
    protected virtual bool AttackArrive()
    {
        Debug.Log("attack");
        attackDest = new Vector3(player.position.x + enemyBehaviour.AttackRange
            , transform.position.y, transform.position.z);
        if (IsArrive(transform.position, attackDest))
            return false;
        return true;
    }
    protected virtual bool TraceArrive()
    {
        moveDest = new Vector3(player.position.x + enemyBehaviour.TraceRange
                , transform.position.y, transform.position.z);
        if (IsArrive(transform.position, moveDest))
            return false;

        return true;
    }

    void Update()
    {
        if (IsDead())
            return;
        root.OnUpdate();
    }

    //protected virtual void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Skill"))
    //    {
    //        OnDamage(10);   // 임시
    //    }
    //}
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill"))
        {
            OnDamage(Player.player.Att);   // 임시
            AnimateFactory(EEnemyAnimState.Hit);
        }
    }

    bool IsDead()
    {
        return enemyStatus.HP <= 0;
    }

    void OnDamage(float amount)
    {
        enemyStatus.HP -= amount;
        if (IsDead())
            OnEnemyDead?.Invoke();
    }
}
