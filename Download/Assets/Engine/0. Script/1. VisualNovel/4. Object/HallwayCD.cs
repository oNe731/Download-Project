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

