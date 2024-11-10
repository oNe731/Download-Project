using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupFrame : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler
{
    [SerializeField] private Transform m_parent;

    private Vector2 m_offset;
    private RectTransform m_rectTransform;
    private Panel_Popup m_panel = null;

    private void Awake()
    {
        m_rectTransform = m_parent.GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_panel != null && m_panel.InputPopupButton == false)
            return;

        Sort_Popup();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_panel != null && m_panel.InputPopupButton == false)
            return;

        Sort_Popup();

        // 오프셋 계산
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, eventData.position, eventData.pressEventCamera, out m_offset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_panel != null && m_panel.InputPopupButton == false)
            return;

        // 창 이동
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
            m_rectTransform.localPosition = localPointerPosition - m_offset;
    }

    private void Sort_Popup() // 레이어 정렬
    {
        Panel_Popup panel = GameManager.Ins.Window.Get_Popup(m_parent.gameObject);
        if (panel != null)
            GameManager.Ins.Window.Sort_PopupIndex(panel.FileType);
    }

    public void Set_OwnerPanel(Panel_Popup panel)
    {
        m_panel = panel;
    }
}
