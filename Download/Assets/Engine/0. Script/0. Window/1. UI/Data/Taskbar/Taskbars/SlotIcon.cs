using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotIcon : MonoBehaviour, IPointerClickHandler
{
    private IconSlot m_slot = null;

    public void Initialize_Icon(IconSlot slot)
    {
        m_slot = slot;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_slot == null)
            return;

        m_slot.Click_Icon();
    }
}
