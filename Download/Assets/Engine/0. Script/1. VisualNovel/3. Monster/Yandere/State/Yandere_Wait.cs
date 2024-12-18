using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace VisualNovel
{
    public class Yandere_Wait : State<HallwayYandere>
    {
        private Animator m_animator;
        private NavMeshAgent m_agent;
        private AudioSource m_audioSource;
        private bool m_switch = false;

        public Yandere_Wait(StateMachine<HallwayYandere> stateMachine) : base(stateMachine)
        {
            m_animator    = m_stateMachine.Owner.GetComponentInChildren<Animator>();
            m_agent       = m_stateMachine.Owner.GetComponent<NavMeshAgent>();
            m_audioSource = m_stateMachine.Owner.GetComponent<AudioSource>();
        }

        public override void Enter_State()
        {
            m_switch = false;

            m_animator.SetBool("IsRun", true);
            m_agent.speed = 10f;

            //GameManager.Instance.UI.Start_FadeOut(1f, Color.black, () => Continue_Play(), 0f, false);
            // VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().Change_Text("선배... 도망가게 두지 않을거에요.");
        }

        public override void Update_State()
        {
            // 특정 구역까지 이동 후 해당 위치에서 대기
            m_agent.destination = new Vector3(0f, 0f, 10f);

            // 전환
            if(m_switch == false && Camera.main.transform.position.z < 11.5f)
            {
                m_switch = true;
                GameManager.Ins.UI.Start_FadeOut(0.8f, Color.black, () => Continue_Play(), 0f, false);
            }

        }

        public override void Exit_State()
        {
            m_agent.speed = 5f;
        }

        private void Continue_Play() // 컷씬 재생 후 게임 재진행
        {
            // BGM 전환
            GameManager.Ins.Sound.Play_AudioSourceBGM("VisualNovel_ChaseBGM", true, 1f);

            // 얀데레 웃음소리
            GameManager.Ins.Sound.Play_ManagerAudioSource("VisualNovel_YandereSmile", false, 1f);

            m_audioSource.enabled = true;
            m_audioSource.Play();

            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_FOLLOW);
            Novel_Day3Chase novel_Chase = GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Day3Chase>();
            novel_Chase.Stage.transform.GetChild(0).gameObject.SetActive(true); // 미니맵 카메라
            novel_Chase.Stage.transform.GetChild(1).GetChild(0).gameObject.SetActive(true); // 미니맵 UI
            novel_Chase.ItemText.GetComponent<ItemText>().Start_ItemText("후후후...");
            novel_Chase.Player.Set_Lock(false);
            novel_Chase.Player.MoveSpeed = 400f;

            m_stateMachine.Change_State((int)HallwayYandere.YandereState.ST_CHASE);
            GameManager.Ins.UI.Start_FadeIn(1f, Color.black);
        }
    }
}


