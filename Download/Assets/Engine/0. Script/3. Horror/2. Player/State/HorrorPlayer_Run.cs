using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Run : HorrorPlayer_Base
    {
        public HorrorPlayer_Run(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
        }

        public override void Enter_State()
        {
            //Debug.Log("뛰기 상태로 전환");
            m_moveSpeed = 700f;
        }

        public override void Update_State()
        {
            base.Update_State();

            if (Input.GetMouseButtonDown(0))
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_ATTACK);
            }
            else
            {
                Input_Weapon();
                Input_Interaction();
                if (Input_Move())
                {
                    m_player.Set_Stamina(-1f * Time.deltaTime); // 스테미나 사용
                    if (m_player.Stamina <= 0 || Input.GetKey(KeyCode.Space) == false)
                    {
                        m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_WALK);
                    }
                }
                else
                {
                    m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_IDLE);
                }
            }
        }

        public override void Exit_State()
        {
        }
    }
}

