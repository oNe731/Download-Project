using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Walk : HorrorPlayer_Base
    {
        public HorrorPlayer_Walk(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
        }

        public override void Enter_State()
        {
            Debug.Log("걷기 상태로 전환");
            m_moveSpeed = 400f;
        }

        public override void Update_State()
        {
            base.Update_State();

            if (Input.GetMouseButtonDown(0))
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_ATTACK);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_RUN);
            }
            else if(Input_Move() == false)
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_IDLE);
            }
            else
            {
                Input_Weapon();
                Input_Interaction();
            }
        }

        public override void Exit_State()
        {
        }
    }
}

