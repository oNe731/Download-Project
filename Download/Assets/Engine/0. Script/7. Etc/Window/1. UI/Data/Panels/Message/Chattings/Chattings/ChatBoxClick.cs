using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChatBoxClick : MonoBehaviour, IPointerClickHandler
{
    private ChatBox m_owner;
    
    public void Set_ChatBox(ChatBox owner)
    {
        m_owner = owner;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_owner == null)
            return;

        m_owner.OnPointerClick();
    }
}
