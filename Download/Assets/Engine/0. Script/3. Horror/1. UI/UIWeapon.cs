using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Horror;

public class UIWeapon : MonoBehaviour
{
    public enum POSITION { PT_FRONT, PT_MIDDLE, PT_BACK, PT_END };
    private enum SPRITNAME { ST_GUN_ON, ST_GUN_OFF, ST_LANTERN_ON, ST_LANTERN_OFF, ST_PIPE_ON, ST_PIPE_OFF, ST_END };

    [SerializeField] private Image    m_imageIcon;

    [SerializeField] private Image    m_textIcon;
    [SerializeField] private TMP_Text m_textTxt;

    private NoteItem.ITEMTYPE m_weaponId;
    private Vector2[] m_position;
    private Dictionary<string, Sprite> m_WeaponSpr = new Dictionary<string, Sprite>();

    public void Initialize_UI(NoteItem.ITEMTYPE weaponId, NoteItem.itemInfo weaponInfo)
    {
        m_WeaponSpr.Add("Gun_OFF", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_WeaponSlot/UI_horror_WeaponSlot_Gun_OFF"));
        m_WeaponSpr.Add("Gun_ON", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_WeaponSlot/UI_horror_WeaponSlot_Gun_ON"));
        m_WeaponSpr.Add("Lantern_OFF", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_WeaponSlot/UI_horror_WeaponSlot_Lantern_OFF"));
        m_WeaponSpr.Add("Lantern_ON", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_WeaponSlot/UI_horror_WeaponSlot_Lantern_ON"));
        m_WeaponSpr.Add("Pipe_OFF", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_WeaponSlot/UI_horror_WeaponSlot_Pipe_OFF"));
        m_WeaponSpr.Add("Pipe_ON", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_WeaponSlot/UI_horror_WeaponSlot_Pipe_ON"));

        m_weaponId = weaponId;

        m_position = new Vector2[(int)POSITION.PT_END];
        m_position[(int)POSITION.PT_FRONT]  = new Vector2(499f, -409f);
        m_position[(int)POSITION.PT_MIDDLE] = new Vector2(512f, -417f);
        m_position[(int)POSITION.PT_BACK]   = new Vector2(525f, -425f);

        Update_Image(true);
        Update_Info(weaponInfo);
    }

    private void Update_Image(bool active)
    {
        switch (m_weaponId)
        {
            case NoteItem.ITEMTYPE.TYPE_PIPE:
                // 무기 아이콘 변경(이미지)
                m_imageIcon.sprite = active ? m_WeaponSpr["Pipe_ON"] : m_WeaponSpr["Pipe_OFF"]; 
                // 무한대 표시(이미지)
                m_textTxt.gameObject.SetActive(false);
                m_textIcon.gameObject.SetActive(active);
                break;

            case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                // 무기 아이콘 변경(이미지)
                m_imageIcon.sprite = active ? m_WeaponSpr["Lantern_ON"] : m_WeaponSpr["Lantern_OFF"];
                // 무한대 표시(이미지)
                m_textTxt.gameObject.SetActive(false);
                m_textIcon.gameObject.SetActive(active);
                break;

            case NoteItem.ITEMTYPE.TYPE_GUN:
                // 무기 아이콘 변경(이미지)
                m_imageIcon.sprite = active ? m_WeaponSpr["Gun_ON"] : m_WeaponSpr["Gun_OFF"];
                // 총알 현재/ 최대 개수(폰트)
                m_textIcon.gameObject.SetActive(false);
                m_textTxt.gameObject.SetActive(active);
                break;
        }
    }

    public void Update_UI(POSITION position, bool active)
    {
        // 순서 정렬
        int index = 0;
        switch(position)
        {
            case POSITION.PT_FRONT:
                index = (int)POSITION.PT_BACK;
                break;
            case POSITION.PT_MIDDLE:
                index = (int)POSITION.PT_MIDDLE;
                break;
            case POSITION.PT_BACK:
                index = (int)POSITION.PT_FRONT;
                break;
        }
        transform.SetSiblingIndex(index);

        // 위치 업데이트
        GetComponent<RectTransform>().anchoredPosition = m_position[(int)position];

        // 비활성화일시 상태 변경
        if(active == false)
            Update_Image(false);
        else
            Update_Image(true);
    }

    public void Update_Info(NoteItem.itemInfo weaponInfo)
    {
        switch (m_weaponId)
        {
            case NoteItem.ITEMTYPE.TYPE_GUN:
                int currentCount;
                NoteItem noteItem = HorrorManager.Instance.Player.Note.Get_Item(NoteItem.ITEMTYPE.TYPE_BULLET);
                if (noteItem == null)
                    currentCount = 0;
                else
                    currentCount = noteItem.m_count;

                Weapon_Gun.GunInfo gunInfo = (Weapon_Gun.GunInfo)weaponInfo;
                m_textTxt.text = currentCount + "/ " + gunInfo.m_bulletMax.ToString();
                break;
        }
    }
}
