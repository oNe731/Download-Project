using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Western
{
    public class Western_MainLv1 : Western_Main
    {
        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            base.Enter_Level();

            GameManager.Ins.Sound.Play_AudioSourceBGM("Western_WantedBGM", true, 1f);
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
            base.Update_Level();
        }

        public override void LateUpdate_Level()
        {
            base.LateUpdate_Level();
        }

        public override void Exit_Level()
        {
            base.Exit_Level();
        }


        protected override void Start_Dialog()
        {
            m_dialogStart = true;
            GameManager.Ins.Western.DialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(true, GameManager.Ins.Load_JsonData<DialogData_PlayWT>("4. Data/2. Western/Dialog/Play/Round1/Dialog1_Main"));
        }
    }
}


