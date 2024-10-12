using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Folder : Panel_Popup
{
    // 즐겨찾기 버튼
    private GameObject[] m_bookmarkPopups;
    private bool m_bookmarkActive = false;
    //*

    // 주소 입력


    // 파일 삭제


    public Panel_Folder() : base()
    {
        m_fileType = FILETYPE.TYPE_FOLDER;
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Panels/Panel_Folder", canvas.GetChild(2).GetChild(2));
        m_object.SetActive(m_select);

        // 버튼 이벤트 추가
        canvas.GetChild(2).GetChild(1).Find("Button_Folder").GetComponent<Button>().onClick.AddListener(() => Active_Popup(true));
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));
    }

    public override void Update_Data()
    {

    }

    public override void Unload_Scene()
    {

    }


    public void Button_Bookmark() // 즐겨찾기 버튼
    {
        for(int i = 0; i < m_bookmarkPopups.Length; ++i)
        {
            m_bookmarkPopups[i].SetActive(!m_bookmarkActive);
        }
        m_bookmarkActive = !m_bookmarkActive;
    }

    public void Button_InputFieldDown()
    {

    }
}
