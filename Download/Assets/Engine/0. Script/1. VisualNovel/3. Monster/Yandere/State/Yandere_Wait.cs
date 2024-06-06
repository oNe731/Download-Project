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

        public Yandere_Wait(StateMachine<HallwayYandere> stateMachine) : base(stateMachine)
        {
            m_animator = m_stateMachine.Owner.GetComponentInChildren<Animator>();
            m_agent = m_stateMachine.Owner.GetComponent<NavMeshAgent>();
        }

        public override void Enter_State()
        {
            m_animator.SetBool("IsRun", true); // 애니메이션 변경
            m_agent.speed = 10f;
            UIManager.Instance.Start_FadeOut(1f, Color.black, () => Continue_Play(), 0f, false);
        }

        public override void Update_State()
        {
            // 특정 구역까지 이동 후 해당 위치에서 대기
            m_agent.destination = new Vector3(0f, 0f, 10f);
        }

        public override void Exit_State()
        {
            m_agent.speed = 5f;
        }

        private void Continue_Play() // 컷씬 재생 후 게임 재진행
        {
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_FOLLOW);
            VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().Player.Set_Lock(false);

            // 컷 씬 재생 완료 후 주인공 시점으로 돌아왔을 때 "후후후..." 대사 출력
            VisualNovelManager.Instance.PlayerObj.GetComponent<HallwayPlayer>().MoveSpeed = 400f;
            VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().ItemText.GetComponent<ItemText>().Start_ItemText("후후후...");

            m_stateMachine.Change_State((int)HallwayYandere.YandereState.ST_CHASE);
            UIManager.Instance.Start_FadeIn(1f, Color.black);
        }
    }
}


