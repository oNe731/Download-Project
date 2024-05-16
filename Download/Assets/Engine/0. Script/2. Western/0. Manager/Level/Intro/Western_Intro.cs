using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Western_Intro : Level
    {
        public Western_Intro(LevelController levelController) : base(levelController)
        {
        }

        public override void Enter_Level()
        {
            WesternManager.Instance.IntroPanel.SetActive(true);
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
        }

        public override void Exit_Level()
        {
            WesternManager.Instance.IntroPanel.SetActive(false);
        }

        protected void Start_Dialog(string path)
        {
            WesternManager.Instance.DialogIntro.GetComponent<Dialog_IntroWT>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_IntroWT>(path));
        }
    }
}

