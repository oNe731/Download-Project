using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class Weapon_Melee : Weapon<HorrorPlayer>
    {
        private BoxCollider m_attackCollider = null;

        public override void Initialize_Weapon(WeaponManagement<HorrorPlayer> weaponManagement, NoteItem noteItem, UIWeapon uIWeapon)
        {
            base.Initialize_Weapon(weaponManagement, noteItem, uIWeapon);

            m_damage = 1f;
            transform.localPosition = new Vector3(0f, 0f, 0f);
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            m_attackCollider = gameObject.transform.GetChild(0).GetChild(0).GetComponent<BoxCollider>();
        }

        public override void Enter_Weapon()
        {
            base.Enter_Weapon();
            GameManager.Ins.Sound.Play_AudioSource(ref m_audioSource, "Horror_Weapon_Bbaru_Install", false, 1f);
        }

        public override void Update_Weapon()
        {
            base.Update_Weapon();
        }

        public override void Exit_Weapon()
        {
            base.Exit_Weapon();
        }

        public override bool Attack_Weapon()
        {
            GameManager.Ins.Sound.Play_AudioSource(ref m_audioSource, "Horror_Weapon_Bbaru_Attack", false, 1f);

            // 박스 콜라이더의 중심과 크기
            Vector3 center      = m_attackCollider.bounds.center;
            Vector3 halfExtents = m_attackCollider.bounds.extents;

            // 몬스터 레이어에 있는 모든 오브젝트와 충돌 검사
            Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, 1 << LayerMask.NameToLayer("Monster"));
            foreach (var hitCollider in hitColliders)
            {
                // Debug.Log($"근접공격 {hitCollider.gameObject.transform.parent.parent.parent.gameObject.name}");
                Monster monster = hitCollider.gameObject.transform.parent.parent.parent.GetComponent<Monster>();
                if (monster == null)
                    continue;
                GameManager.Ins.Sound.Play_AudioSource(ref m_audioSource, "Horror_Weapon_Bbaru_Damaged", false, 1f);
                monster.Damage_Monster(m_damage);
            }

            return true;
        }
    }
}

