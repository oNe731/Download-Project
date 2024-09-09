using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Attack : Bug_Base
{
    public Bug_Attack(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        Look_Player();
        HorrorManager.Instance.Player.Damage_Player(m_owner.Attack);
    }

    public override void Update_State()
    {
        m_stateMachine.Change_State((int)Bug.State.ST_RETREAT); // 후퇴 상태로 전환
    }

    public override void Exit_State()
    {
    }
}
