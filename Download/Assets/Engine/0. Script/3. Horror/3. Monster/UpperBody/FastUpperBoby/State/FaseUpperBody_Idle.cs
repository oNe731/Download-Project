using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FaseUpperBody_Idle : FaseUpperBody_Base
{
    private float m_change = 0f;
    private float m_time = 0f;

    private NavMeshAgent m_agent;

    public FaseUpperBody_Idle(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_agent = m_owner.GetComponent<NavMeshAgent>();
    }

    public override void Enter_State()
    {
        m_time = 0f;
        m_change = Random.Range(0.5f, 1f);

        // 바닥에서 활동/ 원래 바닥에 있던 몹은 그대로 바닥에서 시작
        m_agent.enabled = true;

        // m_animator.SetBool("IsIdle", true);
    }

    public override void Update_State()
    {
        m_time += Time.deltaTime;
        if (m_time >= m_change)
        {
            m_owner.StateMachine.Change_State((int)FaseUpperBody.State.ST_CHASE);
        }
    }

    public override void Exit_State()
    {
        m_agent.enabled = false;
    }
}
