using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AlamBox : MonoBehaviour, IPointerClickHandler
{
    private AlamList m_owner;

    public void Set_Owner(AlamList owner)
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
