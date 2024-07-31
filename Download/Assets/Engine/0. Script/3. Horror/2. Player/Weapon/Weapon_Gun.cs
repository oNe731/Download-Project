using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class Weapon_Gun : Weapon<HorrorPlayer>
    {
        public struct GunInfo : WeaponInfo
        {
            public int m_bulletCount { get; set; }
            public int m_bulletMax { get; set; }
        }

        GameObject m_uiAim = null;

        public override void Initialize_Weapon(WeaponManagement<HorrorPlayer> weaponManagement, UIWeapon uIWeapon)
        {
            base.Initialize_Weapon(weaponManagement, uIWeapon);

            m_damage = 2f;
            m_weaponID = WeaponId.WP_GUN;
            transform.localPosition = new Vector3(0.1f, 0.57f, 1.17f);

            m_uiAim = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Aim", GameObject.Find("Canvas").transform);
            m_uiAim.SetActive(false);

            GunInfo gunInfo = new GunInfo();
            gunInfo.m_bulletMax   = 10;
            gunInfo.m_bulletCount = gunInfo.m_bulletMax;
            m_weaponInfo = gunInfo;
        }

        public override void Enter_Weapon()
        {
            base.Enter_Weapon();
            m_uiAim.SetActive(true);
        }

        public override void Update_Weapon()
        {
            base.Update_Weapon();
        }

        public override void Exit_Weapon()
        {
            base.Exit_Weapon();
            m_uiAim.SetActive(false);
        }

        public override void Attack_Weapon()
        {
            GunInfo info = (GunInfo)m_weaponInfo;
            if (info.m_bulletCount <= 0)
                return;

            RaycastHit hit = GameManager.Instance.Start_Raycast(transform.GetChild(0).transform.position, transform.GetChild(0).transform.forward, 10f, LayerMask.GetMask("Monster"));
            if(hit.collider != null)
            {
                Debug.Log($"원거리공격 {hit.collider.gameObject.transform.parent.gameObject.name}");
                
                Monster monster = hit.collider.gameObject.GetComponent<Monster>();
                if (monster == null)
                    return;
                monster.Damage_Monster(m_damage);
            }

            // UI 업데이트
            info.m_bulletCount--;
            if (info.m_bulletCount < 0)
                info.m_bulletCount = 0;
            m_weaponInfo = info;
            m_uIWeapon.Update_Info(m_weaponInfo);
        }
    }
}
