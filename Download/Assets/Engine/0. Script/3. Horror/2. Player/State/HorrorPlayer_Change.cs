using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Change : HorrorPlayer_Base
    {
        private bool m_preWalk = true;
        private float m_soundTime = 0f;

        public HorrorPlayer_Change(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
        }

        public override void Enter_State()
        {
            Change_Animation("Hold", true);
            m_soundTime = 0f;

            // 이전 무기 비활성화
            if (m_player.WeaponManagement.PreWeapon != -1)
            {
                m_player.WeaponManagement.Weapons[m_player.WeaponManagement.PreWeapon].gameObject.SetActive(false);
                m_player.WeaponManagement.Weapons[m_player.WeaponManagement.PreWeapon].gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }

            // 현재 무기 활성화
            m_player.WeaponManagement.Weapons[m_player.WeaponManagement.CurWeapon].gameObject.SetActive(true);

            Check_PreStateWalk(ref m_preWalk);
        }

        public override void Update_State()
        {
            Update_PreStateWalk(ref m_preWalk, ref m_soundTime);

            if (m_animator.gameObject.activeSelf == false || m_animator.IsInTransition(0) == true) return;
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_triggerName) == true)
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
            m_player.WeaponManagement.Weapons[m_player.WeaponManagement.CurWeapon].gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}

