using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket_RunWait : Straitjacket_Base
{
    public Straitjacket_RunWait(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        Change_Animation("IsIdle");
    }

    public override void Update_State()
    {
        Play_Sound(3f, 5f);

        if (Change_Attack() == false)
        {
            m_targetPosition = GameManager.Ins.Horror.Player.transform.position;

            Vector3 direction = m_targetPosition - m_owner.transform.position;
            direction.y = 0;
            direction = direction.normalized;

            if (Check_Collider(direction, LayerMask.GetMask("Monster")) == false)
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
