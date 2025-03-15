using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRoulette : MonoBehaviour
{
    [SerializeField] private GameRouletteText m_gameRouletteText;
    [SerializeField] private Image rouletteImage; // UI Image
    [SerializeField] private Color targetColor = new Color(0.25f, 0.21f, 0.34f, 0f); // 목표 색상
    private Color startColor;

    private float m_time = 0f;
    private float m_rotationTiem = 2f;
    private float m_rotationSpeed = 500f;
    private RectTransform m_rectTransform;

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        startColor = rouletteImage.color; // 시작 색상 저장
    }

    private void Update()
    {
        if (m_rotationSpeed > 0f)
        {
            // 회전 처리
            Vector3 currentRotation = m_rectTransform.localEulerAngles;
            currentRotation.z += m_rotationSpeed * Time.deltaTime;
            m_rectTransform.localEulerAngles = currentRotation;
            StartCoroutine(ChangeColorOverTime(1f)); // 1초 동안 색상 변경
            // 회전 속도 감소
            m_time += Time.deltaTime;
            if (m_time >= m_rotationTiem)
            {
                
                m_rotationSpeed -= Time.deltaTime * 300f;
                if (m_rotationSpeed <= 0f)
                {
                    m_rotationSpeed = 0f;
                    m_gameRouletteText.Start_Text();
                }
            }
        }
    }

    private IEnumerator ChangeColorOverTime(float duration)
    {
        float halfDuration = duration / 2f;

        while (true) // 무한 루프를 사용하여 반복
        {
            float elapsedTime = 0f;

            // 1초 동안 startColor에서 targetColor로 변경
            while (elapsedTime < halfDuration)
            {
                rouletteImage.color = Color.Lerp(startColor, targetColor, elapsedTime / halfDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 정확하게 targetColor로 설정
            rouletteImage.color = targetColor;
            elapsedTime = 0f;

            // 1초 동안 targetColor에서 startColor로 다시 변경
            while (elapsedTime < halfDuration)
            {
                rouletteImage.color = Color.Lerp(targetColor, startColor, elapsedTime / halfDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 정확하게 startColor로 설정
            rouletteImage.color = startColor;
        }
    }
}