using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayLever : MonoBehaviour
{
    [SerializeField] private GameObject m_level;
    [SerializeField] private float m_rotationSpeed = 5.0f;

    private bool m_rotation  = false;
    private bool m_use       = false;
    private float m_waitTime = 2f;
    private float m_time;

    private void Update()
    {
        if (m_rotation && !m_use)
        {
            if (m_level.transform.rotation.eulerAngles.x < 330)
                m_level.transform.Rotate(0f, -1f * m_rotationSpeed, 0f, Space.Self);
            else
            {
                m_time += Time.deltaTime;
                if (m_time > m_waitTime)
                {
                    VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().Use_Lever(gameObject);
                    m_use = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_rotation || m_use)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            m_rotation = true;
            // 파티클 생성

        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5.0f);
#endif
    }
}
