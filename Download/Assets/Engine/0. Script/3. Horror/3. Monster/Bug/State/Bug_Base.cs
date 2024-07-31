using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Base : State<Monster>
{
    protected Bug m_owner = null;

    public Bug_Base(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_owner = m_stateMachine.Owner.GetComponent<Bug>();
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
}
