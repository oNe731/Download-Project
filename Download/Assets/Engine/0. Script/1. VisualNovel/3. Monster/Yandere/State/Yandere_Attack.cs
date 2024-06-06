using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VisualNovel
{
    public class Yandere_Attack : State<HallwayYandere>
    {
        private Animator m_handAnimator = null;
        private CameraCutscene m_camera = null;
        private GameObject m_redPanel = null;

        private float m_time = 0f;
        private bool m_fadeOut = false;

        public Yandere_Attack(StateMachine<HallwayYandere> stateMachine) : base(stateMachine)
        {

        }

        public override void Enter_State()
        {
            VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().Fail_ChaseGame();

            // 게임 실패 : 얀데레한테 잡힐 시 컷씬 진행 후 복도 시작부터 다시 시작 (재도전 UI 출력)
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            m_camera = (CameraCutscene)CameraManager.Instance.Get_CurCamera();
            m_camera.Start_FOV(10f, 20f);

            // 손 생성
            GameObject handObject = GameManager.Instance.Create_GameObject("5. Prefab/1. VisualNovel/Character/YandereHand", Camera.main.transform);
            handObject.transform.localPosition = new Vector3(0f, 0f, 9f);
            m_handAnimator = handObject.transform.GetChild(0).GetComponent<Animator>();
        }

        public override void Update_State()
        {
            if (m_redPanel == null)
            {
                if (m_camera != null && Camera.main.fieldOfView <= 25f && m_handAnimator != null && m_handAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !m_handAnimator.IsInTransition(0))
                {
                    m_redPanel = GameManager.Instance.Create_GameObject("5. Prefab/1. VisualNovel/UI/Panel_Red", GameObject.Find("Canvas").transform);
                }
            }
            else
            {
                m_time += Time.deltaTime;
                if(m_fadeOut == false && m_time > 1f)
                {
                    m_fadeOut = true;
                    UIManager.Instance.Start_FadeOut(1f, Color.black, () => SceneManager.LoadScene("Window"), 1f, false);
                }
            }
        }

        public override void Exit_State()
        {
        }
    }
}

