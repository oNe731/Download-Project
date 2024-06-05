using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class HeartUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] m_panels;
        private Animator[]   m_panelsAnimator;
        private Coroutine    m_shakeCoroutine = null;
        private bool         m_update  = false;
        private int          m_life    = 0;
        private bool         m_laydown = true;

        private float m_time = 0f;
        private float m_deleteTime = 1f;

        private void Start()
        {
            m_panelsAnimator = new Animator[m_panels.Length];
            for (int i = 0; i < m_panels.Length; ++i)
            {
                m_panelsAnimator[i] = m_panels[i].GetComponent<Animator>();
            }
        }

        public void Update()
        {
            if (m_update == false)
                return;

            if (m_panelsAnimator[m_life].GetCurrentAnimatorStateInfo(0).IsName("AM_HeartPanel_Shave") == true)
            {
                if (m_panelsAnimator[m_life].GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) // 애니메이션 종료일 시
                {
                    m_time += Time.deltaTime;
                    if (m_time >= m_deleteTime)
                    {
                        if (m_life > 0)
                        {
                            if (m_laydown == false)
                                return;

                            WesternManager.Instance.LevelController.Get_CurrentLevel<Western_Play>().LayDown_Group(true);
                        }
                        else
                        {
                            Debug.Log("게임 종료");
                            //
                        }

                        m_update = false;
                    }
                }
            }
        }

        public void Reset_Heart()
        {
            m_update = false;

            for (int i = 0; i < m_panels.Length; ++i)
            {
                Transform firstChild = m_panels[i].transform.GetChild(0);
                //firstChild.gameObject.SetActive(true);
                Animator animator = firstChild.gameObject.GetComponent<Animator>();
                animator.SetBool("isShoot", false);
                animator.Play("AM_Heart_Idle", -1, 0f);

                m_panelsAnimator[i].SetBool("isShave", false);
                m_panelsAnimator[i].SetBool("isShoot", false);
                m_panelsAnimator[i].Play("AM_HeartPanel_Idle", -1, 0f);
            }
        }

        public void Start_Update(int lifeCount, bool laydown)
        {
            if (m_update == true)
                return;

            m_update  = true;
            m_life    = lifeCount;
            m_laydown = laydown;

            // 하트 비활성화
            Transform firstChild = m_panels[lifeCount].transform.GetChild(0);
            //firstChild.gameObject.SetActive(false);
            firstChild.gameObject.GetComponent<Animator>().SetBool("isShoot", true);

            // 이펙트 생성
            GameObject effectObject = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/UI_HeartEffect"), GameObject.Find("Canvas").transform);
            effectObject.transform.position = new Vector3(m_panels[lifeCount].transform.position.x - 10f, m_panels[lifeCount].transform.position.y + 20f, m_panels[lifeCount].transform.position.z);

            GameObject particleObject = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/UI_HeartParticle"), GameObject.Find("Canvas").transform);
            particleObject.transform.position = m_panels[lifeCount].transform.position;

            // 터지는 애니메이션 재생
            m_panelsAnimator[lifeCount].SetBool("isShoot", true);

            // 몇초간 쉐이킹
            if (m_shakeCoroutine != null)
                StopCoroutine(m_shakeCoroutine);
            m_shakeCoroutine = StartCoroutine(Shake(lifeCount, 10f, 0.7f));
        }

        private IEnumerator Shake(int lifeCount, float ShakeAmount, float ShakeTime)
        {
            RectTransform rectTransform = m_panels[lifeCount].GetComponent<RectTransform>();
            Vector3 startPosition       = rectTransform.anchoredPosition;

            float timer = 0;
            while (timer <= ShakeTime)
            {
                timer += Time.deltaTime;

                Vector3 randomPoint = startPosition + Random.insideUnitSphere * ShakeAmount;
                rectTransform.anchoredPosition = Vector3.Lerp(startPosition, randomPoint, timer/ ShakeTime);
                yield return null;
            }

            rectTransform.anchoredPosition = startPosition;

            // 넘어지는 애니메이션 재생
            m_panelsAnimator[lifeCount].SetBool("isShave", true);

            yield break;
        }
    }
}

