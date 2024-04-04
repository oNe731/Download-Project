using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_Choice : MonoBehaviour, IPointerEnterHandler
{
    public int m_buttonIndex;
    public int ButtonIndex
    {
        get { return m_buttonIndex; }
        set { m_buttonIndex = value; }
    }

    public Dialog m_ownerdialog;
    public Dialog Ownerdialog
    {
        set { m_ownerdialog = value; }
    }

    // 커서와 충돌했을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        m_ownerdialog.Enter_Button(m_buttonIndex);
    }
}
