using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileIconSlot : WindowData
{
    private WindowManager.FILETYPE m_fileType = WindowManager.FILETYPE.TYPE_END;
    private string m_fileName;
    private Action m_action;

    private bool m_empty = true;

    private Image m_iconImage;
    private TMP_Text m_nameTxt;
    private Button m_button;

    public bool Empty => m_empty;

    public FileIconSlot() : base()
    {
    }

    public override void Load_Scene()
    {
        Transform fileIconPanel = GameObject.Find("Canvas").transform.GetChild(2);

        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Common/File_Slot", fileIconPanel);
        m_iconImage = m_object.transform.GetChild(0).GetComponent<Image>();
        m_nameTxt = m_object.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        m_button = m_object.transform.GetChild(0).GetComponent<Button>();

        if (m_fileType != WindowManager.FILETYPE.TYPE_END)
            Add_FileIcon(m_fileType, m_fileName, m_action);
    }

    public void Add_FileIcon(WindowManager.FILETYPE fileType, string fileName, Action action)
    {
        m_empty = false;
        m_fileType = fileType;
        m_fileName = fileName;
        m_action = action;

        // 이미지, 텍스트, 버튼 이벤트 등록
        m_iconImage.sprite = GameManager.Ins.Window.Get_FileSprite(m_fileType);
        m_nameTxt.text = m_fileName;
        if(m_action != null)
            m_button.onClick.AddListener(() => m_action());

        m_object.transform.GetChild(0).gameObject.SetActive(true);
    }

    public override void Update_Data()
    {

    }

    public override void Unload_Scene()
    {

    }
}
