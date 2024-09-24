using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class HallwayLever : MonoBehaviour
    {
        [SerializeField] private GameObject m_level;
        [SerializeField] private float m_rotationSpeed = 5.0f;

        private int m_positionIndex = -1;

        private bool m_rotation       = false;
        private bool m_rotationFinish = false;
        private float m_waitTime = 2f;
        private float m_time;

        public int PositionIndex
        {
            get => m_positionIndex;
            set => m_positionIndex = value;
        }


        private void Update()
        {
            if (m_rotation)
            {
                if (m_rotationFinish == false && m_level.transform.rotation.eulerAngles.x < 330)
                    m_level.transform.Rotate(0f, -1f * m_rotationSpeed, 0f, Space.Self);
                else
                {
                    m_rotationFinish = true;

                    m_time += Time.deltaTime;
                    if (m_time > m_waitTime)
                    {
                        GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Chase>().Use_Lever(m_positionIndex);
                        Destroy(gameObject);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_rotation == true || m_rotationFinish == true)
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
}


