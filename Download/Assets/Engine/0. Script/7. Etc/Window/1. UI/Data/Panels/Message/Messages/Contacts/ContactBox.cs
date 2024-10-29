using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContactBox : MonoBehaviour, IPointerClickHandler
{
    private ContactList m_owner;

    public void Set_Owner(ContactList owner)
    {
        m_owner = owner;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_owner == null)
            return;

        //m_owner.Set_Click(true);
    }
}
