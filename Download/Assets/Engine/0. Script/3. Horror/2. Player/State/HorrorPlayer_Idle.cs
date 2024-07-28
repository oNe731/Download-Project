using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Idle : HorrorPlayer_Base
    {
        public HorrorPlayer_Idle(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
        }

        public override void Enter_State()
        {
            Debug.Log("아이들 상태로 전환");
        }

        public override void Update_State()
        {
            base.Update_State();

            if (Input.GetMouseButtonDown(0)) // 왼쪽 마우스 버튼
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_ATTACK);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_RUN);
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_WALK);
            }
            else
            {
                Input_Rotation();
                Input_Weapon();
                Input_Interaction();
            }
        }

        public override void Exit_State()
        {
        }
    }
}

