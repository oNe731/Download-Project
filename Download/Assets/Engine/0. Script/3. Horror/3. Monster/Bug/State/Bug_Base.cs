using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Base : State<Monster>
{
    protected Bug m_owner = null;
    protected float m_chaseDist = 3f;
    protected Animator m_animator = null;
    protected AudioSource m_audioSource = null;

    public Bug_Base(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_owner    = m_stateMachine.Owner.GetComponent<Bug>();
        m_animator = m_owner.Animator;
        m_audioSource = m_stateMachine.Owner.GetComponent<AudioSource>();
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

    protected bool Change_FLY()
    {
        // 플레이어가 일정 범위 내로 접근하면 비행 상태 전환
        float distanceToPlayer = Vector3.Distance(m_stateMachine.Owner.transform.position, HorrorManager.Instance.Player.transform.position);
        if (distanceToPlayer <= m_chaseDist)
        {
            m_stateMachine.Change_State((int)Bug.State.ST_FLY); // 비행 상태로 전환
            return true;
        }

        return false;
    }

    public override void OnDrawGizmos()
    {
#if UNITY_EDITOR
        // 추격 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_stateMachine.Owner.transform.position, m_chaseDist);
#endif
    }
}
