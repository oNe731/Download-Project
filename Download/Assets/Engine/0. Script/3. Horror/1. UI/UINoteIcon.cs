using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Horror;

public class UINoteIcon : MonoBehaviour
{
    public enum TYPE { TYPE_WEAPON, TYPE_ITEM, TYPE_CLUE, TYPE_END }

    [SerializeField] private Image m_Iconimage;
    [SerializeField] private TMP_Text m_nameText;
    [SerializeField] private TMP_Text m_countText;

    public void Initialize_Icon(NoteItem noteItem)
    {
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);

        m_nameText.text = noteItem.m_name;
        switch (noteItem.m_noteType)
        {
            case NoteItem.NOTETYPE.TYPE_WEAPON:
                m_countText.gameObject.SetActive(false);
                switch (noteItem.m_itemType)
                {
                    case NoteItem.ITEMTYPE.TYPE_PIPE:
                        m_Iconimage.sprite = Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_Stick");
                        break;
                    case NoteItem.ITEMTYPE.TYPE_GUN:
                        m_Iconimage.sprite = Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_Gun");
                        break;
                    case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                        m_Iconimage.sprite = Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_Flashlight");
                        break;
                }
                break;

            case NoteItem.NOTETYPE.TYPE_ITEM:
                if(noteItem.m_count <= 1)
                    m_countText.gameObject.SetActive(false);
                else
                {
                    m_countText.gameObject.SetActive(true);
                    m_countText.text = noteItem.m_count.ToString();
                }
    
                switch (noteItem.m_itemType)
                {
                    case NoteItem.ITEMTYPE.TYPE_BULLET:
                        m_Iconimage.sprite = Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_Bullet");
                        HorrorManager.Instance.Player.WeaponManagement.Update_WeaponUI(NoteItem.ITEMTYPE.TYPE_GUN); // 무기 ui 업데이트
                        break;

                    case NoteItem.ITEMTYPE.TYPE_DRUG:
                        m_Iconimage.sprite = Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_Medicine");
                        break;
                }
                break;

            case NoteItem.NOTETYPE.TYPE_CLUE:
                if (noteItem.m_count <= 1)
                    m_countText.gameObject.SetActive(false);
                else
                {
                    m_countText.gameObject.SetActive(true);
                    m_countText.text = noteItem.m_count.ToString();
                }

                switch (noteItem.m_itemType)
                {
                    case NoteItem.ITEMTYPE.TYPE_CLUE1:
                        m_Iconimage.sprite = Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_clue");
                        break;

                    case NoteItem.ITEMTYPE.TYPE_CLUE2:
                        m_Iconimage.sprite = Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_clue");
                        break;

                    case NoteItem.ITEMTYPE.TYPE_CLUE3:
                        m_Iconimage.sprite = Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_clue");
                        break;

                    case NoteItem.ITEMTYPE.TYPE_CLUE4:
                        m_Iconimage.sprite = Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_clue");
                        break;

                    case NoteItem.ITEMTYPE.TYPE_CLUE5:
                        m_Iconimage.sprite = Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_clue");
                        break;
                }
                break;
        }


    }
}
