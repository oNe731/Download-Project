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

        public override void Initialize_Weapon(WeaponManagement<HorrorPlayer> weaponManagement, UIWeapon uIWeapon)
        {
            base.Initialize_Weapon(weaponManagement, uIWeapon);

            m_damage = 2f;
            transform.localPosition = new Vector3(0f, 0f, 0f);
            transform.localRotation = Quaternion.Euler(0f, -3.101f, 0f);

            GunInfo gunInfo = new GunInfo();
            gunInfo.m_bulletMax = 50;
            m_itemInfo.m_itemInfo = gunInfo;

            // 조준점 생성
            m_uiAim = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Aim", GameObject.Find("Canvas").transform.Find("Panel_Basic"));
            m_uiAim.SetActive(false);

            // 사격 이펙트 생성
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
                float[] activeTimes = new float[1];
                string[] texts = new string[1];
                activeTimes[0] = 1f;
                texts[0] = "탄창이 비어있다";
                HorrorManager.Instance.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_BASIC, UIInstruction.ACTIVETYPE.TYPE_FADE, activeTimes, texts);
                return;
            }

            // 이펙트 활성화
            m_effect.Reset_Effect();
            // 파티클 생성
            GameObject particle = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Effect/GunshotDust/Effect_Gunshot");
            particle.transform.position = transform.GetChild(0).transform.position;

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
            m_uIWeapon.Update_Info(m_itemInfo.m_itemType, m_itemInfo.m_itemInfo);
        }
    }
}
