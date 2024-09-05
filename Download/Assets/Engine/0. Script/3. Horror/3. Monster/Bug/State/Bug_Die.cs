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
        // 사망 이벤트 처리
        GameManager.Ins.Resource.Destroy(m_owner.gameObject);
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }
}
