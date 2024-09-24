using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class Novel_End : Level
    {
        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            VisualNovelManager manager = GameManager.Ins.Novel;

            manager.Dialog.SetActive(true);
            switch (m_levelController.Get_Level<Novel_Shoot>((int)VisualNovelManager.LEVELSTATE.LS_SHOOTGAME).DollType)
            {
                case DOLLTYPE.DT_BIRD:
                    manager.Dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Ins.Load_JsonData<DialogData_VN>("4. Data/1. VisualNovel/Dialog/Dialog4_DollBird"));
                    break;

                case DOLLTYPE.DT_SHEEP:
                    manager.Dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Ins.Load_JsonData<DialogData_VN>("4. Data/1. VisualNovel/Dialog/Dialog4_DollSheep"));
                    break;

                case DOLLTYPE.DT_CAT:
                    manager.Dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Ins.Load_JsonData<DialogData_VN>("4. Data/1. VisualNovel/Dialog/Dialog4_DollCat"));
                    break;

                case DOLLTYPE.DT_FAIL:
                    manager.Dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Ins.Load_JsonData<DialogData_VN>("4. Data/1. VisualNovel/Dialog/Dialog4_DollFail"));
                    break;
            }

            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_BASIC_2D);
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                GameManager.Ins.Novel.Active_Popup();
        }

        public override void Exit_Level()
        {
        }

        public override void OnDrawGizmos()
        {
        }
    }
}

