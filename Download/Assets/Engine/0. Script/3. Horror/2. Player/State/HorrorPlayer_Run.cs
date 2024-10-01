using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Run : HorrorPlayer_Base
    {
        private float m_soundTime = 0f;

        public HorrorPlayer_Run(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
        }

        public override void Enter_State()
        {
            m_soundTime = 0f;

            m_player.MoveSpeed = 400f + GameManager.Ins.Horror.LevelController.Get_CurrentLevel<Horror_Base>().PlayerSpeedAdd;

            if (m_player.StateMachine.PreState != (int)HorrorPlayer.State.ST_WALK)
                Change_Animation("Walk");
                
            m_animator.speed = 1.3f;
            m_audioSource.pitch = 1.8f;
        }

        public override void Update_State()
        {
            Play_WalkSound(ref m_soundTime, 0.4f, 1f);

            if (Input.GetMouseButtonDown(0))
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_ATTACK);
            }
            else
            {
                Input_Interaction();
                Input_Weapon();

                if (Input_Move() == true)
                { 
                    m_player.Set_Stamina(-1f * Time.deltaTime); // 스테미나 사용

                    if (m_player.Stamina <= 0 || Input.GetKey(KeyCode.Space) == false)
                        m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_WALK);
                }
                else
                {
                    m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_IDLE);
                }
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

