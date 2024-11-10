using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Western
{
    public class Western_PlayLv2 : Western_Round2
    {
        private WalkPlayer m_player;

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            base.Enter_Level();

            m_player = GameManager.Ins.Western.Stage.transform.GetChild(2).GetComponent<WalkPlayer>();

            // 카메라 설정
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_FOLLOW);
            CameraFollow camera = (CameraFollow)GameManager.Ins.Camera.Get_CurCamera();
            camera.Set_FollowInfo(m_player.transform, m_player.transform, false, false, new Vector3(0.0f, 1.3f, 0.0f), 0.0f, 0.0f, new Vector2(0f, 0f), false, false, true);

            //GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Start_Dialog());

            //Transform positiontarget, Transform rotationtarget, bool isPosition, bool isRotation, Vector3 offset, float moveSpeed, float lerpSpeed, Vector2 rotationLimit, bool isXRotate, bool isYRotate, bool update = false)
        }

        public override void Play_Level() // 튜토리얼 진행 후 Ready Go UI 출력 후 해당 함수 호출
        {
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