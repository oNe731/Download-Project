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
            Check_Stamina(); // 스테미나 회복 여부 판별

            Change_Animation("Idle"); // 아이들 애니메이션 재생
        }

        public override void Update_State()
        {
            if (Input.GetMouseButtonDown(0)) // 왼쪽 마우스 버튼
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_ATTACK);
            }
            else if (Input.GetKey(KeyCode.Space) && m_player.Stamina > 0)
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_RUN);
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_WALK);
            }
            else
            {
                Input_Rotation();
                Input_Interaction();
                Input_Weapon();
                Recover_Stamina();
            }

            Update_Gravity();

            if (m_animator.gameObject.activeSelf == false || m_animator.IsInTransition(0) == true) return; // 손이 활성화 상태인가/ true == 애니메이션 보간중
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_triggerName) == true) Reset_Animation();
        }

        public override void Exit_State()
        {
            Reset_Animation();
        }
    }
}

