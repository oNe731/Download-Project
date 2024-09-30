using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorLight : MonoBehaviour
{
    [SerializeField] private bool m_active;
    [SerializeField] private bool m_timefinish;
    [SerializeField] private float m_minChange;
    [SerializeField] private float m_maxChange;
    [SerializeField] private float m_eventMaxTime;

    private bool m_start = false;
    private float m_change = 0f;
    private float m_eventTime = 0f;
    private float m_time = 0f;

    private Light m_light;

    public Light Light => m_light;

    private void Start()
    {
        m_light = GetComponent<Light>();
    }

    public void Start_Blink(bool active, float minChange, float maxChange, bool timefinish = false, float eventMaxTime = 0f)
    {
        gameObject.SetActive(true);

        m_start = true;

        m_active = active;
        m_minChange = minChange;
        m_maxChange = maxChange;
        m_timefinish = timefinish;
        m_eventMaxTime = eventMaxTime;

        m_eventTime = 0f;
        m_time = 0f;
        m_change = Random.Range(m_minChange, m_maxChange);
    }

    private void Update()
    {
        if (m_start == false)
            return;

        m_eventTime += Time.deltaTime;
        if(m_timefinish == true && m_eventTime >= m_eventMaxTime)
        {
            Finish_Event();
        }
        else
        {
            m_time += Time.deltaTime;
            if (m_time >= m_change)
            {
                m_time = 0f;
                m_change = Random.Range(m_minChange, m_maxChange);

                m_light.enabled = !m_light.enabled;
            }
        }
    }

    public void Finish_Event()
    {
        m_start = false;
        m_light.enabled = m_active;
    }
}
