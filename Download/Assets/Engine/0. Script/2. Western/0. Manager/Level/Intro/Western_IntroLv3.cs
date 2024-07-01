using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Western_IntroLv3 : Western_Intro
    {
        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            base.Enter_Level();
            Start_Dialog("4. Data/2. Western/Dialog/Intro/Dialog3_Intro1");
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
        }

        public override void Exit_Level()
        {
            base.Exit_Level();
        }
    }
}

