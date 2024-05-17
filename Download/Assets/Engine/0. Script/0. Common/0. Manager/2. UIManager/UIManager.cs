using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager m_instance = null;
    public static UIManager Instance
    {
        get
        {
            if (null == m_instance)
                return null;
            return m_instance;
        }
    }

    [SerializeField] private GameObject m_fadeCanvas;
    private Image m_fadeImg; // 페이드에 사용할 이미지

    private bool m_isFade = false;

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);

            m_fadeImg = m_fadeCanvas.GetComponentInChildren<Image>();
            DontDestroyOnLoad(m_fadeCanvas);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Start_FadeIn(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        StartCoroutine(FadeCoroutine(1f, 0f, duration, color, onComplete, waitTime, panalOff));
    }

    public void Start_FadeOut(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        StartCoroutine(FadeCoroutine(0f, 1f, duration, color, onComplete, waitTime, panalOff));
    }

    public void Start_FadeInOut(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        StartCoroutine(FadeCoroutine(1f, 0f, duration, color, () => Start_FadeOut(duration, color, onComplete), waitTime, panalOff));
    }

    public void Start_FadeOutIn(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        StartCoroutine(FadeCoroutine(0f, 1f, duration, color, () => Start_FadeIn(duration, color, onComplete), waitTime, panalOff));
    }

    public void Start_FadeWaitAction(float startAlpha, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        StartCoroutine(FadeWaitAction(startAlpha, color, onComplete, waitTime, panalOff));
    }

    private IEnumerator FadeCoroutine(float startAlpha, float targetAlpha, float duration, Color color, Action onComplete, float waitTime, bool panalOff)
    {
        m_isFade = true;
        m_fadeCanvas.SetActive(true);

        float currentTime = 0f;
        Color startColor  = color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float fadeProgress = currentTime / duration;
            m_fadeImg.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startAlpha, targetAlpha, fadeProgress));

            yield return null;
        }

        m_fadeImg.color = targetColor;
        m_isFade = false;

        if (panalOff)
            m_fadeCanvas.SetActive(false);

        if (onComplete != null)
        {
            yield return new WaitForSeconds(waitTime);
            onComplete?.Invoke(); // 콜백 함수 호출
        }

        yield break;
    }

    private IEnumerator FadeWaitAction(float startAlpha, Color color, Action onComplete, float waitTime, bool panalOff)
    {
        m_isFade = true;
        m_fadeCanvas.SetActive(true);

        m_fadeImg.color = new Color(color.r, color.g, color.b, startAlpha);

        yield return new WaitForSeconds(waitTime);

        m_isFade = false;

        if (panalOff)
            m_fadeCanvas.SetActive(false);

        if (onComplete != null)
            onComplete?.Invoke(); // 콜백 함수 호출

        yield break;
    }
}
