using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CallBox : MonoBehaviour, IPointerClickHandler
{
    private CallList m_owner;

    public void Set_Owner(CallList owner)
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
