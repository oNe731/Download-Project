using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class Novel_Day3After : Novel_Level
    {
        private Dialogs_Day3At m_dialogAsset;

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            m_dialogAsset = GameManager.Ins.Resource.Load<ScriptableObject>("4. Data/1. VisualNovel/Dialogs/Dialogs_Day3At") as Dialogs_Day3At;

            VisualNovelManager manager = GameManager.Ins.Novel;
            manager.Dialog.SetActive(true);
            switch (m_levelController.Get_Level<Novel_Day3Shoot>((int)VisualNovelManager.LEVELSTATE.LS_DAY3SHOOTGAME).DollType)
            {
                case DOLLTYPE.DT_FAIL:
                    manager.Dialog.GetComponent<Dialog_VN>().Start_Dialog(0);
                    break;

                case DOLLTYPE.DT_BIRD:
                    manager.Dialog.GetComponent<Dialog_VN>().Start_Dialog(1);
                    break;

                case DOLLTYPE.DT_CAT:
                    manager.Dialog.GetComponent<Dialog_VN>().Start_Dialog(2);
                    break;

                case DOLLTYPE.DT_SHEEP:
                    manager.Dialog.GetComponent<Dialog_VN>().Start_Dialog(3);
                    break;
            }

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
                    sheetList = m_dialogAsset.S00_11_Fail;
                    break;
                case 1:
                    sheetList = m_dialogAsset.S01_12_Bird;
                    break;
                case 2:
                    sheetList = m_dialogAsset.S02_13_Cat;
                    break;
                case 3:
                    sheetList = m_dialogAsset.S03_14_Sheep;
                    break;
                case 4:
                    sheetList = m_dialogAsset.S04_1_After;
                    break;
                case 5:
                    sheetList = m_dialogAsset.S05_1_Festival;
                    break;
                case 6:
                    sheetList = m_dialogAsset.S06_2_Home;
                    break;
                case 7:
                    sheetList = m_dialogAsset.S07_3_Restroom;
                    break;
            }
            return sheetList;
        }
    }
}

