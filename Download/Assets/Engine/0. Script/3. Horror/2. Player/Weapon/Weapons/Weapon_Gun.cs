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

        public override void Initialize_Weapon(WeaponManagement<HorrorPlayer> weaponManagement, NoteItem noteItem, UIWeapon uIWeapon)
        {
            base.Initialize_Weapon(weaponManagement, noteItem, uIWeapon);

            m_damage = 2f;
            transform.localPosition = new Vector3(0f, 0f, 0f);
            transform.localRotation = Quaternion.Euler(0f, -3.101f, 0f);

            GunInfo gunInfo = new GunInfo();
            gunInfo.m_bulletMax = 50;
            m_itemInfo.m_itemInfo = gunInfo;

            // 조준점 생성
            m_uiAim = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/UI_Aim", GameObject.Find("Canvas").transform.Find("Panel_Play"));
            m_uiAim.SetActive(false);

            // 사격 이펙트 생성
            m_effect = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Weapon_Gun_Effect>();
            m_effect.Initialize_Effect();
        }

        public override void Enter_Weapon()
        {
            base.Enter_Weapon();
            m_uiAim.SetActive(true);
            GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Horror_Weapon_Gun_Install", false, 1f);
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

        public override bool Attack_Weapon()
        {
            bool empty = false;
            Note m_note = GameManager.Ins.Horror.Player.Note;
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
                GameManager.Ins.Horror.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_BASIC, UIInstruction.ACTIVETYPE.TYPE_FADE, activeTimes, texts);
                return false;
            }

            Attack_GunEffect(noteItem);


            RaycastHit hitHead = GameManager.Ins.Start_Raycast(Camera.main.transform.position, Camera.main.transform.forward, 10f, LayerMask.GetMask("Monster_Head"));
            if (hitHead.collider != null) // 헤드샷
            {
                ParentTarget parentTarget = hitHead.collider.GetComponent<ParentTarget>();
                if(parentTarget == null)
                    return true;

                Monster monster = parentTarget.ParentTransform.GetComponent<Monster>();
                if (monster == null)
                    return true;

                monster.Damage_Monster(m_damage * 2f);
                //Debug.Log("헤드샷");
            }
            else // 몸통샷
            {
                RaycastHit hitBody = GameManager.Ins.Start_Raycast(Camera.main.transform.position, Camera.main.transform.forward, 10f, LayerMask.GetMask("Monster_Body"));
                if (hitBody.collider != null)
                {
                    ParentTarget parentTarget = hitBody.collider.GetComponent<ParentTarget>();
                    if (parentTarget == null)
                        return true;

                    Monster monster = parentTarget.ParentTransform.GetComponent<Monster>();
                    if (monster == null)
                        return true;

                    monster.Damage_Monster(m_damage);
                    //Debug.Log("몸통샷");
                }
            }

            return true;
        }

        private void Attack_GunEffect(NoteItem noteItem)
        {
            // 이펙트 활성화
            m_effect.Reset_Effect();
            // 파티클 생성
            GameObject particle = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/Effect/GunshotDust/Effect_Gunshot");
            particle.transform.position = transform.GetChild(0).transform.position;

            // UI 업데이트
            noteItem.m_count--;
            if (noteItem.m_count < 0)
                noteItem.m_count = 0;
            GameManager.Ins.Horror.Player.Note.Set_Item(NoteItem.ITEMTYPE.TYPE_BULLET, noteItem);
            m_uIWeapon.Update_Info(m_itemInfo.m_itemType, m_itemInfo.m_itemInfo);

            // 사운드 재생
            GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Horror_Weapon_Gun_Attack", false, 1f);
        }
    }
}
