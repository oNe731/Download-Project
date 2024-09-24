using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Western_Intro : Level
    {
        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            GameManager.Ins.Western.DialogIntro.gameObject.SetActive(true);
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
        }

        public override void Exit_Level()
        {
            GameManager.Ins.Western.DialogIntro.gameObject.SetActive(false);
        }

        protected void Start_Dialog(string path)
        {
            GameManager.Ins.Western.DialogIntro.GetComponent<Dialog_IntroWT>().Start_Dialog(GameManager.Ins.Load_JsonData<DialogData_IntroWT>(path));
        }
    }
}

