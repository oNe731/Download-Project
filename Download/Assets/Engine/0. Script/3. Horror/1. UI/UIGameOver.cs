using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private Image m_redImage;
    [SerializeField] private Image m_blackImage;
    [SerializeField] private GameObject m_Details;

    private Coroutine m_redCoroutine = null;
    private Coroutine m_blackCoroutine = null;
    //private Coroutine m_retryCoroutine = null;

    public void Start_GameOver()
    {
        m_redImage.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0f);
        m_blackImage.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0f);
        m_Details.SetActive(false);

        if (m_redCoroutine != null)
            StopCoroutine(m_redCoroutine);
        m_redCoroutine = StartCoroutine(RedFadeCoroutine(0f, 1f, 2f));
    }

    private IEnumerator RedFadeCoroutine(float startAlpha, float targetAlpha, float duration)
    {
        float currentTime = 0f;
        Color startColor = m_redImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        bool black = false;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float fadeProgress = currentTime / duration;
            m_redImage.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startAlpha, targetAlpha, fadeProgress));

            if(black == false && m_redImage.color.a >= 0.4f)
            {
                black = true;
                if (m_blackCoroutine != null)
                    StopCoroutine(m_blackCoroutine);
                m_blackCoroutine = StartCoroutine(BlackFadeCoroutine(0f, 1f, 1.5f));
            }

            yield return null;
        }

        m_redImage.color = targetColor;
        yield break;
    }

    private IEnumerator BlackFadeCoroutine(float startAlpha, float targetAlpha, float duration)
    {
        float currentTime = 0f;
        Color startColor = m_blackImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float fadeProgress = currentTime / duration;
            m_blackImage.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startAlpha, targetAlpha, fadeProgress));

            yield return null;
        }

        m_blackImage.color = targetColor;
        m_Details.SetActive(true);
        GameManager.Ins.Camera.Set_CursorLock(false);

        //if (m_retryCoroutine != null)
        //    StopCoroutine(m_retryCoroutine);
        //m_retryCoroutine = StartCoroutine(RetryCoroutine(1f));
        yield break;
    }

    private IEnumerator RetryCoroutine(float duration)
    {
        float currentTime = 0f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Restart_Game(), 0.5f, false);
        yield break;
    }

    public void Yes_Button()
    {
        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Restart_Game(), 0.5f, false);
    }

    private void Restart_Game()
    {
        Destroy(gameObject);
        GameManager.Ins.Horror.Restart_Game();
    }

    public void No_Button()
    {
        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WINDOW), 1f, false);
    }
}
