using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Walk : HorrorPlayer_Base
    {
        private float m_soundTime = 0f;

        public HorrorPlayer_Walk(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
        }

        public override void Enter_State()
        {
            m_soundTime = 0f;

            Check_Stamina(); // 스테미나 회복 여부 판별
            m_player.MoveSpeed = 150f + GameManager.Ins.Horror.LevelController.Get_CurrentLevel<Horror_Base>().PlayerSpeedAdd;

            if (m_player.StateMachine.PreState != (int)HorrorPlayer.State.ST_RUN)
                Change_Animation("Walk");
                
            m_animator.speed = 0.8f;
            m_audioSource.pitch = 1f;
        }

        public override void Update_State()
        {
            Play_WalkSound(ref m_soundTime, 0.6f, 1f);

            if (Input.GetMouseButtonDown(0))
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_ATTACK);
            }
            else if (Input.GetKey(KeyCode.Space) && m_player.Stamina > 1)
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_RUN);
            }
            else if(Input_Move() == false) // 이동 입력 값이 없을 때
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_IDLE);
            }
            else
            {
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

