using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconButton : MonoBehaviour, IPointerClickHandler
{
    private FileIconSlot m_owner;

    public void Set_Owner(FileIconSlot owner)
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
