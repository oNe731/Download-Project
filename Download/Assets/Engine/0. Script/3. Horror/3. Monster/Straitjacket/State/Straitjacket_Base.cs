using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket_Base : State<Monster>
{
    protected Straitjacket m_owner  = null;
    protected Animator  m_animator  = null;
    protected Rigidbody m_rigidbody = null;
    protected AudioSource m_audioSource = null;

    protected Vector3 m_targetPosition;
    protected float   m_speed = 5f;

    protected float m_chaseDist  = 5f;
    protected float m_attackDist = 2.5f;

    private float m_soundTime = 0f;
    private float m_nextTime = 0f;

    public Straitjacket_Base(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_owner = m_stateMachine.Owner.GetComponent<Straitjacket>();
        m_animator = m_owner.Animator;

        m_rigidbody = m_owner.gameObject.GetComponent<Rigidbody>();
        m_rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        m_rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

        m_audioSource = m_owner.gameObject.GetComponent<AudioSource>();
    }

    public override void Enter_State()
    {
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
#if UNITY_EDITOR
        // 추격 범위 표시
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_stateMachine.Owner.transform.position, m_chaseDist);

        // 공격 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_stateMachine.Owner.transform.position, m_attackDist);
#endif
    }

    protected bool Change_Run() // 일정범위 내(스포너)로 주인공이 들어갈 시 주인공을 향해 달려든다.
    {
        // 플레이어가 일정 범위 내로 접근하면 추격 상태 전환
        float distanceToPlayer = Vector3.Distance(m_stateMachine.Owner.transform.position, HorrorManager.Instance.Player.transform.position);
        if (distanceToPlayer <= m_chaseDist)
        {
            m_stateMachine.Change_State((int)Straitjacket.State.ST_RUN); // 추격 상태로 전환
            return true;
        }

        return false;
    }

    protected bool Change_Attack()
    {
        // 플레이어가 일정 범위 내로 접근하면 공격 상태 전환
        float distanceToPlayer = Vector3.Distance(m_stateMachine.Owner.transform.position, HorrorManager.Instance.Player.transform.position);
        if (distanceToPlayer <= m_attackDist)
        {
            m_stateMachine.Change_State((int)Straitjacket.State.ST_ATTACK); // 공격 상태로 전환
            return true;
        }

        return false;
    }

    protected bool Check_Collider(Vector3 dir, int layerIndex) // ~0
    {
        Vector3 startPosition = m_owner.transform.position;
        startPosition.y += 0.3f;

        RaycastHit hit = GameManager.Ins.Start_Raycast(startPosition, dir, 1f, layerIndex);
        if (hit.collider != null)
            return true;

        return false;
    }

    protected void Play_Sound(float min, float max)
    {
        if(m_soundTime == 0f)
            m_nextTime = Random.Range(min, max);

        m_soundTime += Time.deltaTime;
        if(m_soundTime >= m_nextTime)
        {
            m_soundTime = 0f;    
            GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Horror_Straitjacket_Idle", false, 1f);
        }
    }
}
