using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class Weapon_Flashlight : Weapon<HorrorPlayer>
    {
        public override void Initialize_Weapon(WeaponManagement<HorrorPlayer> weaponManagement)
        {
            base.Initialize_Weapon(weaponManagement);

            m_weaponIndex = (int)HorrorPlayer.WeaponId.WP_FLASHLIGHT;
            transform.localPosition = new Vector3(0.34f, 1.03f, 8.07f);
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
        }
    }
}

