using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class Interaction_Research_Item : Interaction
    {
        [SerializeField] private NoteItem m_noteItem;
        
        private void Start()
        {
            GameObject gameObject = HorrorManager.Instance.Create_WorldHintUI(UIWorldHint.HINTTYPE.HT_RESEARCH, transform.GetChild(0), m_uiOffset);
            m_interactionUI = gameObject.GetComponent<UIWorldHint>();
        }

        private void Update()
        {
            //Update_InteractionUI();
        }

        public override void Click_Interaction()
        {
            if (m_interactionUI.gameObject.activeSelf == false || m_interact == true)
                return;

            GameObject ui = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Popup", GameObject.Find("Canvas").transform.GetChild(2));
            if (ui == null)
                return;

            if (m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_PIPE || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_GUN || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_FLASHLIGHT || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_NOTE)
            {
                UIPopup.ItemInfo item = new UIPopup.ItemInfo();
                item.type = m_noteItem.m_itemType;

                ui.GetComponent<UIPopup>().Initialize_UI(UIPopup.TYPE.T_GETITEM, item, m_noteItem);
            }
            else if (m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_BULLET || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_DRUG)
            {
                Note playerNote = HorrorManager.Instance.Player.Note;
                if (playerNote == null)
                {
                    Destroy(ui);
                    return;
                }

                // 수첩 아이템에 추가
                playerNote.Add_Item(m_noteItem);

                // UI 생성
                UIPopup.Expendables info = new UIPopup.Expendables();
                info.text = m_noteItem.m_name + "을 획득했다.";

                ui.GetComponent<UIPopup>().Initialize_UI(UIPopup.TYPE.T_EXPENDABLES, info, m_noteItem);
            }
            else // 단서
            {
                // UI 생성
                UIPopup.ItemInfo item = new UIPopup.ItemInfo();
                item.type = m_noteItem.m_itemType;

                ui.GetComponent<UIPopup>().Initialize_UI(UIPopup.TYPE.T_GETITEM, item, m_noteItem);
            }

            Destroy_Interaction();
        }
    }
}
