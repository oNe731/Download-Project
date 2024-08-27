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
            //Debug.Log("걷기 상태로 전환");
            Check_Stamina();
            m_moveSpeed = 400f;

            Change_Animation("Walk");
        }

        public override void Update_State()
        {
            base.Update_State();

            if (m_animator.gameObject.activeSelf == true && m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_triggerName) == true)
                Reset_Animation();

            if (Input.GetMouseButtonDown(0))
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_ATTACK);
            }
            else if (Input.GetKey(KeyCode.Space) && m_player.Stamina > 0)
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_RUN);
            }
            else if(Input_Move() == false) // 이동 입력 값이 없을 때
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_IDLE);
            }
            else
            {
                Recover_Stamina();
                Input_Weapon();
                Input_Interaction();
            }
        }

        public override void Exit_State()
        {
            Reset_Animation();
        }
    }
}

