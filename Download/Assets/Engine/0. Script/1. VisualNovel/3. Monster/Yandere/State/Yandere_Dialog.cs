using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class Yandere_Dialog : State<HallwayYandere>
    {
        private Animator m_animator;

        public Yandere_Dialog(StateMachine<HallwayYandere> stateMachine) : base(stateMachine)
        {
            m_animator = m_stateMachine.Owner.GetComponentInChildren<Animator>();
        }

        public override void Enter_State()
        {
        }

        public override void Update_State()
        {
        }

        public override void Exit_State()
        {
        }
    }
}

