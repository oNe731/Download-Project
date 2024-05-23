using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Western_PlayLv2 : Western_Play
    {
        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);

            m_criminalText.Add("2캬옹!!!");
            m_criminalText.Add("2으악!");
            m_criminalText.Add("2크흑...숨길 수 있었는데");
            m_criminalText.Add("2분하다옹...!");
            m_criminalText.Add("2야오옹...");
            m_criminalText.Add("2젠장, 들켰다옹");
            m_criminalText.Add("2고양이의 수치다냥...");
            m_criminalText.Add("2내 사랑스런 수염이!");
            m_criminalText.Add("2내 은신을 간파하다니...");
            m_criminalText.Add("2당신은 전설의...!");

            m_citizenText.Add("2후후후...");
            m_citizenText.Add("2선량한 시민을 고르셨군");
            m_citizenText.Add("2하하! 멍청한 녀석이다옹");
            m_citizenText.Add("2없던 수전증이라도 생겼냐옹?");
            m_citizenText.Add("2두 눈 똑바로 뜨라옹");
            m_citizenText.Add("2어이! 나 여기있다옹");
            m_citizenText.Add("2내 움직임을 얏본 대가다옹");
            m_citizenText.Add("2흥, 별거없군");
            m_citizenText.Add("2고양이 앞에서 한 눈을 팔다니");
            m_citizenText.Add("2고양이 목숨이 몇개인지 알아?");
        }

        public override void Enter_Level()
        {
            base.Enter_Level();

            // 스테이지 생성
            m_stage = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/2Stage/2Stage"));
            m_groups = m_stage.transform.Find("Group").GetComponent<Groups>();

            // 카메라 설정
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_BASIC_3D);
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            CameraCutscene camera = (CameraCutscene)CameraManager.Instance.Get_CurCamera();
            camera.Change_Position(new Vector3(0f, 0.62f, -55.65f));
            camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));

            UIManager.Instance.Start_FadeIn(1f, Color.black, () => StartCoroutine(Update_ReadyGo()));
        }

        public override void Play_Level()
        {
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_WALK);
            m_camera = (CameraWalk)CameraManager.Instance.Get_CurCamera();
            m_camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));
            m_camera.Set_Height(0.62f);

            Proceed_Next();

            // BGM 변경
            Camera.main.GetComponent<AudioSource>().clip = Instantiate(Resources.Load<AudioClip>("2. Sound/BGM/Silencios de Los Angeles - Cumbia Deli"));
            Camera.main.GetComponent<AudioSource>().Play();
        }

        public override void Update_Level()
        {
            base.Update_Level();
        }

        public override void LateUpdate_Level()
        {
        }

        public override void Exit_Level()
        {
            base.Exit_Level();
        }

        public override void Play_Finish()
        {
            // BGM 변경
            Camera.main.GetComponent<AudioSource>().clip = Instantiate(Resources.Load<AudioClip>("2. Sound/BGM/La Docerola - Quincas Moreira"));
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }
}

