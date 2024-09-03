using UnityEngine;

namespace VisualNovel
{
    public class HallwayCD : MonoBehaviour
    {
        private int m_positionIndex = -1;
        public int PositionIndex
        {
            get => m_positionIndex;
            set => m_positionIndex = value;
        }

        private float m_floatSpeed = 1f;
        private float m_height = 0.2f;
        private float m_startY = 0f;

        private Quaternion m_initialRotation;

        private void Start()
        {
            m_startY = 0.5f + m_height;
            m_initialRotation = transform.rotation;
        }

        private void Update()
        {
            float newY = m_startY + Mathf.Sin(Time.time * m_floatSpeed) * m_height;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            Vector3 directionToCamera = Camera.main.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);
            transform.rotation = lookRotation * m_initialRotation;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().Get_CD(m_positionIndex);
                Destroy(gameObject);
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 10.0f);
#endif
        }
    }
}

