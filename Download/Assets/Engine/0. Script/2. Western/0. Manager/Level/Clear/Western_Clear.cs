using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Western_Clear : Level
    {
        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            WesternManager.Instance.MainPanel.SetActive(true);
            UIManager.Instance.Start_FadeIn(1f, Color.black);
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
        }

        public override void LateUpdate_Level()
        {
        }

        public override void Exit_Level()
        {
            WesternManager.Instance.MainPanel.SetActive(false);
        }
    }
}

