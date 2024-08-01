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
            m_interactionUI = HorrorManager.Instance.Create_WorldHintUI(UIWorldHint.HINTTYPE.HT_RESEARCH, transform, m_uiOffset);
            m_interactionUI.SetActive(false);
        }

        private void Update()
        {
            Update_InteractionUI();
        }

        public override void Click_Interaction()
        {
            if (m_interactionUI.activeSelf == false || m_interact == true)
                return;

            GameObject ui = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Popup", GameObject.Find("Canvas").transform.GetChild(2));
            if (ui == null)
                return;

            if (m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_PIPE || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_GUN || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_FLASHLIGHT || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_NOTE)
            {
                UIPopup.ItemInfo item = new UIPopup.ItemInfo();
                item.type = m_noteItem.m_itemType;

                ui.GetComponent<UIPopup>().Initialize_UI(UIPopup.TYPE.T_GETITEM, item);
            }
            else if (m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_BULLET || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_DRUG)
            {
                // ��ø �����ۿ� �߰�
                HorrorManager.Instance.Player.Note.Add_Item(m_noteItem);

                // UI ����
                UIPopup.Expendables info = new UIPopup.Expendables();
                info.text = m_noteItem.m_name + "�� ȹ���ߴ�.";

                ui.GetComponent<UIPopup>().Initialize_UI(UIPopup.TYPE.T_EXPENDABLES, info);
            }
            else // �ܼ�
            {
                // ��ø �����ۿ� �߰�
                HorrorManager.Instance.Player.Note.Add_Proviso(m_noteItem);

                // UI ����
                UIPopup.Expendables info = new UIPopup.Expendables();
                info.text = m_noteItem.m_name + "�� ȹ���ߴ�.";

                ui.GetComponent<UIPopup>().Initialize_UI(UIPopup.TYPE.T_EXPENDABLES, info);
            }

            Destroy(m_interactionUI);
            Destroy(gameObject);
        }
    }
}
