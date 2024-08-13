using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Fly : Bug_Base
{
    public Bug_Fly(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_animator.SetBool("IsFly", true);
    }

    public override void Update_State()
    {
        m_owner.transform.LookAt(HorrorManager.Instance.Player.transform);
        m_owner.transform.Rotate(-90, 0, 0); // X축으로 -90도 회전 추가
    }

    public override void Exit_State()
    {
    }
}
