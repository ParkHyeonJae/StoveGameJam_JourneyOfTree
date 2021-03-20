using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BT;

public class EnemyBase : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStatus
    {
        [SerializeField] float hp = 20;
        public float HP { get => hp; set => hp = value; }

        [Range(0.0f, 10.0f)]
        [SerializeField] float moveSpeed = 5.0f;
        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    }

    [System.Serializable]
    public class EnemyBehaviour
    {
        [SerializeField] Animator bladeAnimator = null;
        public Animator BladeAnimator => bladeAnimator;
        [SerializeField] Animator legsAnimator = null;
        public Animator LegsAnimator => legsAnimator;

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


    void Start()
    {
        #region ATTACK
        attackSelector.AddChild(new BTCondition(Selector_Attack02));
        attackSelector.AddChild(new BTCondition(Selector_Attack01));

        attackSelector.AddChild(new BTCondition(() =>
        {
            //Debug.Log("choice : " + attackState);
            attackState = Random.Range(0, 3);
            return false;
        }));

        attack.AddChild(attackSelector);

        attack.AddChild(new BTCondition(AttackArrive));
        #endregion
        #region TRACE
        trace.AddChild(new BTAction(() =>
        {
            Trace(transform.position, moveDest, enemyStatus.MoveSpeed);
            return true;
        }));
        trace.AddChild(new BTCondition(TraceArrive));
        trace.AddChild(new BTAction(() =>
        {
            enemyBehaviour.BackDashAccumulate += Time.deltaTime;
            if (enemyBehaviour.BackDashAccumulate > enemyBehaviour.BackDashTime)
            {
                Dash(new Vector2(-1, 1), enemyBehaviour.BackDashForce);
                enemyBehaviour.BackDashTime = Random.Range(3, enemyBehaviour.BackDashTime);
                enemyBehaviour.BackDashAccumulate = 0f;
            }
            return true;
        }));
        #endregion
        #region DETECT

        detect.AddChild(new BTAction(() =>
        {
            //Debug.Log("회피");
            Rigidbody.AddForce(Vector2.left * enemyBehaviour.JumpForce, ForceMode.Impulse);
            enemyBehaviour.JumpAccumulate = 0f;
            return true;
        }));
        //detect.AddChild(new BTCondition(() =>
        //{
        //    50 %
        //    if (Random.Range(0, 100) <= 50)
        //        return false;

        //    return true;
        //}));
        detect.AddChild(new BTCondition(() =>
        {
            if (enemyBehaviour.JumpAccumulate > enemyBehaviour.JumpCoolTime)
            {
                return false;
            }

            return true;
        }));
        detect.AddChild(new BTCondition(() =>
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
        }));
        #endregion

        #region PATROL
        patrol.AddChild(new BTCondition(() =>
        {
            var overlapped = Physics.OverlapSphere(transform.position
                , float.MaxValue
                , LayerMask.GetMask("Player"));
            if (overlapped.Length == 0)
            {
                Debug.Log("탐색중..");
                return false;
            }
            player = overlapped[0].transform;
            return true;
        }));
        #endregion

        root.AddChild(detect);
        root.AddChild(attack);
        root.AddChild(trace);
        root.AddChild(patrol);
    }
    bool IsArrive(Vector3 arg01, Vector3 arg02)
    {
        return Mathf.Approximately(arg01.sqrMagnitude, arg02.sqrMagnitude);
    }
    void Trace(Vector3 from, Vector3 to, in float speed)
    {
        enemyBehaviour.LegsAnimator.Play("LegsWalk");
        Rigidbody.AddForce((to - from) * speed, ForceMode.Force);
    }
    void Dash(Vector2 dir, in float force)
    {
        Rigidbody.AddForce(dir * force, ForceMode.Impulse);
    }
    bool Selector_Attack02()
    {
        //Attack
        if (attackState != 2)
            return false;

        enemyBehaviour.BladeAnimator.SetInteger("AttackState", 2);
        enemyBehaviour.BladeAnimator.SetTrigger("Attack");
        return true;
    }
    bool Selector_Attack01()
    {
        //Attack
        if (attackState != 1)
            return false;

        enemyBehaviour.BladeAnimator.SetInteger("AttackState", 1);
        enemyBehaviour.BladeAnimator.SetTrigger("Attack");
        return true;
    }
    bool AttackArrive()
    {
        Debug.Log("attack");
        attackDest = new Vector3(player.position.x + enemyBehaviour.AttackRange
            , transform.position.y, transform.position.z);
        if (IsArrive(transform.position, attackDest))
            return false;
        return true;
    }
    bool TraceArrive()
    {
        moveDest = new Vector3(player.position.x + enemyBehaviour.TraceRange
                , transform.position.y, transform.position.z);
        if (IsArrive(transform.position, moveDest))
            return false;
        return true;
    }

    void Update()
    {
        root.OnUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Skill"))
        {
            OnDamage(10);   // 임시
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
