using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClickImage : MonoBehaviour
{
    private bool m_active = false;

    private GameObject m_text;
    private float m_time = 0;
    private float m_waitTime = 3f;

    private void Start()
    {
        m_text = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if(m_active == false)
        {
            m_time += Time.deltaTime;
            if (m_time >= m_waitTime)
            {
                m_time = 0f;
                m_text.SetActive(!m_text.activeSelf);

                if (m_text.activeSelf)
                    m_waitTime = 3f;
                else
                    m_waitTime = 1f;
            }
        }
    }

    public void Button_Click()
    {
        m_active = !m_active;
        transform.GetChild(0).gameObject.SetActive(m_active);

        if(m_active == true)
            m_text.SetActive(true);
    }
}
