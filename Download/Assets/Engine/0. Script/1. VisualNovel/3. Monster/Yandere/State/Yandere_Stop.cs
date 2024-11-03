using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace VisualNovel
{
    public class Yandere_Stop : State<HallwayYandere>
    {
        private float m_time = 0f;
        private float m_maxTime = 5f;

        private GameObject m_minimapIcon;
        private GameObject m_stopLight;
        private Animator m_animator;
        private NavMeshAgent m_agent;

        public Yandere_Stop(StateMachine<HallwayYandere> stateMachine, GameObject minimapIcon, GameObject stopLight) : base(stateMachine)
        {
            m_minimapIcon = minimapIcon;
            m_stopLight = stopLight;

            m_animator = m_stateMachine.Owner.GetComponentInChildren<Animator>();
            m_agent = m_stateMachine.Owner.GetComponent<NavMeshAgent>();
        }

        public override void Enter_State()
        {
            m_time = 0.0f;
            
            m_minimapIcon.SetActive(true);
            m_stopLight.SetActive(true);

            if (m_agent.gameObject.activeSelf == true)
                m_agent.destination = m_agent.transform.position;

            m_animator.speed = 0f;

            // 파티클 생성
        }

        public override void Update_State()
        {
            m_time += Time.deltaTime;
            if (m_time > m_maxTime)
                m_stateMachine.Change_State(m_stateMachine.PreState);
        }

        public override void Exit_State()
        {
            m_minimapIcon.SetActive(false);
            m_stopLight.SetActive(false);

            m_animator.speed = 1f;
        }
    }
}