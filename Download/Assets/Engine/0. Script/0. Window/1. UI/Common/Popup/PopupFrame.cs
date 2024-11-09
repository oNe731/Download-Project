using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupFrame : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler
{
    [SerializeField] private Transform m_parent;

    private Vector2 m_offset;
    private RectTransform m_rectTransform;

    private void Awake()
    {
        m_rectTransform = m_parent.GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Sort_Popup();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Sort_Popup();

        // ������ ���
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, eventData.position, eventData.pressEventCamera, out m_offset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // â �̵�
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
            m_rectTransform.localPosition = localPointerPosition - m_offset;
    }

    private void Sort_Popup() // ���̾� ����
    {
        Panel_Popup panel = GameManager.Ins.Window.Get_Popup(m_parent.gameObject);
        if (panel != null)
            GameManager.Ins.Window.Sort_PopupIndex(panel.FileType);
    }
}
