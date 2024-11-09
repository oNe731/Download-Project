using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_RecycleBinSub : Panel_Popup
{
    private TMP_Text m_frameText;
    private Image    m_iconImage;
    private TMP_Text m_detailText;
    private TMP_Text m_confirmText;
    private TMP_Text m_cancelText;

    private Sprite m_warningSprite;

    public Panel_RecycleBinSub() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_TRASHSUB;

        m_warningSprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/0. Window/Window Recycle bin/UI_Window_RecycleBin_WarningIcon");
    }

    protected override void Active_Event(bool active)
    {
        if(active == true)
        {
            switch(m_activeType)
            {
                case (int)Panel_RecycleBin.BUTTONTYPE.BT_DELETE:
                    WindowManager WMD = GameManager.Ins.Window;
                    WindowFile windowFileD = WMD.Recyclebin.SelectFolderBoxs[0].FileData;

                    m_frameText.text = "선택 항목 삭제";
                    m_iconImage.sprite = WMD.Get_FileSprite(windowFileD.FileData.fileType);
                    m_detailText.text = $"정말 '{windowFileD.FileData.fileName}'을(를) 삭제하시겠습니까?\n이 파일은 휴지통에서 영원히 사라지게 되며, 복구가 불가능합니다.";
                    m_confirmText.text = "삭제";
                    m_cancelText.text = "취소";
                    break;

                case (int)Panel_RecycleBin.BUTTONTYPE.BT_RESTORE:
                    //WindowManager WMR = GameManager.Ins.Window;
                    //WindowFile windowFileR = WMR.Recyclebin.SelectFolderBoxs[0].FileData;

                    //m_frameText.text = "선택 항목 복구";
                    //m_iconImage.sprite = WMR.Get_FileSprite(windowFileR.FileData.fileType);
                    //m_detailText.text = $"정말 '{windowFileR.FileData.fileName}'을(를) 복구하시겠습니까?";
                    //m_confirmText.text = "복구";
                    //m_cancelText.text = "취소";r
                    break;

                case (int)Panel_RecycleBin.BUTTONTYPE.BT_DELETEALL:
                    m_frameText.text = "여러 항목 삭제";
                    m_iconImage.sprite = m_warningSprite;
                    m_detailText.text = "선택한 항목을 모두 영구 삭제하시겠습니까?";
                    m_confirmText.text = "삭제";
                    m_cancelText.text = "취소";
                    break;

                case (int)Panel_RecycleBin.BUTTONTYPE.BT_RESTOREALL:
                    m_frameText.text = "여러 항목 복구";
                    m_iconImage.sprite = m_warningSprite;
                    m_detailText.text = "선택한 항목을 모두 복구하시겠습니까?";
                    m_confirmText.text = "복구";
                    m_cancelText.text = "취소";
                    break;
            }
        }
        else
        {
            GameManager.Ins.Window.Recyclebin.Reset_SelectBox();
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Recycle/Panel_RecycleBinSub", canvas.GetChild(3));
        m_select = false;
        m_object.SetActive(m_select);

        // 버튼 이벤트 추가
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region 기본 셋팅
        m_object.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Button_Confirm());// 확인
        m_object.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => Button_Cancel()); // 취소

        m_frameText   = m_object.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>(); // 프레임 이름 변경
        m_iconImage   = m_object.transform.GetChild(2).GetComponent<Image>();                // 아이콘 이미지 변경
        m_detailText  = m_object.transform.GetChild(3).GetComponent<TMP_Text>();             // 내용 변경
        m_confirmText = m_object.transform.GetChild(4).GetChild(0).GetComponent<TMP_Text>(); // 확인 텍스트
        m_cancelText  = m_object.transform.GetChild(5).GetChild(0).GetComponent<TMP_Text>(); // 취소 텍스트
        #endregion
    }

    public void Button_Confirm()
    {
        GameManager.Ins.Window.Recyclebin.Button_Event((Panel_RecycleBin.BUTTONTYPE)m_activeType);
    }

    public void Button_Cancel()
    {
        GameManager.Ins.Window.Recyclebin.Reset_SelectBox();
        Active_Popup(false);
    }
}
