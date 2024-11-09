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

    public bool Empty => m_empty;
    public WindowFile File => m_file;

    public FileIconSlot() : base()
    {
    }

    public override void Load_Scene()
    {
        Transform fileIconPanel = GameObject.Find("Canvas").transform.GetChild(2);
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Common/File_Slot", fileIconPanel);
        m_object.transform.GetChild(0).GetComponent<IconButton>().Set_Owner(this);

        m_iconImage = m_object.transform.GetChild(0).GetComponent<Image>();
        m_nameTxt = m_object.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();

        if (m_file != null)
            Add_FileIcon(m_file);
    }

    public void Add_FileIcon(WindowFile file)
    {
        m_empty = false;
        m_file = file;

        m_iconImage.sprite = GameManager.Ins.Window.Get_FileSprite(m_file.FileData.fileType);
        m_nameTxt.text = m_file.FileData.fileName;

        m_object.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnPointerClick()
    {
        if (m_file.FileData.fileAction == null)
            return;

        m_file.FileData.fileAction();
    }

    public void Remove_FileIcon()
    {
        m_empty = true;
        m_file = null;

        m_object.transform.GetChild(0).gameObject.SetActive(false);
    }
}
