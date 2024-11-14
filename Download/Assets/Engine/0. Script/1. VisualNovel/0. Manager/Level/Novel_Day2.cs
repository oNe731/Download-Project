using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class Novel_Day2 : Novel_Level
    {
        private Dialogs_Day2 m_dialogAsset;

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            m_dialogAsset = GameManager.Ins.Resource.Load<ScriptableObject>("4. Data/1. VisualNovel/Dialogs/Dialogs_Day2") as Dialogs_Day2;

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
                    sheetList = m_dialogAsset.S01_2_BandRoom;
                    break;
                case 2:
                    sheetList = m_dialogAsset.S02_21_LMinatsu;
                    break;
                case 3:
                    sheetList = m_dialogAsset.S03_211_LHina;
                    break;
                case 4:
                    sheetList = m_dialogAsset.S04_212_LAyaka;
                    break;
                case 5:
                    sheetList = m_dialogAsset.S05_213_LState;
                    break;
                case 6:
                    sheetList = m_dialogAsset.S06_21_LMinatsu2;
                    break;
                case 7:
                    sheetList = m_dialogAsset.S07_2111_LHina;
                    break;
                case 8:
                    sheetList = m_dialogAsset.S08_2122_LAyaka;
                    break;
                case 9:
                    sheetList = m_dialogAsset.S09_2133_LMinatsu;
                    break;
                case 10:
                    sheetList = m_dialogAsset.S10_22_LHina;
                    break;
                case 11:
                    sheetList = m_dialogAsset.S11_221_LPicture;
                    break;
                case 12:
                    sheetList = m_dialogAsset.S12_222_LWhat;
                    break;
                case 13:
                    sheetList = m_dialogAsset.S13_22_LHina2;
                    break;
                case 14:
                    sheetList = m_dialogAsset.S14_2211_LSorry;
                    break;
                case 15:
                    sheetList = m_dialogAsset.S15_2222_LUnderstand;
                    break;
                case 16:
                    sheetList = m_dialogAsset.S16_2233_LTalk;
                    break;
                case 17:
                    sheetList = m_dialogAsset.S17_23_LAyaka;
                    break;
                case 18:
                    sheetList = m_dialogAsset.S18_231_LHina;
                    break;
                case 19:
                    sheetList = m_dialogAsset.S19_232_LMean;
                    break;
                case 20:
                    sheetList = m_dialogAsset.S20_23_LAyaka2;
                    break;
                case 21:
                    sheetList = m_dialogAsset.S21_2311_LSorry;
                    break;
                case 22:
                    sheetList = m_dialogAsset.S22_2322_Method;
                    break;
                case 23:
                    sheetList = m_dialogAsset.S23_2333_Mediation;
                    break;
                case 24:
                    sheetList = m_dialogAsset.S24_2_BandRoom2;
                    break;
                case 25:
                    sheetList = m_dialogAsset.S25_3_SchoolLeave;
                    break;
            }
            return sheetList;
        }
    }
}
