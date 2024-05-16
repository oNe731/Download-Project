using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class Yandere_Attack : State<HallwayYandere>
    {
        public Yandere_Attack(StateMachine<HallwayYandere> stateMachine) : base(stateMachine)
        {

        }

        public override void Enter_State()
        {
            // 공격 진행
        }

        public override void Update_State()
        {
        }

        public override void Exit_State()
        {
        }
    }
}

