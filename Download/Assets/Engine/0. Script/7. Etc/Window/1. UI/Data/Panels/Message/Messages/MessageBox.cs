using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MessageBox : MonoBehaviour, IPointerClickHandler
{
    private MessageList m_owner;

    public void Set_Owner(MessageList owner)
    {
        m_owner = owner;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_owner == null)
            return;

        m_owner.Set_Click(true);

        GameManager.Ins.Window.CHATTING.Set_ChattingsData(m_owner.Chattings);
        GameManager.Ins.Window.CHATTING.Active_ChildPopup(true);
    }
}
