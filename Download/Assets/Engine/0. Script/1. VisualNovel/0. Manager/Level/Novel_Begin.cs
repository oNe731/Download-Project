using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class Novel_Begin : Level
    {
        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            VisualNovelManager.Instance.Dialog.SetActive(true);
            VisualNovelManager.Instance.Dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Ins.Load_JsonData<DialogData_VN>("4. Data/1. VisualNovel/Dialog/Dialog1_School")); // 앞 경로 및 뒤 확장자 삭제

            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_BASIC_2D);
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
