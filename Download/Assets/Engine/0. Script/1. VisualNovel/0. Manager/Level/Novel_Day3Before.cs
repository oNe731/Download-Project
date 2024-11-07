using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class Novel_Day3Before : Novel_Level
    {
        private Dialogs_Day3Be m_dialogAsset;

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            m_dialogAsset = GameManager.Ins.Resource.Load<ScriptableObject>("4. Data/1. VisualNovel/Dialogs/Dialogs_Day3Be") as Dialogs_Day3Be;
            VisualNovelManager manager = GameManager.Ins.Novel;

            manager.Dialog.SetActive(true);
            Dialog_VN dialog = manager.Dialog.GetComponent<Dialog_VN>();
            dialog.Reset_Skip();
            dialog.Start_Dialog(0);

            GameManager.Ins.Sound.Play_AudioSourceBGM("VisualNovel_ScriptBGM", true, 1f);
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
            m_dialogAsset = null;
        }

        public override void OnDrawGizmos()
        {
        }

        public override List<ExcelData> Get_DialogData(int sheetIndex)
        {
            List<ExcelData> sheetList = null;
            switch (sheetIndex)
            {
                case 0:
                    sheetList = m_dialogAsset.S00_1_Road;
                    break;
                case 1:
                    sheetList = m_dialogAsset.S01_2_SchoolAfter;
                    break;
                case 2:
                    sheetList = m_dialogAsset.S02_21_Dango;
                    break;
                case 3:
                    sheetList = m_dialogAsset.S03_22_Yakitori;
                    break;
                case 4:
                    sheetList = m_dialogAsset.S04_23_Puding;
                    break;
                case 5:
                    sheetList = m_dialogAsset.S05_2_SchoolAfter2;
                    break;
            }
            return sheetList;
        }
    }
}
