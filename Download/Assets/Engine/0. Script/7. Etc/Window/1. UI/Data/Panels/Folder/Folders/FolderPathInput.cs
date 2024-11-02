using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FolderPathInput : MonoBehaviour
{
    private TMP_InputField m_pathText;
    private bool m_inputPath = false;
    private GameObject m_dropDownPanel;
    private FolderDropdownPath[] m_dropPath;

    public GameObject DropDownPanel => m_dropDownPanel;

    private void Start()
    {
        m_pathText = transform.GetChild(1).GetComponent<TMP_InputField>();

        m_pathText.onSelect.AddListener(Input_Start);
        m_pathText.onValueChanged.AddListener(Input_Changed);
        m_pathText.onEndEdit.AddListener(Input_End);

        m_dropDownPanel = GameManager.Ins.Window.Folder.DropDownPanel;
        m_dropPath = new FolderDropdownPath[m_dropDownPanel.transform.childCount];
        for(int i = 0; i < m_dropDownPanel.transform.childCount; ++i)
        {
            m_dropPath[i] = m_dropDownPanel.transform.GetChild(i).GetComponent<FolderDropdownPath>();
            m_dropPath[i].Initialize_Dropdown(this);
        }
    }

    private void Input_Start(string text)
    {
        WindowManager WM = GameManager.Ins.Window;
        WM.Folder.FavoriteTransform.gameObject.SetActive(false);

        // 주소형식 변경 : \
        string path;
        if(m_inputPath == false)
        {
            m_inputPath = true;
            path = WM.Folder.Path;
        }
        else
        {
            path = WM.Folder.Set_RestorPathFormat(m_pathText.text);
        }

        m_pathText.text = path;
        m_dropDownPanel.SetActive(false);
    }

    private void Input_Changed(string text)
    {
    }

    private void Input_End(string text)
    {
        // 주소 형식 복구 : >
        m_pathText.text = GameManager.Ins.Window.Folder.Set_PathFormat(m_pathText.text);
    }

    public void Button_DropDwon() // 최근 경로 2개 출력
    {
        WindowManager WM = GameManager.Ins.Window;
        WM.Folder.FavoriteTransform.gameObject.SetActive(false);
        if (WM.Folder.IsButtonClick == false)
            return;

        if (m_dropDownPanel.activeSelf == false)
        {
            int createCount = 0;

            // 이전 파일이 있으면 해당 파일 경로부터 출력
            Stack<int> prev = new Stack<int>(WM.Folder.PreviousFileIndex);
            int prevCount = prev.Count;
            for (int i = 0; i < prevCount; ++i)
            {
                int fildeIndex = prev.Pop();
                if(fildeIndex == 0) // 배경화면
                {
                    m_dropPath[createCount].Set_Dropdown(FolderDropdownPath.STATE.ST_BEFORE, 2 - createCount, WM.BackgroundPath);

                    createCount++;
                    if (createCount >= 2)
                    {
                        m_dropDownPanel.SetActive(true);
                        return;
                    }
                }
                else // 기타 파일
                {
                    WindowFile file = GameManager.Ins.Window.Get_WindowFile(fildeIndex);
                    if (file != null)
                    {
                        m_dropPath[createCount].Set_Dropdown(FolderDropdownPath.STATE.ST_BEFORE, 2 - createCount, file.FilePath);

                        createCount++;
                        if (createCount >= 2)
                        {
                            m_dropDownPanel.SetActive(true);
                            return;
                        }
                    }
                }
            }

            // 2개가 안채워졌으면 앞경로가 있으면 앞경로로 나머지 출력
            Stack<int> next = new Stack<int>(GameManager.Ins.Window.Folder.NextFileIndex);
            int nextCount = next.Count;
            for (int i = 0; i < nextCount; ++i)
            {
                int fildeIndex = next.Pop();
                WindowFile file = GameManager.Ins.Window.Get_WindowFile(fildeIndex);
                if (file != null)
                {
                    m_dropPath[createCount].Set_Dropdown(FolderDropdownPath.STATE.ST_NEXT, 2 - createCount, file.FilePath);

                    createCount++;
                    if (createCount >= 2)
                    {
                        m_dropDownPanel.SetActive(true);
                        return;
                    }
                }
            }

            // 초기화
            for(int i = 1; i >= createCount; --i )
                m_dropPath[i].Set_Dropdown(FolderDropdownPath.STATE.ST_END, 0, "");
            m_dropDownPanel.SetActive(true);
        }
        else
        {
            m_dropDownPanel.SetActive(false);
        }
    }

    public void Button_GoPath() // 바로가기 버튼
    {
        WindowManager WM = GameManager.Ins.Window;
        WM.Folder.FavoriteTransform.gameObject.SetActive(false);
        if (WM.Folder.IsButtonClick == false)
        {
            m_dropDownPanel.SetActive(false);
            return;
        }

        string path = WM.Folder.Set_RestorPathFormat(m_pathText.text);
        if (path == WM.Folder.Path) // 현재 폴더와 같은 경로일 때 재 실행 방지
        {
            m_dropDownPanel.SetActive(false);
            return;
        }

        // 이동
        if (path == WM.BackgroundPath) // 배경화면일 시
        {
            WM.Folder.Active_Popup(true, 0);
        }
        else if (WM.Check_File(path) == false) // 존재하지 않을 시 이전 경로로 입력값 초기화
        {
            m_pathText.text = WM.Folder.Set_PathFormat(WM.Folder.Path);
        }
        else // 존재할 시 해당 폴더로 위치 이동 및 액션 실행
        {
            WindowFile windowFile = WM.Get_WindowFile(path, new WindowFileData());
            if(windowFile.FilePath != WM.Folder.Path) // 현재 폴더와 같은 경로일 때 재 실행 방지
                windowFile.FileData.fileAction();
            else // 초기화
                m_pathText.text = WM.Folder.Set_PathFormat(WM.Folder.Path);
        }
        m_dropDownPanel.SetActive(false);
    }
}
