using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket_Wait : Straitjacket_Base
{
    public Straitjacket_Wait(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_animator.SetBool("IsIdle", true);
    }

    public override void Update_State()
    {
        Play_Sound(3f, 5f);

        if (Change_Attack() == false)
        {
            m_targetPosition = HorrorManager.Instance.Player.transform.position;

            Vector3 direction = m_targetPosition - m_owner.transform.position;
            direction.y = 0;
            direction = direction.normalized;

            if (Check_Collider(direction, LayerMask.GetMask("Monster")) == false)
                m_stateMachine.Change_State((int)Straitjacket.State.ST_RUN);
        }
    }

    public override void Exit_State()
    {
        m_animator.SetBool("IsIdle", false);
    }
}
