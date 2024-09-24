using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket_AttackWait : Straitjacket_Base
{
    private float m_time = 0f;

    public Straitjacket_AttackWait(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0f;

        Change_Animation("IsIdle");
    }

    public override void Update_State()
    {
        Play_Sound(3f, 5f);

        m_time += Time.deltaTime;
        if(m_time >= 1f)
        {
            if (Change_Attack() == false) // 거리가 일정 이상일 시 재 추격, 아닐 시 재공격
                m_stateMachine.Change_State((int)Straitjacket.State.ST_RUN);
        }

        if (m_animator.IsInTransition(0) == true) return;
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_triggerName) == true) Reset_Animation();
    }

    public override void Exit_State()
    {
        Reset_Animation();
    }
}
