using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Attack : HorrorPlayer_Base
    {
        private bool isAttak = false;
        private NoteItem.ITEMTYPE m_weaponType;

        private float m_soundTime = 0f;

        public HorrorPlayer_Attack(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
        }

        public override void Enter_State()
        {
            // 현재 무기 장착 타입 검사
            NoteItem itemType = m_player.WeaponManagement.Get_CurrentWeaoponType();
            if(itemType == null)
            {
                m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
                return;
            }

            // 무기에 따른 공격 가능 여부 판별
            m_weaponType = itemType.m_itemType;
            switch (m_weaponType)
            {
                case NoteItem.ITEMTYPE.TYPE_GUN:
                    if (Attak_Weapon() == false) return;
                    break;

                case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                    m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
                    return;
            }    

            // 공격 애니메이션 변경
            Change_Animation("Attack");
            m_soundTime = 0f;
        }

        public override void Update_State()
        {
            if (Input_Move() == true) // 이동 입력 값이 있을 때
            {
                if(m_player.StateMachine.PreState == (int)HorrorPlayer.State.ST_WALK)
                    Play_WalkSound(ref m_soundTime, 0.6f, 1f);
                else
                    Play_WalkSound(ref m_soundTime, 0.4f, 1f);
            }

            // 현재 애니메이션 상태 확인
            if (m_animator.gameObject.activeSelf == false || m_animator.IsInTransition(0) == true) return;
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_triggerName) == true)
            {
                Reset_Animation();

                float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if(isAttak == false)
                {
                    switch (m_weaponType)
                    {
                        case NoteItem.ITEMTYPE.TYPE_PIPE:
                            if (animTime >= 0.35f)
                            {
                                Attak_Weapon();
                            }
                            break;
                    }
                }

                if (animTime >= 1.0f) // 애니메이션 종료
                    m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
            }
        }

        public override void Exit_State()
        {
            Reset_Animation();
            isAttak = false;
        }

        private bool Attak_Weapon()
        {
            if (m_player.WeaponManagement.Attack_Weapon() == false) // 공격 시도, 공격 가능한 상태인가?
            {
                m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
                return false;
            }

            isAttak = true;
            return true;
        }
    }
}

