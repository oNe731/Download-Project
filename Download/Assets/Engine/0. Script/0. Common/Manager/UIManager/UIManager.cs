using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CURSORTYPE { CT_ORIGIN, CT_BASIC, CT_NOVELSHOOT, CT_END };

public class UIManager : MonoBehaviour
{
    private GameObject m_fadeCanvas;
    private Image      m_fadeImg;
    private Coroutine  m_fadeCoroutine = null;
    private bool       m_isFade = false;

    private Dictionary<string, Texture2D> m_cursorImage = new Dictionary<string, Texture2D>();

    private void Start()
    {
        m_fadeCanvas = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Common/UICanvas", transform);
        m_fadeImg = m_fadeCanvas.GetComponentInChildren<Image>();

        m_cursorImage.Add("ShootGameCursor", GameManager.Ins.Resource.Load<Texture2D>("1. Graphic/2D/1. VisualNovel/UI/Shoot/UI_VisualNovel_Shoot_Aim_Green"));
    }

    public void Start_FadeIn(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        if (m_fadeCoroutine != null)
            StopCoroutine(m_fadeCoroutine);
        m_fadeCoroutine = StartCoroutine(FadeCoroutine(1f, 0f, duration, color, onComplete, waitTime, panalOff));
    }

    public void Start_FadeOut(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        if (m_fadeCoroutine != null)
            StopCoroutine(m_fadeCoroutine);
        m_fadeCoroutine = StartCoroutine(FadeCoroutine(0f, 1f, duration, color, onComplete, waitTime, panalOff));
    }

    public void Start_FadeInOut(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        if (m_fadeCoroutine != null)
            StopCoroutine(m_fadeCoroutine);
        m_fadeCoroutine = StartCoroutine(FadeCoroutine(1f, 0f, duration, color, () => Start_FadeOut(duration, color, onComplete), waitTime, panalOff));
    }

    public void Start_FadeOutIn(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        if (m_fadeCoroutine != null)
            StopCoroutine(m_fadeCoroutine);
        m_fadeCoroutine = StartCoroutine(FadeCoroutine(0f, 1f, duration, color, () => Start_FadeIn(duration, color, onComplete), waitTime, panalOff));
    }

    public void Start_FadeWaitAction(float startAlpha, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        if (m_fadeCoroutine != null)
            StopCoroutine(m_fadeCoroutine);
        m_fadeCoroutine = StartCoroutine(FadeWaitAction(startAlpha, color, onComplete, waitTime, panalOff));
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


    public void Change_Cursor(CURSORTYPE type)
    {
        Texture2D texture = null;
        Vector2 hotspot = Vector2.zero;
        CursorMode mode = CursorMode.ForceSoftware;

        switch (type)
        {
            case CURSORTYPE.CT_ORIGIN:
                texture = null;
                hotspot = Vector2.zero;
                mode = CursorMode.Auto;
                break;

            case CURSORTYPE.CT_BASIC:
                break;

            case CURSORTYPE.CT_NOVELSHOOT:
                texture = m_cursorImage["ShootGameCursor"];
                hotspot = new Vector2(texture.width / 2, texture.height / 2);
                mode = CursorMode.ForceSoftware;
                break;
        }

        Cursor.SetCursor(texture, hotspot, mode);
    }
}
