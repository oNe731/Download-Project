using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class HallwayYandere : MonoBehaviour
    {
        // ´ÙÀÌ¾ó·Î±×ÄÆ¾À -> µîÀåÄÆ¾À -> ´ë±â»óÅÂ -> Ãß°Ý»óÅÂ -> °ø°Ý»óÅÂ // ¸ØÃã»óÅÂ
        public enum YandereState { ST_DIALOG, ST_APPEAR, ST_WAIT, ST_CHASE, ST_ATTCK, ST_STOP, ST_END }

        [SerializeField] private GameObject m_minimapIcon;
        [SerializeField] private GameObject m_stopLight;

        private StateMachine<HallwayYandere> m_stateMachine;
        public StateMachine<HallwayYandere> StateMachine => m_stateMachine;

        private void Start()
        {
            m_stateMachine = new StateMachine<HallwayYandere>(gameObject);

            List<State<HallwayYandere>> states = new List<State<HallwayYandere>>();
            states.Add(new Yandere_Dialog(m_stateMachine)); // 0
            states.Add(new Yandere_Appear(m_stateMachine)); // 1
            states.Add(new Yandere_Wait(m_stateMachine));   // 2
            states.Add(new Yandere_Chase(m_stateMachine));  // 3
            states.Add(new Yandere_Attack(m_stateMachine)); // 4
            states.Add(new Yandere_Stop(m_stateMachine, m_minimapIcon, m_stopLight));   // 5

            m_stateMachine.Initialize_State(states, (int)YandereState.ST_DIALOG);
        }

        private void Update()
        {
            m_stateMachine.Update_State();
        }

        public void Used_Lever()
        {
            m_stateMachine.Change_State((int)YandereState.ST_STOP);
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            m_stateMachine.OnDrawGizmos();
#endif
        }
    }
}

