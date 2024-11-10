using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPlayer : MonoBehaviour
{
    private float m_moveSpeed = 4f;   // 이동 속도
    private float m_bounceSpeed = 7f;     // 상하 속도
    private float m_bounceHeight = 0.01f; // 위아래 움직임 높이
    public float  m_returnSpeed = 15f;    // 기본 높이로 복구하는 속도

    private float m_startHeight;
    private float m_bounceTimer = 0f;
    private float m_currentHeight;

    private void Start()
    {
        m_startHeight = transform.position.y;
        m_currentHeight = m_startHeight;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // 전진
            transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime);

            // 상하
            m_bounceTimer += m_bounceSpeed * Time.deltaTime;
            float bounceOffset = Mathf.Sin(m_bounceTimer) * m_bounceHeight;
            m_currentHeight = m_startHeight + bounceOffset;
        }
        else
        {
            // 높이 복구
            m_currentHeight = Mathf.Lerp(m_currentHeight, m_startHeight, m_returnSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, m_currentHeight, transform.position.z);
    }
}
