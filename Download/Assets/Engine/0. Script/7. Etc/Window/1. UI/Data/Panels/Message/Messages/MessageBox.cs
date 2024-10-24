using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MessageBox : MonoBehaviour, IPointerClickHandler
{
    private List<ChattingData> m_chattings;

    public void Set_ChattingsData(List<ChattingData> chattings)
    {
        m_chattings = chattings;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Ins.Window.CHATTING.Active_ChildPopup(true);
        GameManager.Ins.Window.CHATTING.Set_ChattingsData(m_chattings);
    }
}
