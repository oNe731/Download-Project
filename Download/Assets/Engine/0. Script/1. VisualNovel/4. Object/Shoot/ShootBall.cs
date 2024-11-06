using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class ShootBall : MonoBehaviour
    {
        private ShootSlingshot m_Owner;
        private GameObject m_targetUI;

        private Vector3 m_startPosition = new Vector3(0f, 0f, 0f);
        private Vector3 m_targetPosition = new Vector3(0f, 0f, 0f);

        private float m_heightArc = 5.0f;
        private float m_speed     = 2.0f;
        private bool  m_arrived   = false;
        private float m_maxY = 0f;

        private SphereCollider m_collider;

        public void Initialize_Ball(ShootSlingshot owner, GameObject targetUI, Vector3 targetPosition, float speed)
        {
            m_Owner          = owner;
            m_targetUI       = targetUI;
            m_targetPosition = targetPosition;
            m_speed          = speed;

            m_startPosition = transform.position;
            m_maxY = transform.position.y;

            m_collider = GetComponent<SphereCollider>();
            m_collider.enabled = false;

            // 각도에 따른 속도 조절
            float angle = Vector3.Angle((m_startPosition - m_targetPosition).normalized, Vector3.up);
            if (angle < 90)
                angle = 90 + (90 - angle);
            float result = (90 - Mathf.Abs(angle - 90));
            m_speed *= result;
        }

        private void Update()
        {
            if (GameManager.Ins.IsGame == false)
                return;

            float nextX = Mathf.MoveTowards(transform.position.x, m_targetPosition.x, m_speed * Time.deltaTime);

            float distance = m_targetPosition.x - m_startPosition.x;
            float baseY = Mathf.Lerp(m_startPosition.y, m_targetPosition.y, (nextX - m_startPosition.x) / distance);
            float arc = m_heightArc * (nextX - m_startPosition.x) * (nextX - m_targetPosition.x) / (-0.25f * distance * distance);

            Vector3 nextPosition = new Vector3(nextX, baseY + arc, transform.position.z);
            transform.rotation = LookAt2D(nextPosition - transform.position);
            transform.position = nextPosition;

            if (nextPosition.y > m_maxY) { m_maxY = nextPosition.y; } // 가는 길에 충돌 처리 되는 버그 수정
            else if (!m_collider.enabled && nextPosition.y < m_maxY && Vector3.Distance(nextPosition, m_targetPosition) <= 0.5f) { m_collider.enabled = true; }
            else if (m_collider.enabled == true && !m_arrived)
            {
                if (nextPosition == m_targetPosition)
                    Arrived();
            }
        }

        private void LateUpdate()
        {
            // 게임 종료 시 존재하는 공 삭제
            if (GameManager.Ins.Novel.LevelController.Get_Level<Novel_Day3Shoot>((int)VisualNovelManager.LEVELSTATE.LS_DAY3SHOOTGAME).ShootGameOver)
            {
                Destroy(m_targetUI);
                Destroy(gameObject);
            }
        }

        private Quaternion LookAt2D(Vector2 forward)
        {
            return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
        }

        public void Arrived()
        {
            m_arrived = true;
            if (m_Owner != null)
                m_Owner.Use = true;
            Destroy(m_targetUI);
            Destroy(gameObject);
        }
    }
}

