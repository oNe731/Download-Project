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
            m_criminalText.Add("ļ��!!!");
            m_criminalText.Add("����!");
            m_criminalText.Add("ũ��...���� �� �־��µ�");
            m_criminalText.Add("���ϴٿ�...!");
            m_criminalText.Add("�߿���...");
            m_criminalText.Add("����, ���״ٿ�");
            m_criminalText.Add("������� ��ġ�ٳ�...");
            m_criminalText.Add("�� ������� ������!");
            m_criminalText.Add("�� ������ �����ϴٴ�...");
            m_criminalText.Add("����� ������...!");

            m_citizenText.Clear();
            m_citizenText.Add("������...");
            m_citizenText.Add("������ �ù��� ���̱�");
            m_citizenText.Add("����! ��û�� �༮�̴ٿ�");
            m_citizenText.Add("���� �������̶� ����Ŀ�?");
            m_citizenText.Add("�� �� �ȹٷ� �߶��");
            m_citizenText.Add("����! �� �����ִٿ�");
            m_citizenText.Add("�� �������� �躻 �밡�ٿ�");
            m_citizenText.Add("��, ���ž���");
            m_citizenText.Add("����� �տ��� �Ѵ��� �ȴٴ�");
            m_citizenText.Add("����� ����� ����� �˾�?");

            base.Enter_Level();
            m_heartUI.Reset_Heart();

            // �������� ����
            m_stage = GameManager.Ins.Western.Stage.transform.GetChild(1).gameObject;
            m_groups = m_stage.transform.Find("Group").GetComponent<Groups>();
            m_bar = m_stage.transform.GetChild(0).transform.GetChild(3).GetComponent<Bar>();
            m_operation.SetActive(true);

            // ī�޶� ����
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_BASIC_3D);
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            CameraCutscene camera = (CameraCutscene)GameManager.Ins.Camera.Get_CurCamera();
            camera.Change_Position(new Vector3(0f, 0.62f, m_groups.Start_Position(0).z));
            camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));

            // ��ź �̺�Ʈ �߰�
            m_eventIndex = new List<int>();
            m_eventCount = Random.Range(3, 6); // �ּ� 3 - 5��
            List<int> availableNumbers = new List<int>();
            for (int i = 2; i <= 10; i++) // 1�� �׷� ���� 2���� 10���� �� ����
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
                // ���̾�α� ����
                GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Start_Dialog());
                GameManager.Ins.Sound.Play_AudioSourceBGM("Western_MainBGM", true, 1f);
            }
            else
            {
                // Ʃ�丮�� ���� �� ���� ����
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

        public override void Play_Level() // Ʃ�丮�� ���� �� Ready Go UI ��� �� �ش� �Լ� ȣ��
        {
            m_tutorialTarget.Clear();
            m_isTutorial = false;

            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_WALK);
            m_camera = (CameraWalk)GameManager.Ins.Camera.Get_CurCamera();
            m_camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));
            m_camera.Set_Height(0.62f);

            Proceed_Next();

            // BGM ����
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
            if (m_stateType == STATETYPE.TYPE_DIALOGSTART) // ������ ���̾�αװ� ���� ��µǸ� �ǳ� Ȱ��ȭ �� Ʃ�丮�� ����
            {
                if (m_dialogPlay.LastIndex == true)
                {
                    m_stateType = STATETYPE.TYPE_TUTORIALPERSON;
                    m_groups.WakeUp_Next(ref m_eventIndex, false, 0.4f);
                }
            }
            else if (m_stateType == STATETYPE.TYPE_TUTORIALPERSON) // ���� Ʃ�丮��
            {
                if (GameManager.Ins.Western.IsShoot == true)
                {
                    if (Input.GetMouseButtonDown(0)) { Click_Panel(); }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (m_targetUI == null)
                            return;

                        // Ʃ�丮�󿡼� �̹� ���� ����� �� ��鸲.
                        if (Check_Person(m_targetUI.GetComponent<TargetUI>().Target))
                        {
                            m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>().Start_Shake();
                        }
                        else
                        {
                            if (m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>().PersonType == Person.PERSONTYPE.PT_CRIMINAL) // ������ ��
                            {
                                GameObject criminal = m_groups.Get_Criminal();
                                GameManager.Ins.Sound.Play_AudioSource(criminal.GetComponent<AudioSource>(), "Western_Criminal_Caught", false, 1f);

                                GameManager.Ins.Western.IsShoot = false;
                                m_dialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(false, GameManager.Ins.Load_JsonData<DialogData_PlayWT>("4. Data/2. Western/Dialog/Play/Round1/Dialog1_Tutorial_Criminal"));
                                m_stateType = STATETYPE.TYPE_END; // ��ź���� Ʃ�丮�� ����
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
            else if (m_stateType == STATETYPE.TYPE_GAMESTART) // ���� �� UI ��� �� ���� ����
            {
                GameManager.Ins.StartCoroutine(Update_ReadyGo());
                m_stateType = STATETYPE.TYPE_END;
            }
        }

        private void Update_Play()
        {
            if (m_finishGroup != true)
            {
                // �ش� ������ �������� �� �ǳ� �����
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
                        if (m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>().PersonType == Person.PERSONTYPE.PT_CRIMINAL) // ������ ��
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

        public void Play_Finish() // ������ ���� ������ �����ϴ� ��� ȣ��
        {
            if (m_bar == null)
                return;

            // �� �̺�Ʈ �߻�
            m_bar.Start_Event();

            // ���� UI ��Ȱ��ȭ
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
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)  // �ִϸ��̼� ������ ��
                    {
                        m_readyGoUI.GetComponent<Animator>().SetBool("IsReadyGo", true);
                        m_uiIndex++;
                    }
                }
                else if (m_uiIndex == 1 && animator.GetCurrentAnimatorStateInfo(0).IsName("AM_Go") == true)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)  // �ִϸ��̼� ������ ��
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

                    Vector3 position = new Vector3(hit.point.x, hit.point.y, hit.collider.gameObject.GetComponent<Person>().Get_GroupZ() - 0.005f); // �� ��
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

            // �Ͼ�� ȭ������ ��½ ȿ�� ���� (������ �ѹ��� ������°� ������ ������)
            GameManager.Ins.UI.Start_FadeIn(0.3f, Color.white);

            // ����Ʈ ����
            GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/1Stage/Effect/Person_Effect", m_targetUI.transform.position, Quaternion.identity);

            // �Ѿ��ڱ� ������Ʈ ����.
            GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/BulletMarkUI", m_targetUI.transform.position, Quaternion.identity, m_targetUI.GetComponent<TargetUI>().Target.transform);
        }

        private void Create_SpeechBubble(Person.PERSONTYPE type, Vector3 position, ref List<string> textlist, int index)
        {
            // Ÿ�̸� ����
            m_groups.Destroy_Timer();

            // ��ǳ�� UI
            GameObject uiObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/UI_SpeechBubble", GameObject.Find("Canvas").transform);
            uiObject.GetComponent<SpeechBubble>().PersonType = type;
            uiObject.GetComponent<Transform>().position = Camera.main.WorldToScreenPoint(position + new Vector3(0f, 0.5f, 0f));

            //Debug.Log("Ÿ��:" + type + ", ��� ��� �ε���: " + index + ", ����: " + textlist.Count);

            uiObject.transform.GetChild(0).GetComponent<TMP_Text>().text = textlist[index];
            textlist.RemoveAt(index);
        }


        public void Proceed_Next()
        {
            m_startGroup = true;

            // ī�޶� ������Ʈ
            m_camera.Start_Move(m_groups.Next_Position());

            // UI ������Ʈ
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
            // ���� �����
            GameManager.Ins.Western.LevelController.Change_Level((int)WesternManager.LEVELSTATE.LS_PlayLv1, true);
        }

        public void Over_Game()
        {
            // ���� ��� ����
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

