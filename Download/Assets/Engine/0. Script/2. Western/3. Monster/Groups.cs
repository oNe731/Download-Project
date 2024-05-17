using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Groups : MonoBehaviour
    {
        [SerializeField] private Group[] m_groups;

        private int m_currentIndex = -1;

        private void Start()
        {
        }

        public void WakeUp_Next(bool isCount = true, float timerSpeed = 1f)
        {
            m_currentIndex++;
            if (m_currentIndex >= m_groups.Length - 1)
            {
                WesternManager.Instance.LevelController.Get_CurrentLevel<Western_Play>().finishGroup = true;
                WesternManager.Instance.LevelController.Get_CurrentLevel<Western_Play>().Play_Finish();
                return;
            }

            m_groups[m_currentIndex].WakeUp_Group(isCount, timerSpeed);
        }

        public void LayDown_Group(bool nextMove = false)
        {
            m_groups[m_currentIndex].LayDown_Group(nextMove);
        }

        public Vector3 Next_Position()
        {
            int nextIndex = m_currentIndex + 1;
            if (nextIndex >= m_groups.Length)
                return new Vector3();

            Vector3 nextPosition = m_groups[nextIndex].gameObject.transform.position;
            return new Vector3(nextPosition.x, nextPosition.y, nextPosition.z - 1.83f);
        }

        public GameObject Get_Criminal()
        {
            return m_groups[m_currentIndex].Get_Criminal();
        }

        public void Destroy_Timer()
        {
            m_groups[m_currentIndex].Destroy_Timer();
        }
    }
}

