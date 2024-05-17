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
            VisualNovelManager.Instance.Dialog.SetActive(true);
            switch (m_levelController.Get_Level<Novel_Shoot>((int)VisualNovelManager.LEVELSTATE.LS_SHOOTGAME).DollType)
            {
                case DOLLTYPE.DT_BIRD:
                    VisualNovelManager.Instance.Dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>("Assets/Resources/4. Data/1. VisualNovel/Dialog/Dialog2_DollBird.json"));
                    break;

                case DOLLTYPE.DT_SHEEP:
                    VisualNovelManager.Instance.Dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>("Assets/Resources/4. Data/1. VisualNovel/Dialog/Dialog2_DollSheep.json"));
                    break;

                case DOLLTYPE.DT_CAT:
                    VisualNovelManager.Instance.Dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>("Assets/Resources/4. Data/1. VisualNovel/Dialog/Dialog2_DollCat.json"));
                    break;

                case DOLLTYPE.DT_FAIL:
                    VisualNovelManager.Instance.Dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>("Assets/Resources/4. Data/1. VisualNovel/Dialog/Dialog2_DollFail.json"));
                    break;
            }

            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_BASIC_2D);
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                VisualNovelManager.Instance.Active_Popup();
        }

        public override void Exit_Level()
        {
        }

        public override void OnDrawGizmos()
        {
        }
    }
}

