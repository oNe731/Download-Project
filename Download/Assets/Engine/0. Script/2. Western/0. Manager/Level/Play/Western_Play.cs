using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Western
{
    public abstract class Western_Play : Level
    {
        protected GameObject m_stage     = null;
        protected Groups     m_groups    = null;
        protected GameObject m_targetUI  = null;
        protected CameraWalk m_camera    = null;
        protected GameObject m_readyGoUI = null;

        protected int   m_life = 5;
        protected bool  m_startGroup = false;
        protected float m_uiTime  = 0f;
        protected int   m_uiIndex = 0;

        protected List<string> m_criminalText = new List<string>();
        protected List<string> m_citizenText  = new List<string>();

        private bool m_finishGroup = false;

        protected int m_eventCount;
        protected List<int> m_eventIndex;

        public Groups Groups => m_groups;
        public bool finishGroup
        {
            get => m_finishGroup;
            set => m_finishGroup = value;
        }
        public GameObject TargetUI
        {
            get => m_targetUI;
            set => m_targetUI = value;
        }

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            GameManager.Ins.Western.HeartUI.Reset_Heart();
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
            if(m_finishGroup != true)
            {
                // 해당 구역에 도착했을 때 판넬 세우기
                if (m_startGroup == true && m_camera.IsMove == false)
                {
                    m_startGroup = false;
                    m_groups.WakeUp_Next(ref m_eventIndex, true, 0.4f);
                }
                else if (GameManager.Ins.Western.IsShoot == true)
                {
                    if (Input.GetMouseButtonDown(0)) { Click_Panel(); }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (m_targetUI == null)
                            return;

                        GameManager.Ins.Western.IsShoot = false;
                        if (m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>().PersonType == Person.PERSONTYPE.PT_CRIMINAL) // 범인일 때
                        {
                            GameObject criminal = m_groups.Get_Criminal();
                            GameManager.Ins.Sound.Play_AudioSource(criminal.GetComponent<AudioSource>(), "Western_Criminal_Caught", false, 1f);

                            Create_SpeechBubble(Person.PERSONTYPE.PT_CRIMINAL, criminal.transform.position, ref m_criminalText, Random.Range(0, m_criminalText.Count));
                        }
                        else
                            Create_SpeechBubble(Person.PERSONTYPE.PT_CITIZEN, m_groups.Get_Criminal().transform.position, ref m_citizenText, Random.Range(0, m_citizenText.Count));

                        Space_Panel();
                        GameManager.Ins.Resource.Destroy(m_targetUI);
                    }
                }
            }
        }

        public abstract void Play_Finish();

        public override void Exit_Level()
        {
            GameManager.Ins.Resource.Destroy(m_stage);
        }


        protected IEnumerator Update_ReadyGo()
        {
            m_uiIndex = 0;
            m_readyGoUI = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/UI_ReadyGo", GameObject.Find("Canvas").transform);
            Animator animator = m_readyGoUI.GetComponent<Animator>();

            while (m_uiIndex < 2)
            {
                if (m_uiIndex == 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("AM_Ready") == true)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)  // 애니메이션 종료일 시
                    {
                        m_readyGoUI.GetComponent<Animator>().SetBool("IsReadyGo", true);
                        m_uiIndex++;
                    }
                }
                else if (m_uiIndex == 1 && animator.GetCurrentAnimatorStateInfo(0).IsName("AM_Go") == true)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)  // 애니메이션 종료일 시
                    {
                        GameManager.Ins.Resource.Destroy(m_readyGoUI);
                        Play_Level();
                        m_uiIndex++;
                    }
                }

                yield return null;
            }

            yield break;
        }

        protected void Click_Panel()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Person"))
                {
                    GameManager.Ins.Western.Gun.Click_Gun();

                    Vector3 position = new Vector3(hit.point.x, hit.point.y, hit.collider.gameObject.GetComponent<Person>().Get_GroupZ() - 0.005f); // 맨 앞
                    if (m_targetUI == null)
                        m_targetUI = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/TargetUI", position, Quaternion.identity);
                    else
                        m_targetUI.transform.position = position;

                    m_targetUI.GetComponent<TargetUI>().Target = hit.collider.gameObject;
                }
            }
        }

        protected void Space_Panel()
        {
            GameManager.Ins.Western.Gun.Shoot_Gun();

            Person person = m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>();
            person.Start_Shake();
            person.Stop_Animation();

            // 하얀색 화면으로 번쩍 효과 적용 (등장은 한번에 사라지는건 서서히 빠르게)
            GameManager.Ins.UI.Start_FadeIn(0.3f, Color.white);

            // 이펙트 생성
            GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/1Stage/Effect/Person_Effect", m_targetUI.transform.position, Quaternion.identity);

            // 총알자국 오브젝트 생성.
            GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/BulletMarkUI", m_targetUI.transform.position, Quaternion.identity, m_targetUI.GetComponent<TargetUI>().Target.transform);
        }

        private void Create_SpeechBubble(Person.PERSONTYPE type, Vector3 position, ref List<string> textlist, int index)
        {
            // 타이머 삭제
            m_groups.Destroy_Timer();

            // 말풍선 UI
            GameObject uiObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/UI_SpeechBubble", GameObject.Find("Canvas").transform);
            uiObject.GetComponent<SpeechBubble>().PersonType = type;
            uiObject.GetComponent<Transform>().position = Camera.main.WorldToScreenPoint(position + new Vector3(0f, 0.5f, 0f));
            uiObject.transform.GetChild(0).GetComponent<TMP_Text>().text = textlist[index];
            textlist.RemoveAt(index);
        }


        public void Proceed_Next()
        {
            m_startGroup = true;

            // 카메라 업데이트
            m_camera.Start_Move(m_groups.Next_Position());

            // UI 업데이트
            GameManager.Ins.Western.StatusBarUI.Start_UpdateValue(m_groups.CurrentIndex, m_groups.CurrentIndex + 1,
                Camera.main.transform.position, m_groups.Next_Position());
        }

        public void Fail_Group()
        {
            if (m_targetUI != null)
                GameManager.Ins.Resource.Destroy(m_targetUI);

            m_groups.Destroy_Timer();

            GameManager.Ins.Western.IsShoot = false;
            m_groups.Get_Criminal().GetComponent<Criminal>().Change_Attack();
        }

        public void Attacked_Player(bool laydown = true)
        {
            m_life--;
            GameManager.Ins.Western.HeartUI.Start_Update(m_life, laydown);
        }

        public void LayDown_Group(bool nextMove = false)
        {
            m_groups.LayDown_Group(nextMove);
        }
    }
}

