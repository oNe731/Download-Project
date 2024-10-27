using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileIconSlot : WindowData
{
    private bool m_empty = true;
    private WindowFile m_file;

    private Image    m_iconImage;
    private TMP_Text m_nameTxt;
    private Button   m_button;

    public bool Empty => m_empty;
    public WindowFile File => m_file;

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

        if (m_file != null)
            Add_FileIcon(m_file);
    }

    public void Add_FileIcon(WindowFile file)
    {
        m_empty = false;
        m_file = file;

        // 이미지, 텍스트, 버튼 이벤트 등록
        m_iconImage.sprite = GameManager.Ins.Window.Get_FileSprite(m_file.FileData.fileType);
        m_nameTxt.text = m_file.FileData.fileName;
        if(m_file.Action != null)
            m_button.onClick.AddListener(() => m_file.Action());

        m_object.transform.GetChild(0).gameObject.SetActive(true);
    }

    public override void Update_Data()
    {
    }

    public override void Unload_Scene()
    {

    }
}
