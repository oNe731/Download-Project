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
            GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Day3Chase>().Fail_ChaseGame();
        }

        public override void Update_State()
        {
        }

        public override void Exit_State()
        {
        }
    }
}

