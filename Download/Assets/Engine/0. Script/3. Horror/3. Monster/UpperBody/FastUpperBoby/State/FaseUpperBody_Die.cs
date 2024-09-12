using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseUpperBody_Die : FaseUpperBody_Base
{
    public FaseUpperBody_Die(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        GameManager.Ins.Resource.Destroy(m_owner.gameObject);
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }
}
