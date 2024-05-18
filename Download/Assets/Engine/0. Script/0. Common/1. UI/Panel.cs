using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    private RectTransform m_rectTransform = null;
    private Coroutine m_shakeCoroutine = null;
    private Vector3 m_StartPosition;

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_StartPosition = m_rectTransform.anchoredPosition;
    }

    public void Start_Shake(float ShakeAmount, float ShakeTime)
    {
        if (m_shakeCoroutine != null)
            StopCoroutine(m_shakeCoroutine);
        m_shakeCoroutine = StartCoroutine(Shake(ShakeAmount, ShakeTime));
    }

    private IEnumerator Shake(float ShakeAmount, float ShakeTime)
    {
        float timer = 0;
        while (timer <= ShakeTime)
        {
            timer += Time.deltaTime;

            Vector3 randomPoint = m_StartPosition + Random.insideUnitSphere * ShakeAmount;
            m_rectTransform.anchoredPosition = Vector3.Lerp(m_StartPosition, randomPoint, Time.deltaTime);
            yield return null;
        }

        m_rectTransform.anchoredPosition = m_StartPosition;
        yield break;
    }
}
