using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    private TMP_Text m_loadingText;

    private string m_basicString = "Loading";
    private int   m_index = 0;
    private float m_time = 0f;

    private void Start()
    {
        m_loadingText = GetComponent<TMP_Text>();
        m_loadingText.text = m_basicString;
    }

    private void Update()
    {
        if (GameManager.Ins.IsGame == false)
            return;

        m_time += Time.deltaTime;
        if (m_time > 0.3f)
        {
            m_time = 0f;
            if(m_index >= 3)
            {
                m_index = 0;
                m_loadingText.text = m_basicString;
            }
            else
            {
                m_index++;
                m_loadingText.text += ".";
            }
        }
    }
}
