using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class Interaction_Item : Interaction
    {
        [SerializeField] private NoteItem m_noteItem;

        [SerializeField] private bool     m_closeText;
        [SerializeField] private float[]  m_activeTimes;
        [SerializeField] private string[] m_texts;

        [SerializeField] private bool          m_closeEvent;
        [SerializeField] private UIPopup.EVENT m_eventType;

        public NoteItem NoteItem { set => m_noteItem = value; }

        private void Start()
        {
            GameObject gameObject = GameManager.Ins.Horror.Create_WorldHintUI(UIWorldHint.HINTTYPE.HT_RESEARCH, transform.GetChild(0), m_uiOffset);
            m_interactionUI = gameObject.GetComponent<UIWorldHint>();
        }

        private void Update()
        {
            //Update_InteractionUI();
        }

        public override void Click_Interaction()
        {
            if (No_Click())
                return;

            UIPopup.TYPE type = UIPopup.TYPE.T_END;
            if (m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_END) // 퀘스트 조합 아이템 (가져가기/ 두고가기)
                type = UIPopup.TYPE.T_QUESTITEM;
            else // 노트, 장비, 소모품 아이템, 단서 (가져가기)
            {
                if (m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_NOTE) // 노트
                    type = UIPopup.TYPE.T_NOTE;
                else if (m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_PIPE || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_GUN || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_FLASHLIGHT) // 장비
                    type = UIPopup.TYPE.T_WEAPON;
                else
                {
                    Note playerNote = GameManager.Ins.Horror.Player.Note; // 노트 보유 여부 검사
                    if (playerNote == null)
                        return;

                    if (m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_BULLET || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_DRUG || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_1FKEY) // 소모품 아이템
                        type = UIPopup.TYPE.T_EXPENITEM;
                    else // 단서
                        type = UIPopup.TYPE.T_CLUE;
                }
            }

            GameManager.Ins.Horror.Active_Popup(type, m_noteItem, m_closeText, m_activeTimes, m_texts, m_closeEvent, m_eventType);

            Check_Delete();
        }
    }
}
