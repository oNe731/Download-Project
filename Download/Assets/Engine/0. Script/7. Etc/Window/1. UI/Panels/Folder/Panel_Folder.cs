using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Folder : Panel_Popup
{
    // 즐겨찾기 버튼
    [SerializeField] private GameObject[] m_bookmarkPopups;
    private bool m_bookmarkActive = false;
    //*

    // 주소 입력


    // 파일 삭제


    private void Start()
    {
        
    }

    private void Update()
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
