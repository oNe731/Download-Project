using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Attack : HorrorPlayer_Base
    {
        public HorrorPlayer_Attack(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
        }

        public override void Enter_State()
        {
            Debug.Log("공격 상태로 전환");
            m_player.WeaponManagement.Attack_Weapon();
        }

        public override void Update_State()
        {
            base.Update_State();
            m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
        }

        public override void Exit_State()
        {
        }
    }
}

