using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class Weapon_Flashlight : Weapon<HorrorPlayer>
    {
        public override void Initialize_Weapon(WeaponManagement<HorrorPlayer> weaponManagement, UIWeapon uIWeapon)
        {
            base.Initialize_Weapon(weaponManagement, uIWeapon);

            m_itemInfo.m_name = "¼ÕÀüµî";
            m_itemInfo.m_count = 1;
            m_itemInfo.m_noteType = NoteItem.NOTETYPE.TYPE_WEAPON;
            m_itemInfo.m_itemType = NoteItem.ITEMTYPE.TYPE_FLASHLIGHT;
            transform.localPosition = new Vector3(0f, 0f, 0f);
            transform.localRotation = Quaternion.Euler(0f, -2.92f, -2.73f);
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

