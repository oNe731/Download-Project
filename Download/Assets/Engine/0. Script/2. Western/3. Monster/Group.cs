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
            m_grouptransform    = GetComponent<Transform>();
            m_wakeUpQuaternion  = Quaternion.Euler(new Vector3(0f, 0f, 0f));
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

        public void WakeUp_Group(bool useEvent, bool isCount, float timerSpeed)
        {
            StartCoroutine(WakeUp(useEvent, isCount, timerSpeed));
        }

        public void LayDown_Group(bool nextMove)
        {
            StartCoroutine(LayDown(nextMove));
        }

        private IEnumerator WakeUp(bool useEvent, bool isCount, float timerSpeed)
        {
            for (int i = 0; i < m_person.Length; ++i)
                m_person[i].SetActive(true);

            bool  isEvent = false;
            float time    = 0f;
            while (m_grouptransform.rotation != m_wakeUpQuaternion)
            {
                time += Time.deltaTime;
                if(isEvent == false && time > 0.8f)
                {
                    isEvent = true;
                    if (useEvent == true)
                        Use_Event();

                    if (isCount) // 카운트 시작
                        Start_Count(timerSpeed);
                    else
                        WesternManager.Instance.IsShoot = true;
                }

                m_grouptransform.rotation = Quaternion.Slerp(m_grouptransform.rotation, m_wakeUpQuaternion, m_wakeUpRotationSpeed * Time.deltaTime);
                yield return null;
            }

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

        private void Use_Event()
        {
            switch (WesternManager.Instance.LevelController.Curlevel)
            {
                // Temp -----------------------------------------
                case (int)WesternManager.LEVELSTATE.LS_PlayLv1:
                    StartCoroutine(Create_Bomb());
                    break;
                // Temp -----------------------------------------

                case (int)WesternManager.LEVELSTATE.LS_PlayLv2:
                    StartCoroutine(Create_Bomb());
                    break;
            }
        }

        private IEnumerator Create_Bomb()
        {
            int createCount = Random.Range(1,3); // 1, 2개 생성
            int count = 0;
            int dir = Random.Range(0, 2); // 0, 1

            GameObject bombPrefab = Resources.Load<GameObject>("5. Prefab/2. Western/Common/Bomb");
            Vector3 leftSpawnPosition  = transform.position + new Vector3(-3f, 0.8f, -0.1f);
            Vector3 rightSpawnPosition = transform.position + new Vector3(3f, 0.8f, -0.1f);

            GameObject secondBomb = null;
            if(createCount == 2)
            {
                secondBomb = Instantiate(bombPrefab, Vector3.zero, Quaternion.identity);
                secondBomb.SetActive(false);
            }

            while (count < createCount)
            {
                if (count == 0) // 첫 번째 생성
                {
                    GameObject firstBomb = Instantiate(bombPrefab, Vector3.zero, Quaternion.identity);
                    if (dir == 0) // 왼쪽에 생성
                        firstBomb.transform.localPosition = leftSpawnPosition;
                    else if (dir == 1) // 오른쪽에 생성
                        firstBomb.transform.localPosition = rightSpawnPosition;

                    Bomb script = firstBomb.GetComponent<Bomb>();
                    script.TargetPosition = transform.position;
                    script.TimerMax = 2f;

                    script.DifferentBomb = secondBomb;
                }
                else if (count == 1) // 두 번째 생성
                {
                    yield return new WaitForSeconds(2f);

                    secondBomb.SetActive(true);
                    if (dir == 0) // 오른쪽에 생성
                        secondBomb.transform.localPosition = rightSpawnPosition;
                    else if (dir == 1) // 왼쪽에 생성
                        secondBomb.transform.localPosition = leftSpawnPosition;

                    Bomb script = secondBomb.GetComponent<Bomb>();
                    script.TargetPosition = transform.position;
                    script.TimerMax = 3f;
                }

                count++;
                yield return null;
            }

            yield break;
        }
    }
}

