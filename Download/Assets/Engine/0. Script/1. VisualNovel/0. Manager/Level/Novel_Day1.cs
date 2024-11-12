using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class Novel_Day1 : Novel_Level
    {
        private Dialogs_Day1 m_dialogAsset;

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            m_dialogAsset = GameManager.Ins.Resource.Load<ScriptableObject>("4. Data/1. VisualNovel/Dialogs/Dialogs_Day1") as Dialogs_Day1;

            VisualNovelManager manager = GameManager.Ins.Novel;
            manager.Dialog.SetActive(true);
            Dialog_VN dialog = manager.Dialog.GetComponent<Dialog_VN>();
            dialog.Reset_Skip();
            dialog.Start_Dialog(0);

            //GameManager.Ins.Sound.Play_AudioSourceBGM("VisualNovel_ScriptBGM", true, 1f);
            GameManager.Ins.Sound.Stop_AudioSourceBGM();
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
                    sheetList = m_dialogAsset.S00_1_SchoolGo;
                    break;
                case 1:
                    sheetList = m_dialogAsset.S01_2_SchoolHallway;
                    break;
                case 2:
                    sheetList = m_dialogAsset.S02_3_BandRoom;
                    break;
                case 3:
                    sheetList = m_dialogAsset.S03_31_Base;
                    break;
                case 4:
                    sheetList = m_dialogAsset.S04_32_Electric;
                    break;
                case 5:
                    sheetList = m_dialogAsset.S05_33_Piano;
                    break;
                case 6:
                    sheetList = m_dialogAsset.S06_3_BandRoom2;
                    break;
                case 7:
                    sheetList = m_dialogAsset.S07_4_SchoolDrop;
                    break;
                case 8:
                    sheetList = m_dialogAsset.S08_41_Class;
                    break;
                case 9:
                    sheetList = m_dialogAsset.S09_411_What;
                    break;
                case 10:
                    sheetList = m_dialogAsset.S10_412_Thanks;
                    break;
                case 11:
                    sheetList = m_dialogAsset.S11_413_Like;
                    break;
                case 12:
                    sheetList = m_dialogAsset.S12_41_Class2;
                    break;
                case 13:
                    sheetList = m_dialogAsset.S13_42_Band;
                    break;
                case 14:
                    sheetList = m_dialogAsset.S14_421_Effort;
                    break;
                case 15:
                    sheetList = m_dialogAsset.S15_422_Fan;
                    break;
                case 16:
                    sheetList = m_dialogAsset.S16_423_Sing;
                    break;
                case 17:
                    sheetList = m_dialogAsset.S17_42_Band2;
                    break;
                case 18:
                    sheetList = m_dialogAsset.S18_43_Home;
                    break;
                case 19:
                    sheetList = m_dialogAsset.S19_431_Good;
                    break;
                case 20:
                    sheetList = m_dialogAsset.S20_432_Same;
                    break;
                case 21:
                    sheetList = m_dialogAsset.S21_433_Piano;
                    break;
                case 22:
                    sheetList = m_dialogAsset.S22_43_Home2;
                    break;
                case 23:
                    sheetList = m_dialogAsset.S23_4_SchoolDrop2;
                    break;
            }
            return sheetList;
        }
    }
}
