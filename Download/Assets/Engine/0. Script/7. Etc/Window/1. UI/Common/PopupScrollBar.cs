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
        newValue = Mathf.Clamp(newValue, 0f, 1f);    // 값 범위 제한
        newValue = Mathf.Round(newValue * 10) / 10f; // 소수점 반올림

        //Debug.Log($"Old Value: {m_scrollBar.value}, New Value: {newValue}");

        m_scrollBar.value = newValue;
    }
}
