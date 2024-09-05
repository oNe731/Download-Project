using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VisualNovel
{
    public class Novel_Shoot : Level
    {
        private DOLLTYPE m_dollType = DOLLTYPE.DT_FAIL;

        private bool m_shootGameStart = false;
        private bool m_shootGameStop = false;
        private bool m_shootGameOver = false;
        private bool m_shootGameNext = false;
        private float m_time = 0f;
        private float m_maxTime = 60f;
        private float m_overTime = 0f;

        private GameObject m_stage;
        private TMP_Text m_countTxt;
        private ShootContainerBelt m_container;

        public DOLLTYPE DollType
        {
            get => m_dollType;
            set => m_dollType = value;
        }
        public bool ShootGameStart
        {
            get => m_shootGameStart;
            set => m_shootGameStart = value;
        }
        public bool ShootGameStop
        {
            get => m_shootGameStop;
            set => m_shootGameStop = value;
        }
        public bool ShootGameOver
        {
            get => m_shootGameOver;
            set => m_shootGameOver = value;
        }

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            VisualNovelManager.Instance.Dialog.SetActive(false);
            m_stage = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/Map/Shoot");
            m_countTxt = m_stage.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>();
            m_container = m_stage.transform.GetChild(1).GetChild(0).gameObject.GetComponent<ShootContainerBelt>();

            m_time = m_maxTime;
            m_countTxt.text = m_time.ToString();

            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_BASIC_2D);
            CameraBasic_2D camera = (CameraBasic_2D)GameManager.Ins.Camera.Get_CurCamera();
            camera.Change_Position(new Vector3(0f, 0f, -9f));
            camera.Change_Rotation(new Vector3(0f, 0f, 0f));

            GameManager.Ins.UI.Start_FadeIn(1f, Color.black);
        }

        public override void Play_Level()
        {
            // 새총 BGM
            GameManager.Ins.Sound.Play_AudioSourceBGM("VisualNovel_ShootBGM", true, 1f);

            m_shootGameStart = true;
            GameManager.Ins.UI.Change_Cursor(CURSORTYPE.CT_NOVELSHOOT);
            m_container.Start_Belt();
        }

        public override void Update_Level()
        {
            if (!m_shootGameStart || m_shootGameStop)
                return;

            if (!m_shootGameOver)
                Update_Count();
            else
                GameOver_ShootGame();
        }

        public override void Exit_Level()
        {
            Camera.main.GetComponent<AudioSource>().Stop();
            GameManager.Ins.UI.Change_Cursor(CURSORTYPE.CT_ORIGIN);
            Destroy(m_stage);
        }

        public override void OnDrawGizmos()
        {
        }

        private void Update_Count()
        {
            m_time -= Time.deltaTime;
            if (m_time <= 0.5f)
            {
                int Count = 0;
                m_countTxt.text = Count.ToString();

                m_shootGameOver = true;
                m_container.UseBelt = false; // 1) 인형 일시 정지

                m_dollType = DOLLTYPE.DT_FAIL;

            }
            else
            {
                int Count = (int)m_time;
                m_countTxt.text = Count.ToString();
            }
        }

        private void GameOver_ShootGame()
        {
            if (m_shootGameNext)
                return;

            // 인형 또는 쓰레기 1개 이상 획득해도 사격 게임 종료/ 미연시 시작 : 맞춘 종류에 따라 다음 대사가 다름.
            m_overTime += Time.deltaTime;
            if (m_overTime > 1.5f)
            {
                if (!m_container.OverEffect)
                    m_container.Over_Game(); // 2) 1.5초 뒤 인형 전부 폭발
                else if (!m_shootGameNext && m_overTime > 3) // 3) 1.5초 뒤 페이드 아웃으로 전환
                {
                    m_shootGameNext = true;
                    GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => m_levelController.Change_Level((int)VisualNovelManager.LEVELSTATE.LS_NOVELEND), 0.5f, false);
                }
            }
        }
    }
}