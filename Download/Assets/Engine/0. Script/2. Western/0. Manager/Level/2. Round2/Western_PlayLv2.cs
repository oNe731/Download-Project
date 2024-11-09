using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Western
{
    public class Western_PlayLv2 : Western_Round2
    {
        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            base.Enter_Level();
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