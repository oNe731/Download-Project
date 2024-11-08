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

            m_citizenText.Add("������...");
            m_citizenText.Add("������ �ù��� ���̱�");
            m_citizenText.Add("����! ��û�� �༮�̴ٿ�");
            m_citizenText.Add("���� �������̶� ����Ŀ�?");
            m_citizenText.Add("�� �� �ȹٷ� �߶��");
            m_citizenText.Add("����! �� �����ִٿ�");
            m_citizenText.Add("�� �������� �溻 �밡�ٿ�");
            m_citizenText.Add("��, ���ž���");
            m_citizenText.Add("����� �տ��� �� ���� �ȴٴ�");
            m_citizenText.Add("����� ����� ����� �˾�?");

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
        }

        public override void Enter_Level()
        {
            base.Enter_Level();

            // �������� ����
            m_stage = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/1Stage/1Stage");
            m_groups = m_stage.transform.Find("Group").GetComponent<Groups>();
            m_bar = m_stage.transform.GetChild(0).transform.GetChild(3).GetComponent<Bar>();
            GameManager.Ins.Western.Operation.SetActive(true);

            // ī�޶� ����
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_BASIC_3D);
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            CameraCutscene camera = (CameraCutscene)GameManager.Ins.Camera.Get_CurCamera();
            camera.Change_Position(new Vector3(0f, 0.62f, m_groups.Start_Position().z));
            camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));

            // ���̾�α� ����
            GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Start_Dialog());

            GameManager.Ins.Sound.Play_AudioSourceBGM("Western_MainBGM", true, 1f);
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
            if (m_stateType == STATETYPE.TYPE_DIALOGSTART) // ������ ���̾�αװ� ���� ��µǸ� �ǳ� Ȱ��ȭ �� Ʃ�丮�� ����
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
            else if (m_stateType == STATETYPE.TYPE_GAMESTART) // ���� �� UI ��� �� ���� ����
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

        public override void Play_Finish() // ������ ���� ������ �����ϴ� ��� ȣ��
        {
            if (m_bar == null)
                return;

            // �� �̺�Ʈ �߻�
            m_bar.Start_Event();
        }
    }
}

