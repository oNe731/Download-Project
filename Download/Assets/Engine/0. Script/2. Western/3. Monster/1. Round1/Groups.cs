using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Groups : MonoBehaviour
    {
        [SerializeField] private Group[] m_groups;
        private int m_currentIndex = -1;

        public Group[] Group => m_groups;
        public int CurrentIndex
        {
            get => m_currentIndex; set => m_currentIndex = value;
        }

        private void Start()
        {
            for (int i = 0; i < m_groups.Length; ++i)
                m_groups[i].Initialize(i, this);
        }

        public void WakeUp_Next(ref List<int> eventIndex, bool isCount = true, float timerSpeed = 1f)
        {
            m_currentIndex++;
            if (m_currentIndex >= m_groups.Length - 1)
            {
                GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>().finishGroup = true;
                GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>().Play_Finish();
                return;
            }

            bool useEvent = false;
            if (eventIndex != null && eventIndex.Count > 0 && eventIndex[0] == m_currentIndex)
            {
                useEvent = true;
                eventIndex.RemoveAt(0);
            }
            m_groups[m_currentIndex].WakeUp_Group(useEvent, isCount, timerSpeed);
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

        public Vector3 Start_Position(int startIndex = 0)
        {
            Vector3 currentPosition = m_groups[startIndex].gameObject.transform.position;
            return new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - 1.83f);
        }

        public GameObject Get_Criminal()
        {
            return m_groups[m_currentIndex].Get_Criminal();
        }

        public void Destroy_Timer()
        {
            Western_PlayLv1 level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>();
            if (level != null)
                level.Destroy_Timer();
        }

        public bool Check_ElementCriminal(int groupIndex, Person.ElementType element)
        {
            switch(GameManager.Ins.Western.LevelController.Curlevel)
            {
                case (int)WesternManager.LEVELSTATE.LS_PlayLv1:
                    Person.ElementType1 element1 = (Person.ElementType1)element;
                    if (groupIndex > 0)
                    {
                        Person.ElementType1 beforeElement = (Person.ElementType1)m_groups[groupIndex - 1].Get_Criminal().GetComponent<Criminal>().Element;
                        if (element1.scarf == beforeElement.scarf)
                            return true;

                        if (groupIndex > 1)
                        {
                            beforeElement = (Person.ElementType1)m_groups[groupIndex - 2].Get_Criminal().GetComponent<Criminal>().Element;
                            if (element1.scarf == beforeElement.scarf)
                                return true;
                        }
                    }
                    break;

                case (int)WesternManager.LEVELSTATE.LS_PlayLv2:
                    break;
            }

            return false;
        }

        public bool Check_ElementCitizen(int groupIndex, int personIndex, Person.ElementType element)
        {
            // 같은 라인 내에서 중복 금지
            switch (GameManager.Ins.Western.LevelController.Curlevel)
            {
                case (int)WesternManager.LEVELSTATE.LS_PlayLv1:
                    if(personIndex == 1) // 2개 이상 같은 요소가 있으면 다시 조합
                    {
                        int sameIndex = 0;

                        Person.ElementType1 currentElement = (Person.ElementType1)element;
                        Person.ElementType1 beforElement   = (Person.ElementType1)m_groups[groupIndex].Get_Citizen(0).GetComponent<Citizen>().Element;
                        if (beforElement.blindfold == currentElement.blindfold)
                            sameIndex++;

                        if (beforElement.eye == currentElement.eye)
                            sameIndex++;

                        if (beforElement.scarf == currentElement.scarf)
                            sameIndex++;

                        if (sameIndex >= 2)
                            return true;
                    }
                    break;

                case (int)WesternManager.LEVELSTATE.LS_PlayLv2:
                    break;
            }

            return false;
        }
    
        public Group Get_CurrentGroup()
        {
            return m_groups[m_currentIndex];
        }
    }
}

