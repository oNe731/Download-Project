using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayLight : MonoBehaviour
{
    private new Light m_light;
    private float m_changeTime;
    private float m_time;

    private void Start()
    {
        m_light = GetComponent<Light>();
        m_changeTime = Random.Range(1.0f, 3.0f);
    }

    private void Update()
    {
        m_time += Time.deltaTime;
        if (m_time > m_changeTime)
        {
            m_time = 0.0f;
            m_changeTime = Random.Range(0.5f, 2.0f);
            m_light.enabled = !m_light.enabled;
        }
    }
}
