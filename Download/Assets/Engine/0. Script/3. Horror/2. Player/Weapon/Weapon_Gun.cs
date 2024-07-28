using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class Weapon_Gun : Weapon<HorrorPlayer>
    {
        GameObject m_uiAim = null;

        public override void Initialize_Weapon(WeaponManagement<HorrorPlayer> weaponManagement)
        {
            base.Initialize_Weapon(weaponManagement);

            m_weaponIndex = (int)HorrorPlayer.WeaponId.WP_GUN;
            transform.localPosition = new Vector3(0.1f, 0.57f, 1.17f);

            m_uiAim = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Aim", GameObject.Find("Canvas").transform);
            m_uiAim.SetActive(false);
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
            RaycastHit hit = GameManager.Instance.Start_Raycast(transform.GetChild(0).transform.position, transform.GetChild(0).transform.forward, 10f, LayerMask.GetMask("Monster"));
            
            if(hit.collider != null)
                Debug.Log($"원거리공격 {hit.collider.gameObject.transform.parent.gameObject.name}");
        }
    }
}
