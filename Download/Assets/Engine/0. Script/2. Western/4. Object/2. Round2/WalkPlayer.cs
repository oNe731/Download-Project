using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPlayer : MonoBehaviour
{
    private float m_moveSpeed = 4f;   // �̵� �ӵ�
    private float m_bounceSpeed = 7f;     // ���� �ӵ�
    private float m_bounceHeight = 0.01f; // ���Ʒ� ������ ����
    public float  m_returnSpeed = 15f;    // �⺻ ���̷� �����ϴ� �ӵ�

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
            // ����
            transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime);

            // ����
            m_bounceTimer += m_bounceSpeed * Time.deltaTime;
            float bounceOffset = Mathf.Sin(m_bounceTimer) * m_bounceHeight;
            m_currentHeight = m_startHeight + bounceOffset;
        }
        else
        {
            // ���� ����
            m_currentHeight = Mathf.Lerp(m_currentHeight, m_startHeight, m_returnSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, m_currentHeight, transform.position.z);
    }
}
