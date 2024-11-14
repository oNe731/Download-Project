using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Western
{
    public class Western_PlayLv1 : Western_Round1
    {
        protected GameObject m_stage = null;
        protected Groups m_groups = null;
        protected GameObject m_targetUI = null;
        protected GameObject m_timer = null;
        protected CameraWalk m_camera = null;
        protected GameObject m_readyGoUI = null;

        protected int m_life = 5;
        protected bool m_startGroup = false;
        protected float m_uiTime = 0f;
        protected int m_uiIndex = 0;

        protected List<string> m_criminalText = new List<string>();
        protected List<string> m_citizenText = new List<string>();

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

        enum STATETYPE { TYPE_DIALOGSTART, TYPE_TUTORIALPERSON, TYPE_DIALOGFINISH, TYPE_GAMESTART, TYPE_END };

        private STATETYPE m_stateType = STATETYPE.TYPE_END;

        private List<GameObject> m_tutorialTarget = new List<GameObject>();
        private bool m_isTutorial = true;
        private int m_tutorialIndex = -1;

        private Bar m_bar = null;


        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);


        }

        public override void Enter_Level()
        {
            m_criminalText.Clear();
            m_criminalText.Add("캬옹!!!");
            m_criminalText.Add("으악!");
            m_criminalText.Add("크흑...숨길 수 있었는데");
            m_criminalText.Add("분하다옹...!");
            m_criminalText.Add("야오옹...");
            m_criminalText.Add("젠장, 들켰다옹");
            m_criminalText.Add("고양이의 수치다냥...");
            m_criminalText.Add("내 사랑스런 수염이!");
            m_criminalText.Add("내 은신을 간파하다니...");
            m_criminalText.Add("당신은 전설의...!");

            m_citizenText.Clear();
            m_citizenText.Add("후후후...");
            m_citizenText.Add("선량한 시민을 고르셨군");
            m_citizenText.Add("하하! 멍청한 녀석이다옹");
            m_citizenText.Add("없던 수전증이라도 생겼냐옹?");
            m_citizenText.Add("두 눈 똑바로 뜨라옹");
            m_citizenText.Add("어이! 나 여기있다옹");
            m_citizenText.Add("내 움직임을 얕본 대가다옹");
            m_citizenText.Add("흥, 별거없군");
            m_citizenText.Add("고양이 앞에서 한눈을 팔다니");
            m_citizenText.Add("고양이 목숨이 몇개인지 알아?");

            base.Enter_Level();
            m_heartUI.Reset_Heart();

            // 스테이지 생성
            m_stage = GameManager.Ins.Western.Stage.transform.GetChild(1).gameObject;
            m_groups = m_stage.transform.Find("Group").GetComponent<Groups>();
            m_bar = m_stage.transform.GetChild(0).transform.GetChild(3).GetComponent<Bar>();
            m_operation.SetActive(true);

            // 카메라 설정
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_BASIC_3D);
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            CameraCutscene camera = (CameraCutscene)GameManager.Ins.Camera.Get_CurCamera();
            camera.Change_Position(new Vector3(0f, 0.62f, m_groups.Start_Position(0).z));
            camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));

            // 폭탄 이벤트 추가
            m_eventIndex = new List<int>();
            m_eventCount = Random.Range(3, 6); // 최소 3 - 5번
            List<int> availableNumbers = new List<int>();
            for (int i = 2; i <= 10; i++) // 1번 그룹 제외 2부터 10까지 값 존재
                availableNumbers.Add(i);
            for (int i = 0; i < m_eventCount; ++i)
            {
                int randomIndex = Random.Range(0, availableNumbers.Count);
                m_eventIndex.Add(availableNumbers[randomIndex]);
                availableNumbers.RemoveAt(randomIndex);
            }
            m_eventIndex.Sort();

            if (GameManager.Ins.Western.LevelController.Prelevel != (int)WesternManager.LEVELSTATE.LS_PlayLv1)
            {
                // 다이얼로그 시작
                GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Start_Dialog());
                GameManager.Ins.Sound.Play_AudioSourceBGM("Western_MainBGM", true, 1f);
            }
            else
            {
                // 튜토리얼 제외 후 게임 시작
                GameManager.Ins.IsGame = true;
                GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Set_ReadyStart());
                GameManager.Ins.Sound.Stop_AudioSourceBGM();
            }
        }

        private void Set_ReadyStart()
        {
            m_life = 5;

            m_isTutorial = true;
            m_stateType = STATETYPE.TYPE_GAMESTART;

            GameManager.Ins.Resource.Destroy(m_groups.Group[0].gameObject);
            m_groups.CurrentIndex = 0;
        }

        public override void Play_Level() // 튜토리얼 진행 후 Ready Go UI 출력 후 해당 함수 호출
        {
            m_tutorialTarget.Clear();
            m_isTutorial = false;

            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_WALK);
            m_camera = (CameraWalk)GameManager.Ins.Camera.Get_CurCamera();
            m_camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));
            m_camera.Set_Height(0.62f);

            Proceed_Next();

            // BGM 변경
            GameManager.Ins.Sound.Play_AudioSourceBGM("Western_Play1BGM", true, 1f);
        }

        public override void Update_Level()
        {
            if (GameManager.Ins.IsGame == false)
                return;

            if (m_isTutorial)
                Update_Tutorial();
            else
                Update_Play();
        }

        public override void LateUpdate_Level()
        {
        }

        public override void Exit_Level()
        {
            GameManager.Ins.Resource.Destroy(m_stage);
        }


        private void Start_Dialog()
        {
            m_stateType = STATETYPE.TYPE_DIALOGSTART;
            m_dialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(true, GameManager.Ins.Load_JsonData<DialogData_PlayWT>("4. Data/2. Western/Dialog/Play/Round1/Dialog1_Play"));
        }

        private void Update_Tutorial()
        {
            if (m_stateType == STATETYPE.TYPE_DIALOGSTART) // 마지막 다이얼로그가 전부 출력되면 판넬 활성화 및 튜토리얼 진행
            {
                if (m_dialogPlay.LastIndex == true)
                {
                    m_stateType = STATETYPE.TYPE_TUTORIALPERSON;
                    m_groups.WakeUp_Next(ref m_eventIndex, false, 0.4f);
                }
            }
            else if (m_stateType == STATETYPE.TYPE_TUTORIALPERSON) // 범인 튜토리얼
            {
                if (GameManager.Ins.Western.IsShoot == true)
                {
                    if (Input.GetMouseButtonDown(0)) { Click_Panel(); }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (m_targetUI == null)
                            return;

                        // 튜토리얼에서 이미 쐈던 사람일 시 흔들림.
                        if (Check_Person(m_targetUI.GetComponent<TargetUI>().Target))
                        {
                            m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>().Start_Shake();
                        }
                        else
                        {
                            if (m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>().PersonType == Person.PERSONTYPE.PT_CRIMINAL) // 범인일 때
                            {
                                GameObject criminal = m_groups.Get_Criminal();
                                GameManager.Ins.Sound.Play_AudioSource(criminal.GetComponent<AudioSource>(), "Western_Criminal_Caught", false, 1f);

                                GameManager.Ins.Western.IsShoot = false;
                                m_dialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(false, GameManager.Ins.Load_JsonData<DialogData_PlayWT>("4. Data/2. Western/Dialog/Play/Round1/Dialog1_Tutorial_Criminal"));
                                m_stateType = STATETYPE.TYPE_END; // 폭탄에서 튜토리얼 진행
                            }
                            else
                            {
                                m_tutorialIndex++;
                                switch (m_tutorialIndex)
                                {
                                    case 0:
                                        m_dialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(false, GameManager.Ins.Load_JsonData<DialogData_PlayWT>("4. Data/2. Western/Dialog/Play/Round1/Dialog1_Tutorial_Citizen1"));
                                        break;

                                    case 1:
                                        m_dialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(false, GameManager.Ins.Load_JsonData<DialogData_PlayWT>("4. Data/2. Western/Dialog/Play/Round1/Dialog1_Tutorial_Citizen2"));
                                        break;
                                }

                                m_tutorialTarget.Add(m_targetUI.GetComponent<TargetUI>().Target);
                            }

                            Space_Panel();
                        }

                        GameManager.Ins.Resource.Destroy(m_targetUI);
                    }
                }
            }
            else if (m_stateType == STATETYPE.TYPE_DIALOGFINISH)
            {
                if (m_dialogPlay.Active == false)
                {
                    m_tutorialIndex = 0;
                    m_stateType = STATETYPE.TYPE_GAMESTART;

                    m_groups.LayDown_Group();
                }
            }
            else if (m_stateType == STATETYPE.TYPE_GAMESTART) // 레디 고 UI 출력 후 게임 시작
            {
                GameManager.Ins.StartCoroutine(Update_ReadyGo());
                m_stateType = STATETYPE.TYPE_END;
            }
        }

        private void Update_Play()
        {
            if (m_finishGroup != true)
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

        private bool Check_Person(GameObject target)
        {
            for (int i = 0; i < m_tutorialTarget.Count; ++i)
            {
                if (m_tutorialTarget[i] == target)
                    return true;
            }

            return false;
        }

        public void Play_Finish() // 마지막 골인 지점에 도착하는 즉시 호출
        {
            if (m_bar == null)
                return;

            // 바 이벤트 발생
            m_bar.Start_Event();

            // 설명 UI 비활성화
            m_operation.SetActive(false);
        }

        public void Change_FinishDialog()
        {
            m_stateType = STATETYPE.TYPE_DIALOGFINISH;
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
                    m_gun.Click_Gun();

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
            m_gun.Shoot_Gun();

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

            //Debug.Log("타입:" + type + ", 사용 대사 인덱스: " + index + ", 개수: " + textlist.Count);

            uiObject.transform.GetChild(0).GetComponent<TMP_Text>().text = textlist[index];
            textlist.RemoveAt(index);
        }


        public void Proceed_Next()
        {
            m_startGroup = true;

            // 카메라 업데이트
            m_camera.Start_Move(m_groups.Next_Position());

            // UI 업데이트
            m_statusBarUI.Start_UpdateValue(m_groups.CurrentIndex, m_groups.CurrentIndex + 1,
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
            m_heartUI.Start_Update(m_life, laydown);
        }

        public void LayDown_Group(bool nextMove = false)
        {
            m_groups.LayDown_Group(nextMove);
        }

        public void Create_Timer(float timerSpeed)
        {
            Destroy_Timer();

            m_timer = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/UI_Timer", Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
            RectTransform timerTransform = m_timer.GetComponent<RectTransform>();
            timerTransform.anchoredPosition = new Vector2(0f, 281f);

            Timer timer = m_timer.GetComponent<Timer>();
            timer.Start_Timer(timerSpeed);
        }

        public void Destroy_Timer()
        {
            if (m_timer != null)
                GameManager.Ins.Resource.Destroy(m_timer);
        }

        public void Restart_Game()
        {
            // 게임 재시작
            GameManager.Ins.Western.LevelController.Change_Level((int)WesternManager.LEVELSTATE.LS_PlayLv1, true);
        }

        public void Over_Game()
        {
            // 이전 요소 삭제
            GameManager.Ins.IsGame = false;

            GameManager.Ins.UI.EventUpdate = true;
            GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/Panel_Fail", GameObject.Find("Canvas").transform), 0f, false);
        }

        public void Destroy_Element()
        {
            Destroy_Timer();
            if (m_targetUI != null)
                GameManager.Ins.Resource.Destroy(m_targetUI);
        }
    }
}

