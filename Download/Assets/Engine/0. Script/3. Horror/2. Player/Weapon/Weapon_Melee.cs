using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class Weapon_Melee : Weapon<HorrorPlayer>
    {
        private BoxCollider m_attackCollider = null;

        public override void Initialize_Weapon(WeaponManagement<HorrorPlayer> weaponManagement, UIWeapon uIWeapon)
        {
            base.Initialize_Weapon(weaponManagement, uIWeapon);

            m_damage = 1f;
            m_weaponID = WeaponId.WP_MELEE;
            transform.localPosition = new Vector3(0f, 0.023f, 0f);

            m_attackCollider = gameObject.transform.GetChild(1).GetComponent<BoxCollider>();
        }

        public override void Enter_Weapon()
        {
            base.Enter_Weapon();
        }

        public override void Update_Weapon()
        {
            base.Update_Weapon();
        }

        public override void Exit_Weapon()
        {
            base.Exit_Weapon();
        }

        public override void Attack_Weapon()
        {
            Debug.Log("근접공격");

            // 박스 콜라이더의 중심과 크기
            Vector3 center      = m_attackCollider.bounds.center;
            Vector3 halfExtents = m_attackCollider.bounds.extents;

            // 몬스터 레이어에 있는 모든 오브젝트와 충돌 검사
            Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, 1 << LayerMask.NameToLayer("Monster"));
            foreach (var hitCollider in hitColliders)
            {
                Debug.Log($"근접공격 {hitCollider.gameObject.transform.parent.gameObject.name}");

                Monster monster = hitCollider.gameObject.GetComponent<Monster>();
                if (monster == null)
                    return;
                monster.Damage_Monster(m_damage);
            }
        }
    }
}

