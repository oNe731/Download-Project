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

    [SerializeField] private Image[] m_lightImg;

    private Dictionary<string, Sprite> m_WeaponSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_lightSpr = new Dictionary<string, Sprite>();

    public void Initialize_UI()
    {
        m_WeaponSpr.Add("Gun", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/Weapon/WeaponSlot/UI_horror_PlayerWeapon_Gun"));
        m_WeaponSpr.Add("Flash", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/Weapon/WeaponSlot/UI_horror_PlayerWeapon_Flash"));
        m_WeaponSpr.Add("Pipe", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/Weapon/WeaponSlot/UI_horror_PlayerWeapon_Pipe"));

        m_lightSpr.Add("SlotON", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/Weapon/WeaponSlot/UI_horror_PlayerWeapon_SlotON"));
        m_lightSpr.Add("SlotOFF", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/Weapon/WeaponSlot/UI_horror_PlayerWeapon_SlotOFF"));

        GetComponent<RectTransform>().anchoredPosition = new Vector2(530.2f, -343f);

        gameObject.SetActive(false);
    }

    public void Update_UI(int count, POSITION position, NoteItem.ITEMTYPE weaponId, NoteItem.itemInfo weaponInfo)
    {
        /*// 순서 정렬
        int index = 0;
        switch (position)
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
        if (active == false)
            Update_Image(false);
        else
            Update_Image(true);*/

        // 개수에 따른 슬롯 비/활성화
        for(int i = 0; i < count; ++i)
            m_lightImg[i].gameObject.SetActive(true);
        for (int i = count; i < m_lightImg.Length; ++i)
            m_lightImg[i].gameObject.SetActive(false);

        // 순서 정렬
        for (int i = 0; i < m_lightImg.Length; ++i)
        {
            if(i == (int)position)
                m_lightImg[(int)position].sprite = m_lightSpr["SlotON"];
            else
                m_lightImg[i].sprite = m_lightSpr["SlotOFF"];
        }

        Update_Image(weaponId);
        Update_Info(weaponId, weaponInfo);
    }

    private void Update_Image(NoteItem.ITEMTYPE weaponId)
    {
        switch (weaponId)
        {
            case NoteItem.ITEMTYPE.TYPE_PIPE:
                // 무기 아이콘 변경(이미지)
                m_imageIcon.sprite = m_WeaponSpr["Pipe"]; //active ? m_WeaponSpr["Pipe_ON"] : m_WeaponSpr["Pipe_OFF"];
                // 무한대 표시(이미지)
                m_textTxt.gameObject.SetActive(false);
                m_textIcon.gameObject.SetActive(true);
                break;

            case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                // 무기 아이콘 변경(이미지)
                m_imageIcon.sprite = m_WeaponSpr["Flash"];
                // 무한대 표시(이미지)
                m_textTxt.gameObject.SetActive(false);
                m_textIcon.gameObject.SetActive(true);
                break;

            case NoteItem.ITEMTYPE.TYPE_GUN:
                // 무기 아이콘 변경(이미지)
                m_imageIcon.sprite = m_WeaponSpr["Gun"];
                // 총알 현재/ 최대 개수(폰트)
                m_textIcon.gameObject.SetActive(false);
                m_textTxt.gameObject.SetActive(true);
                break;
        }
    }

    public void Update_Info(NoteItem.ITEMTYPE weaponId, NoteItem.itemInfo weaponInfo)
    {
        switch (weaponId)
        {
            case NoteItem.ITEMTYPE.TYPE_GUN:
                int currentCount;

                Note note = HorrorManager.Instance.Player.Note;
                if (note == null)
                    currentCount = 0;
                else
                {
                    NoteItem noteItem = note.Get_Item(NoteItem.ITEMTYPE.TYPE_BULLET);
                    if (noteItem == null)
                        currentCount = 0;
                    else
                        currentCount = noteItem.m_count;
                }

                Weapon_Gun.GunInfo gunInfo = (Weapon_Gun.GunInfo)weaponInfo;
                m_textTxt.text = currentCount + "/ " + gunInfo.m_bulletMax.ToString();
                break;
        }
    }
}
