using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRoulette : MonoBehaviour
{
    [SerializeField] private GameRouletteText m_gameRouletteText;
    [SerializeField] private Image rouletteImage; // UI Image
    [SerializeField] private Color targetColor = new Color(0.25f, 0.21f, 0.34f, 0f); // ��ǥ ����
    private Color startColor;

    private float m_time = 0f;
    private float m_rotationTiem = 2f;
    private float m_rotationSpeed = 500f;
    private RectTransform m_rectTransform;

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        startColor = rouletteImage.color; // ���� ���� ����
    }

    private void Update()
    {
        if (m_rotationSpeed > 0f)
        {
            // ȸ�� ó��
            Vector3 currentRotation = m_rectTransform.localEulerAngles;
            currentRotation.z += m_rotationSpeed * Time.deltaTime;
            m_rectTransform.localEulerAngles = currentRotation;
            StartCoroutine(ChangeColorOverTime(1f)); // 1�� ���� ���� ����
            // ȸ�� �ӵ� ����
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

        while (true) // ���� ������ ����Ͽ� �ݺ�
        {
            float elapsedTime = 0f;

            // 1�� ���� startColor���� targetColor�� ����
            while (elapsedTime < halfDuration)
            {
                rouletteImage.color = Color.Lerp(startColor, targetColor, elapsedTime / halfDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // ��Ȯ�ϰ� targetColor�� ����
            rouletteImage.color = targetColor;
            elapsedTime = 0f;

            // 1�� ���� targetColor���� startColor�� �ٽ� ����
            while (elapsedTime < halfDuration)
            {
                rouletteImage.color = Color.Lerp(targetColor, startColor, elapsedTime / halfDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // ��Ȯ�ϰ� startColor�� ����
            rouletteImage.color = startColor;
        }
    }
}