using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class Weapon_Flashlight : Weapon<HorrorPlayer>
    {
        public override void Initialize_Weapon(WeaponManagement<HorrorPlayer> weaponManagement, NoteItem noteItem, UIWeapon uIWeapon)
        {
            base.Initialize_Weapon(weaponManagement, noteItem, uIWeapon);

            m_damage = 0f;
            transform.localPosition = new Vector3(0f, 0f, 0f);
            transform.localRotation = Quaternion.Euler(0f, -2.92f, -2.73f);
        }

        public override void Enter_Weapon()
        {
            base.Enter_Weapon();
            GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Horror_Weapon_Handlight_Install", false, 1f);
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
            return false;
        }
    }
}

