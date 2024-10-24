using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket_Idle : Straitjacket_Base
{
    private float m_change = 0f;
    private float m_time = 0f;

    public Straitjacket_Idle(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0f;
        m_change = Random.Range(0.5f, 1.5f);

        Change_Animation("IsIdle");
    }

    public override void Update_State()
    {
        Play_Sound(3f, 5f);

        if (Change_Attack() == false)
        {
            if(Change_Run() == false)
            {
                m_time += Time.deltaTime;
                if (m_time >= m_change)
                {
                    m_owner.StateMachine.Change_State((int)Straitjacket.State.ST_WALK);
                }
            }
        }

        if (m_animator.IsInTransition(0) == true) return;
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_triggerName) == true) Reset_Animation();
    }

    public override void Exit_State()
    {
        Reset_Animation();
    }
}
