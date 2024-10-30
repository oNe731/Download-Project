using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_FileDelete : Panel_Popup
{
    private RectTransform m_rectTransform;
    private TMP_Text m_text;

    public Panel_FileDelete() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_FILEDELETE;
    }

    protected override void Active_Event(bool active)
    {
        if(active == true)
        {
            m_rectTransform.anchoredPosition = new Vector2(0f, 0f);
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Folder/Panel_FolderDelete", canvas.GetChild(3));
        //m_object.SetActive(m_select);
        m_select = false; // 기본 비활성화
        m_object.SetActive(false);

        // 버튼 이벤트 추가
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region 기본 셋팅
        m_rectTransform = m_object.GetComponent<RectTransform>();
        m_text = m_object.transform.GetChild(3).GetComponent<TMP_Text>();
        m_object.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Button_Confirm());
        m_object.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => Button_Cancel());
        #endregion
    }

    public override void Update_Data()
    {
    }

    public override void Unload_Scene()
    {
    }

    public void Set_FileDelete(WindowFile windowFile)
    {
        if (m_text == null)
            return;

        m_text.text = Regex.Replace(m_text.text, @"{{(.*?)}}", windowFile.FileData.fileName);
    }

    private void Button_Confirm() // 확인(삭제)
    {
        //* 파일 삭제 및 휴지통으로 이동
        // 


        //* 딕셔너리에서 해당 키값 삭제 및 경로 변경 후 재추가
        //
    }

    private void Button_Cancel() // 취소
    {
        Active_Popup(false);
        GameManager.Ins.Window.Folder.Reset_SelectBox();
    }
}
