using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Die : Bug_Base
{
    public Bug_Die(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        // ªÁ∏¡ ¿Ã∫•∆Æ √≥∏Æ
        Debug.Log("Bug ªÁ∏¡");
        GameManager.Ins.Resource.Destroy(m_owner.gameObject);
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }
}
