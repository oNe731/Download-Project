using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket_Die : Straitjacket_Base
{
    public Straitjacket_Die(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_animator.SetBool("IsDie", true);
        GameManager.Ins.Resource.Destroy(m_owner.gameObject);
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }
}
