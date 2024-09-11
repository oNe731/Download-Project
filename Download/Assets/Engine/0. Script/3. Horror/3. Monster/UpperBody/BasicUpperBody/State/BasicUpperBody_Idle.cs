using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUpperBody_Idle : BasicUpperBody_Base
{
    private float m_change = 0f;
    private float m_time = 0f;

    public BasicUpperBody_Idle(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0f;
        m_change = Random.Range(0.5f, 1.5f);

        // m_animator.SetBool("IsIdle", true);
    }

    public override void Update_State()
    {
        if (Change_Attack() == false)
        {
            if (Change_Chase() == false)
            {
                m_time += Time.deltaTime;
                if (m_time >= m_change)
                {
                    m_owner.StateMachine.Change_State((int)BasicUpperBody.State.ST_WALK);
                }
            }
        }
    }

    public override void Exit_State()
    {
        // m_animator.SetBool("IsIdle", false);
    }
}
