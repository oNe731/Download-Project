using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInstruction : MonoBehaviour
{
    public enum ACTIVETYPE { TYPE_BASIC, TYPE_FADE, TYPE_END }

    [SerializeField] private TMP_Text m_txt;

    private bool  m_active = false;
    private bool  m_finish = false;
    private int   m_index = 0;
    private float m_time = 0f;
    private float[]  m_activeTimes;
    private string[] m_texts;
    private ACTIVETYPE m_closeType = ACTIVETYPE.TYPE_END;

    private Coroutine m_fadeCoroutine = null;

    public void Initialize_UI(ACTIVETYPE openType, ACTIVETYPE closeType, float[] activeTimes, string[] texts)
    {
        m_closeType  = closeType;

        m_activeTimes = activeTimes;
        m_texts = texts;

        m_active = false;
        m_finish = false;
        m_index = 0;
        Update_IndexInfo();

        gameObject.SetActive(true);
        switch (openType)
        {
            case ACTIVETYPE.TYPE_BASIC:
                m_txt.color = new Color(m_txt.color.r, m_txt.color.g, m_txt.color.b, 1f);
                m_active = true;
                break;
            case ACTIVETYPE.TYPE_FADE:
                if (m_fadeCoroutine != null)
                    StopCoroutine(m_fadeCoroutine);
                m_fadeCoroutine = StartCoroutine(Fade_Color(0f, 1f, 1f));
                break;
        }
    }

    private void Update()
    {
        if (m_active == false || m_finish == true)
            return;

        m_time += Time.deltaTime;
        if (m_time >= m_activeTimes[m_index])
        {
            m_index++;
            if(m_index == m_activeTimes.Length)
            {
                m_finish = true;
                switch (m_closeType)
                {
                    case ACTIVETYPE.TYPE_BASIC:
                        gameObject.SetActive(false);
                        break;
                    case ACTIVETYPE.TYPE_FADE:
                        if (m_fadeCoroutine != null)
                            StopCoroutine(m_fadeCoroutine);
                        m_fadeCoroutine = StartCoroutine(Fade_Color(1f, 0f, 1f));
                        break;
                }
                return;
            }

            Update_IndexInfo();
        } 
    }

    private void Update_IndexInfo()
    {
        m_time = 0;
        m_txt.text = m_texts[m_index];
    }

    private IEnumerator Fade_Color(float startAlpha, float targetAlpha, float duration)
    {
        m_txt.color = new Color(m_txt.color.r, m_txt.color.g, m_txt.color.b, startAlpha);

        Color startColor = m_txt.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        float currentTime = 0f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float fadeProgress = currentTime / duration;
            m_txt.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startAlpha, targetAlpha, fadeProgress));

            yield return null;
        }
        m_txt.color = targetColor;

        if(targetAlpha == 0f)
            gameObject.SetActive(false);
        else
            m_active = true;

        yield break;
    }
}
