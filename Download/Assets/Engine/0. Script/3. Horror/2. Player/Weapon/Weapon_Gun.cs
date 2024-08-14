using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class Weapon_Gun : Weapon<HorrorPlayer>
    {
        public struct GunInfo : NoteItem.itemInfo
        {
            public int m_bulletMax { get; set; }
        }

        GameObject m_uiAim  = null;
        Weapon_Gun_Effect m_effect = null;

        private GameObject ui = null;

        public override void Initialize_Weapon(WeaponManagement<HorrorPlayer> weaponManagement, UIWeapon uIWeapon)
        {
            base.Initialize_Weapon(weaponManagement, uIWeapon);

            m_itemInfo.m_name = "권총";
            m_itemInfo.m_count = 1;
            m_itemInfo.m_noteType = NoteItem.NOTETYPE.TYPE_WEAPON;
            m_itemInfo.m_itemType = NoteItem.ITEMTYPE.TYPE_GUN;

            m_damage = 2f;
            GunInfo gunInfo = new GunInfo();
            gunInfo.m_bulletMax = 50;
            m_itemInfo.m_itemInfo = gunInfo;

            transform.localPosition = new Vector3(0f, 0f, 0f);
            transform.localRotation = Quaternion.Euler(0f, -3.101f, 0f);

            m_uiAim = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Aim", GameObject.Find("Canvas").transform);
            m_uiAim.SetActive(false);

            m_effect = transform.GetChild(2).gameObject.GetComponent<Weapon_Gun_Effect>();
            m_effect.Initialize_Effect();
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
            bool empty = false;

            Note m_note = HorrorManager.Instance.Player.Note;
            NoteItem noteItem = null;
            if (m_note == null)
                empty = true;
            else
            {
                noteItem = m_note.Get_Item(NoteItem.ITEMTYPE.TYPE_BULLET);
                if (noteItem == null || noteItem.m_count <= 0)
                    empty = true;
            }

            if(empty == true)
            {
                if (ui == null)
                    ui = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Popup", GameObject.Find("Canvas").transform.GetChild(2));
                
                if (ui == null)
                    return;
                UIPopup.Expendables info = new UIPopup.Expendables();
                info.text = "탄창이 비어있다";
                ui.GetComponent<UIPopup>().Initialize_UI(UIPopup.TYPE.T_EXPENDABLES, info);
                return;
            }

            // 이펙트 활성화
            m_effect.Reset_Effect();

            RaycastHit hit = GameManager.Instance.Start_Raycast(Camera.main.transform.position, Camera.main.transform.forward, 10f, LayerMask.GetMask("Monster"));
            if (hit.collider != null)
            {
                Debug.Log($"원거리공격 {hit.collider.gameObject.transform.parent.parent.parent.gameObject.name}");

                Monster monster = hit.collider.gameObject.transform.parent.parent.parent.GetComponent<Monster>();
                if (monster == null)
                    return;
                monster.Damage_Monster(m_damage);
            }

            // UI 업데이트
            noteItem.m_count--;
            if (noteItem.m_count < 0)
                noteItem.m_count = 0;
            HorrorManager.Instance.Player.Note.Set_Item(NoteItem.ITEMTYPE.TYPE_BULLET, noteItem);
            m_uIWeapon.Update_Info(m_itemInfo.m_itemInfo);
        }
    }
}
