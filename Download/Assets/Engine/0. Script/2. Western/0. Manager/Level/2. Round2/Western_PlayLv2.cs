using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Western
{
    public class Western_PlayLv2 : Western_Round2
    {
        private WalkPlayer m_player;

        protected int m_uiIndex = 0;
        protected GameObject m_readyGoUI = null;

        private PlayerPortrait  m_playerPortrait;
        private Gun             m_gun;
        private PlayerDialog    m_playerDialog;
        private PlayerMemo      m_PlayerMemo;
        private PlayerStatusBar m_playerStatus;

        public WalkPlayer Player => m_player;
        public PlayerPortrait PlayerPortrait => m_playerPortrait;
        public Gun Gun => m_gun;
        public PlayerDialog PlayerDialog => m_playerDialog;
        public PlayerMemo PlayerMemo => m_PlayerMemo;
        public PlayerStatusBar PlayerStatus => m_playerStatus;

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            base.Enter_Level();

            GameObject stage = GameManager.Ins.Western.Stage;
            m_player = stage.transform.GetChild(2).GetComponent<WalkPlayer>();
            m_player.Set_Lock(true);

            m_playerPortrait = stage.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<PlayerPortrait>();
            m_gun = stage.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Gun>();
            m_playerDialog = stage.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<PlayerDialog>();
            m_PlayerMemo = stage.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<PlayerMemo>();
            m_playerStatus = stage.transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<PlayerStatusBar>();

            // 카메라 설정
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_FOLLOW);
            CameraFollow camera = (CameraFollow)GameManager.Ins.Camera.Get_CurCamera();
            camera.Set_FollowInfo(m_player.transform, m_player.transform, false, false, new Vector3(0.0f, 1.3f, 0.0f), 0.0f, 0.0f, new Vector2(0f, 0f), false, false, true);

            GameManager.Ins.Western.IsShoot = false;
            Cursor.lockState = CursorLockMode.None;
            GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Play_Level());
        }

        public override void Play_Level() // 바로 시작
        {
            // 플레이어 조작 가능
            m_player.Set_Lock(false);

            // 주인공 대사 출력
            string[] texts = new string[2];
            texts[0] = "젠장... 여긴 어디지.";
            texts[1] = "말도 사라졌어. 지금 내 발이 땅에 닿아있다고!";
            m_playerDialog.Start_Dialog(texts, 1f);
        }

        public override void Update_Level()
        {
            if (GameManager.Ins.IsGame == false)
                return;


        }

        public override void LateUpdate_Level()
        {
        }

        public override void Exit_Level()
        {
        }
    }
}