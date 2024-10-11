using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
    [SerializeField] private Slider m_owerSlider;
    [SerializeField] private float m_errorValue; // ¿ÀÂ÷°ª
    [SerializeField] public float m_lerpSpeed;
    private Slider m_slider;

    private void Start()
    {
        m_slider = GetComponent<Slider>();
        m_slider.maxValue = m_owerSlider.maxValue;
    }

    private void LateUpdate()
    {
        if (m_slider.value <= m_owerSlider.value - m_errorValue)
            m_slider.value = m_owerSlider.value - m_errorValue;
        else
            m_slider.value = Mathf.Lerp(m_slider.value, m_owerSlider.value - m_errorValue, Time.deltaTime * m_lerpSpeed);
    }
}
