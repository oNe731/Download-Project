using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileIconSlot : WindowData
{
    private bool m_empty = true;
    private bool m_isClickState = true;
    private WindowFile m_file;

    private Image    m_iconImage;
    private TMP_Text m_nameTxt;

    public bool Empty => m_empty;
    public bool IsClickState { get => m_isClickState; set => m_isClickState = value; }
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
        Update_FileIconState();

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
        if (m_object.transform.childCount > 1)
        {
            for (int i = m_object.transform.childCount - 1; i >= 1; i--)
            {
                GameObject child = m_object.transform.GetChild(i).gameObject;
                GameManager.Ins.Resource.Destroy(child);
            }
        }
    }

    public void Update_FileIconState()
    {
        switch(m_file.FileData.fileType)
        {
            case WindowManager.FILETYPE.TYPE_MESSAGE:
                // 초반 한번 관련 프리팹 생성 및 위치 설정
                if (m_object.transform.childCount <= 1)
                    GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Common/Message_Count", m_object.transform);

                // 개수에 따른 개수 설정
                int count = GameManager.Ins.Window.Message.UnreadCount;
                if(count > 0)
                {
                    m_object.transform.GetChild(1).gameObject.SetActive(true);
                    m_object.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = GameManager.Ins.Window.Message.UnreadCount.ToString();
                }
                else
                {
                    m_object.transform.GetChild(1).gameObject.SetActive(false);
                }
                break;
        }
    }
}
