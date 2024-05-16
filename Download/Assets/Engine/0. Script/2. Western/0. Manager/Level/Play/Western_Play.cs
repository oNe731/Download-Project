using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Western
{
    public class Western_Play : Level
    {
        protected GameObject m_stage = null;
        protected Groups m_groups = null;
        protected GameObject m_targetUI = null;
        protected CameraWalk m_camera = null;
        protected GameObject m_readyGoUI = null;

        protected int m_life = 4;
        protected bool m_startGroup = false;
        protected float m_uiTime = 0f;

        protected List<string> m_criminalText = new List<string>();
        protected List<string> m_citizenText = new List<string>();

        public Western_Play(LevelController levelController) : base(levelController)
        {
        }

        public override void Enter_Level()
        {
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
            // 해당 구역에 도착했을 때 판넬 세우기
            if (m_startGroup == true && m_camera.IsMove == false)
            {
                m_startGroup = false;
                m_groups.WakeUp_Next(true, 0.7f);
            }
            else if (WesternManager.Instance.IsShoot == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.CompareTag("Person"))
                        {
                            Vector3 position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 0.001f);
                            if (m_targetUI == null)
                                m_targetUI = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/TargetUI"), position, Quaternion.identity);
                            else
                                m_targetUI.transform.position = position;

                            m_targetUI.GetComponent<TargetUI>().Target = hit.collider.gameObject;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (m_targetUI == null)
                        return;

                    WesternManager.Instance.IsShoot = false;
                    if (m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>().PersonType == Person.PERSONTYPE.PT_CRIMINAL) // 범인일 때
                    {
                        // 범인일 때
                        Create_SpeechBubble(m_groups.Get_Criminal().transform.position, ref m_criminalText, Random.Range(0, m_criminalText.Count));
                    }
                    else
                    {
                        // 시민일 때
                        Create_SpeechBubble(m_groups.Get_Criminal().transform.position, ref m_citizenText, Random.Range(0, m_citizenText.Count));
                    }

                    // 총알자국 오브젝트 생성.
                    Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/BulletMarkUI"), m_targetUI.transform.position, Quaternion.identity, m_targetUI.GetComponent<TargetUI>().Target.transform);
                    Destroy(m_targetUI);
                }
            }
        }

        public override void Exit_Level()
        {
        }

        public void Fail_Group()
        {
            if (m_targetUI != null)
                Destroy(m_targetUI);

            WesternManager.Instance.IsShoot = false;
            m_groups.Get_Criminal().GetComponent<Criminal>().Change_Attack();
        }

        public void Attacked_Player()
        {
            m_life--;
            Debug.Log("Life : " + m_life);

            // hp UI 변경
            // 

            if (m_life > 0)
            {
                m_groups.LayDown_Group(true);
            }
            else
            {
                Debug.Log("게임 종료");
            }
        }

        public void Proceed_Next()
        {
            m_startGroup = true;
            m_camera.Start_Move(m_groups.Next_Position());
        }

        private void Create_SpeechBubble(Vector3 position, ref List<string> textlist, int index)
        {
            // 타이머 삭제
            m_groups.Destroy_Timer();

            // 말풍선 UI
            GameObject uiObject = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/UI_SpeechBubble"), GameObject.Find("Canvas").transform);
            uiObject.GetComponent<Transform>().position = Camera.main.WorldToScreenPoint(position + new Vector3(0f, 0.5f, 0f));
            uiObject.transform.GetChild(0).GetComponent<TMP_Text>().text = textlist[index];
            textlist.RemoveAt(index);
        }
    }
}

