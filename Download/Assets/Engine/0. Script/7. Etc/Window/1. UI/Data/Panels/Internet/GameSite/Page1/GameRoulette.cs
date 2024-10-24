using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoulette : MonoBehaviour
{
    [SerializeField] private GameRouletteText m_gameRouletteText;

    private float m_time = 0f;
    private float m_rotationTiem = 2f;
    private float m_rotationSpeed = 500f;

    private RectTransform m_rectTransform;

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (m_rotationSpeed <= 0f)
            return;

        Vector3 currentRotation = m_rectTransform.localEulerAngles;
        currentRotation.z += m_rotationSpeed * Time.deltaTime;
        m_rectTransform.localEulerAngles = currentRotation;

        m_time += Time.deltaTime;
        if(m_time >= m_rotationTiem) // 회전 속도 유지
        {
            // 회전 속도 감소
            m_rotationSpeed -= Time.deltaTime * 300f;
            if (m_rotationSpeed < 0f)
            {
                m_rotationSpeed = 0f;
                m_gameRouletteText.Start_Text();
            }
        }
    }
}
