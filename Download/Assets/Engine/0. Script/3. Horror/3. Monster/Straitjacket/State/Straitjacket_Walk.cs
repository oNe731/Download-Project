using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket_Walk : Straitjacket_Base
{
    private float m_change = 0f;
    private float m_time = 0f;

    public Straitjacket_Walk(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_speed = 2.5f;
    }

    public override void Enter_State()
    {
        base.Enter_State();

        Reset_RandomDirection();

        m_time = 0f;
        m_change = Random.Range(2.5f, 5.0f);

        m_animator.SetBool("IsWalk", true);
    }

    public override void Update_State()
    {
        Play_Sound(2f, 4f);

        if (Change_Attack() == false)
        {
            if (Change_Run() == false)
            {
                m_time += Time.deltaTime;
                if (m_time >= m_change)
                    m_owner.StateMachine.Change_State((int)Straitjacket.State.ST_IDLE);
                else
                    Move_Monster();
            }
        }
    }

    public override void Exit_State()
    {
        base.Exit_State();

        m_animator.SetBool("IsWalk", false);
    }
}
