using Horror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGetItem : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;

    private NoteItem m_itemType;

    public void Initialize_UI(NoteItem noteItem)
    {
        m_itemType = noteItem;

        Transform cameraTransform = HorrorManager.Instance.Player.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(0);
        switch (m_itemType.m_itemType)
        {
            case NoteItem.ITEMTYPE.TYPE_PIPE:
                m_text.text = "파이프 획득";
                HorrorManager.Instance.Player.WeaponManagement.Add_Weapon(GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Character/Weapon/Melee", cameraTransform).GetComponent<Weapon<HorrorPlayer>>());
                break;
            case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                m_text.text = "손전등 획득";
                HorrorManager.Instance.Player.WeaponManagement.Add_Weapon(GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Character/Weapon/Flashlight", cameraTransform).GetComponent<Weapon<HorrorPlayer>>());
                break;
            case NoteItem.ITEMTYPE.TYPE_GUN:
                m_text.text = "총 획득";
                HorrorManager.Instance.Player.WeaponManagement.Add_Weapon(GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Character/Weapon/Gun", cameraTransform).GetComponent<Weapon<HorrorPlayer>>());
                break;

            case NoteItem.ITEMTYPE.TYPE_NOTE:
                m_text.text = "동료의 수첩 획득";
                HorrorManager.Instance.Player.Acquire_Note();
                break;

            default:
                m_text.text = m_itemType.m_name + " 획득";
                Note playerNote = HorrorManager.Instance.Player.Note;
                if (playerNote == null)
                    return;
                playerNote.Add_Proviso(m_itemType);
                break;
        }
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            HorrorManager.Instance.Set_Pause(false);

            GameObject ui = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Popup", GameObject.Find("Canvas").transform.GetChild(2));
            if (ui == null)
                return;

            UIPopup.Expendables info = new UIPopup.Expendables();

            switch (m_itemType.m_itemType)
            {
                case NoteItem.ITEMTYPE.TYPE_PIPE:
                    info.text = "아이템 ‘파이프’를 얻었다.\n[TAB]으로 사용 가능";
                    break;
                case NoteItem.ITEMTYPE.TYPE_GUN:
                    info.text = "[Ctrl]로 장비교체 가능";
                    break;
                case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                    info.text = "[Ctrl]로 장비교체 가능";
                    break;

                case NoteItem.ITEMTYPE.TYPE_NOTE:
                    info.text = "아이템 ‘동료의 수첩’을 얻었다.\n[ESC]로 사용 가능";
                    break;
            }

            ui.GetComponent<UIPopup>().Initialize_UI(UIPopup.TYPE.T_EXPENDABLES, info, m_itemType);

            Destroy(gameObject);
        }
    }
}
