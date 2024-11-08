using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Western
{
    public class Western_PlayLv1 : Western_Play
    {
        enum STATETYPE { TYPE_DIALOGSTART, TYPE_TUTORIALPLAY, TYPE_DIALOGFINISH, TYPE_GAMESTART, TYPE_END };

        private STATETYPE m_stateType = STATETYPE.TYPE_END;
        private List<GameObject> m_tutorialTarget = new List<GameObject>();
        private bool m_isTutorial = true;
        private int m_tutorialIndex = -1;
        private Bar m_bar = null;

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);

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

            m_citizenText.Add("후후후...");
            m_citizenText.Add("선량한 시민을 고르셨군");
            m_citizenText.Add("하하! 멍청한 녀석이다옹");
            m_citizenText.Add("없던 수전증이라도 생겼냐옹?");
            m_citizenText.Add("두 눈 똑바로 뜨라옹");
            m_citizenText.Add("어이! 나 여기있다옹");
            m_citizenText.Add("내 움직임을 얏본 대가다옹");
            m_citizenText.Add("흥, 별거없군");
            m_citizenText.Add("고양이 앞에서 한 눈을 팔다니");
            m_citizenText.Add("고양이 목숨이 몇개인지 알아?");

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
        }

        public override void Enter_Level()
        {
            base.Enter_Level();

            // 스테이지 생성
            m_stage = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/1Stage/1Stage");
            m_groups = m_stage.transform.Find("Group").GetComponent<Groups>();
            m_bar = m_stage.transform.GetChild(0).transform.GetChild(3).GetComponent<Bar>();
            GameManager.Ins.Western.Operation.SetActive(true);

            // 카메라 설정
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_BASIC_3D);
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            CameraCutscene camera = (CameraCutscene)GameManager.Ins.Camera.Get_CurCamera();
            camera.Change_Position(new Vector3(0f, 0.62f, m_groups.Start_Position().z));
            camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));

            // 다이얼로그 시작
            GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Start_Dialog());

            GameManager.Ins.Sound.Play_AudioSourceBGM("Western_MainBGM", true, 1f);
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
                base.Update_Level();
        }

        public override void LateUpdate_Level()
        {
        }

        public override void Exit_Level()
        {
            base.Exit_Level();
        }


        private void Start_Dialog()
        {
            m_stateType = STATETYPE.TYPE_DIALOGSTART;
            GameManager.Ins.Western.DialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(true, GameManager.Ins.Load_JsonData<DialogData_PlayWT>("4. Data/2. Western/Dialog/Play/Round1/Dialog1_Play"));
        }

        private void Update_Tutorial()
        {
            if (m_stateType == STATETYPE.TYPE_DIALOGSTART) // 마지막 다이얼로그가 전부 출력되면 판넬 활성화 및 튜토리얼 진행
            {
                if (GameManager.Ins.Western.DialogPlay.LastIndex == true)
                {
                    m_stateType = STATETYPE.TYPE_TUTORIALPLAY;
                    m_groups.WakeUp_Next(ref m_eventIndex, false, 0.4f);
                }
            }
            else if (m_stateType == STATETYPE.TYPE_TUTORIALPLAY)
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
                                GameManager.Ins.Western.DialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(false, GameManager.Ins.Load_JsonData<DialogData_PlayWT>("4. Data/2. Western/Dialog/Play/Round1/Dialog1_Tutorial_Criminal"));
                                m_stateType = STATETYPE.TYPE_DIALOGFINISH;
                            }
                            else
                            {
                                m_tutorialIndex++;
                                switch (m_tutorialIndex)
                                {
                                    case 0:
                                        GameManager.Ins.Western.DialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(false, GameManager.Ins.Load_JsonData<DialogData_PlayWT>("4. Data/2. Western/Dialog/Play/Round1/Dialog1_Tutorial_Citizen1"));
                                        break;

                                    case 1:
                                        GameManager.Ins.Western.DialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(false, GameManager.Ins.Load_JsonData<DialogData_PlayWT>("4. Data/2. Western/Dialog/Play/Round1/Dialog1_Tutorial_Citizen2"));
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
                if (GameManager.Ins.Western.DialogPlay.Active == false)
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

        private bool Check_Person(GameObject target)
        {
            for (int i = 0; i < m_tutorialTarget.Count; ++i)
            {
                if (m_tutorialTarget[i] == target)
                    return true;
            }

            return false;
        }

        public override void Play_Finish() // 마지막 골인 지점에 도착하는 즉시 호출
        {
            if (m_bar == null)
                return;

            // 바 이벤트 발생
            m_bar.Start_Event();
        }
    }
}

