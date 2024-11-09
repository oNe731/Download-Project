using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupScrollBar : MonoBehaviour
{
    private Scrollbar m_scrollBar;
    private float m_value = 0.1f;

    private void Start()
    {
        m_scrollBar = GetComponent<Scrollbar>();
    }

    public void Move_Scroll(float direction)
    {
        float newValue = m_scrollBar.value + (m_value * direction);
        newValue = Mathf.Clamp(newValue, 0f, 1f);    // �� ���� ����
        newValue = Mathf.Round(newValue * 10) / 10f; // �Ҽ��� �ݿø�

        m_scrollBar.value = newValue;
    }
}
