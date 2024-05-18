using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Group : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_person;
        private Groups m_groups = null;
        private GameObject m_timer = null;

        private int m_groupIndex;
        private int m_criminalIndex;

        private Transform m_grouptransform;
        private Quaternion m_wakeUpQuaternion;
        private Quaternion m_layDownQuaternion;

        private float m_wakeUpRotationSpeed = 3f;
        private float m_layDownRotationSpeed = 9f;

        private void Start()
        {
            m_grouptransform = GetComponent<Transform>();
            m_wakeUpQuaternion = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            m_layDownQuaternion = Quaternion.Euler(new Vector3(90f, 0f, 0f));
        }

        public void Initialize(int groupIndex, Groups groups, int roundIndex)
        {
            m_groupIndex = groupIndex;
            m_groups = groups;

            int criminal = 0;
            int citizen = 0;
            m_criminalIndex = Random.Range(0, 3);
            for (int i = 0; i < m_person.Length; ++i)
            {
                if (i == m_criminalIndex)
                {
                    m_person[i].AddComponent<Criminal>();
                    m_person[i].GetComponent<Criminal>().Initialize(m_groupIndex, criminal, m_groups, roundIndex);
                    criminal++;
                }
                else
                {
                    m_person[i].AddComponent<Citizen>();
                    m_person[i].GetComponent<Citizen>().Initialize(m_groupIndex, citizen, m_groups, roundIndex);
                    citizen++;
                }
            }
        }

        private void Update()
        {
        }

        public void WakeUp_Group(bool isCount, float timerSpeed)
        {
            StartCoroutine(WakeUp(isCount, timerSpeed));
        }

        public void LayDown_Group(bool nextMove)
        {
            StartCoroutine(LayDown(nextMove));
        }

        private IEnumerator WakeUp(bool isCount, float timerSpeed)
        {
            for (int i = 0; i < m_person.Length; ++i)
                m_person[i].SetActive(true);

            while (m_grouptransform.rotation != m_wakeUpQuaternion)
            {
                m_grouptransform.rotation = Quaternion.Slerp(m_grouptransform.rotation, m_wakeUpQuaternion, m_wakeUpRotationSpeed * Time.deltaTime);
                yield return null;
            }

            if (isCount) // 카운트 시작
                Start_Count(timerSpeed);
            else
                WesternManager.Instance.IsShoot = true;

            yield break;
        }

        private IEnumerator LayDown(bool nextMove)
        {
            while (m_grouptransform.rotation != m_layDownQuaternion)
            {
                m_grouptransform.rotation = Quaternion.Slerp(m_grouptransform.rotation, m_layDownQuaternion, m_layDownRotationSpeed * Time.deltaTime);
                yield return null;
            }

            for (int i = 0; i < m_person.Length; ++i)
            {
                if(m_person[i] != null)
                    m_person[i].SetActive(false);
            }
                

            // 자식 오브젝트 삭제
            Transform[] children = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
                children[i] = transform.GetChild(i);
            foreach (Transform child in children)
                Destroy(child.gameObject);

            // 자동 전진
            if (nextMove == true)
                WesternManager.Instance.LevelController.Get_CurrentLevel<Western_Play>().Proceed_Next();

            yield break;
        }

        private void Start_Count(float timerSpeed)
        {
            Destroy_Timer();

            m_timer = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/UI_Timer"), Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
            RectTransform timerTransform = m_timer.GetComponent<RectTransform>();
            timerTransform.anchoredPosition = new Vector2(0f, 250f);

            Timer timer = m_timer.GetComponent<Timer>();
            timer.Start_Timer(timerSpeed);
        }

        public GameObject Get_Criminal()
        {
            return m_person[m_criminalIndex];
        }

        public GameObject Get_Citizen(int index)
        {
            for(int i = 0; i < m_person.Length; ++i)
            {
                if(i != m_criminalIndex) // 범인이 아니고 시민일 때
                {
                    if(m_person[i].GetComponent<Person>().PersonIndex == index) // 찾는 시민일 때
                    {
                        return m_person[i];
                    }
                }
            }

            return null;
        }

        public void Destroy_Timer()
        {
            if (m_timer != null)
                Destroy(m_timer);
        }
    }
}

