using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private ScrollRect m_scrollRect;

    private void Start()
    {
        m_scrollRect = GetComponent<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_scrollRect.StopMovement();
        m_scrollRect.enabled = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_scrollRect.enabled = true;
    }
}
