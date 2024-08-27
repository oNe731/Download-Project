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
            //Debug.Log("공격 상태로 전환");
            if (m_player.WeaponManagement.Attack_Weapon() == false) // 공격 가능한 상태인가?
            {
                m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
                return;
            }

            Change_Animation("Attack");
        }

        public override void Update_State()
        {
            base.Update_State();

            // 현재 애니메이션 상태 확인
            if (m_animator.gameObject.activeSelf == true && m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_triggerName) == true)
            {
                Reset_Animation();

                float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (animTime >= 1.0f) // 애니메이션 종료
                    m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
            }
        }

        public override void Exit_State()
        {
            Reset_Animation();
        }
    }
}

