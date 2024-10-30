using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Folder : Panel_Popup
{
    public enum TYPE { TYPE_NONE, TYPE_GAMEZIP, TYPE_FILESAVE, TYPE_END } // 기본 배경, 게임 Zip, 파일 저장
    public enum EVENT { EVENT_GAMEZIP, EVENT_END } // 게임 Zip

    private string m_path;
    private List<FoldersData> m_foldersData;
    private List<bool> m_eventBool;

    private TMP_InputField m_pathText;     // 폴더 경로
    private ScrollRect m_scrollRect;       // 폴더 스크롤
    private FileInput m_fileInput;         // 저장 패널 스크립트
    private FolderBox m_folderBox;         // 선택한 파일
    private Transform m_folderTransform;   // 폴더 패널 트랜스폼
    private Transform m_favoriteTransform; // 즐겨찾기 패널 트랜스폼
    private RectTransform m_lastInterval;  // 폴더 패널 하단 여백 렉트트랜스폼

    public string Path => m_path;

    public FolderBox SelectFolderBox => m_folderBox; 
    public Transform FavoriteTransform => m_favoriteTransform;

    public Panel_Folder() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_FOLDER;

        m_eventBool = new List<bool>();
        for (int i = 0; i < (int)EVENT.EVENT_END; ++i)
            m_eventBool.Add(false);
    }

    protected override void Active_Event(bool active)
    {
        if(active == true)
        {
            // 폴더 창 초기화
            m_scrollRect.verticalNormalizedPosition = 1f;
            m_favoriteTransform.gameObject.SetActive(false);
            Reset_SelectBox();

            switch (m_activeType)
            {
                case (int)TYPE.TYPE_NONE: // 기본 배경 파일 읽기
                    Set_WindowBackgroundData();
                    break;

                case (int)TYPE.TYPE_GAMEZIP: // zip 파일 자식 파일 읽기
                    m_isButtonClick = false;
                    m_pathText.enabled = false;

                    WindowFile file = GameManager.Ins.Window.Get_WindowFile(GameManager.Ins.Window.Get_FullFilePath(GameManager.Ins.Window.BackgroundPath, "Zip"), new WindowFileData(WindowManager.FILETYPE.TYPE_ZIP, "Zip"));
                    FolderData folderData = (FolderData)file.FileData.windowSubData;
                    List<FoldersData> foldersDatas = new List<FoldersData>();
                    foldersDatas.Add(new FoldersData(folderData.childFolders));
                    Set_FolderData("C:\\Users\\user\\Desktop\\Zip", foldersDatas);

                    // 삭제 이벤트 실행
                    if (m_eventBool[(int)EVENT.EVENT_GAMEZIP] == false)
                        Start_Event(EVENT.EVENT_GAMEZIP);
                    break;

                case (int)TYPE.TYPE_FILESAVE:
                    Set_WindowBackgroundData();                                               // 배경 파일 읽기
                    m_fileInput.gameObject.SetActive(true);                                   // 저장 인풋 패널 활성화
                    m_lastInterval.sizeDelta = new Vector2(m_lastInterval.sizeDelta.x, 170f); // 간격 수정

                    // 인풋 초기화
                    m_fileInput.NameField.text = "";
                    m_fileInput.PathField.text = GameManager.Ins.Window.BackgroundPath;
                    m_fileInput.PathField.enabled = false;
                    break;
            }
        }
        else
        {
            switch (m_activeType)
            {
                case (int)TYPE.TYPE_GAMEZIP:
                    m_isButtonClick = true;
                    m_pathText.enabled = true;

                    GameManager.Ins.Window.FileIconSlots.Add_FileIcon(WindowManager.FILETYPE.TYPE_NOVEL, "오싹오싹 밴드부", () => GameManager.Ins.Window.WindowButton.Button_VisualNovel());
                    GameManager.Ins.Window.FileIconSlots.Add_FileIcon(WindowManager.FILETYPE.TYPE_WESTERN, "THE LEGEND COWBOY", () => GameManager.Ins.Window.WindowButton.Button_Western());
                    GameManager.Ins.Window.FileIconSlots.Add_FileIcon(WindowManager.FILETYPE.TYPE_HORROR, "THE HOSPITAL", () => GameManager.Ins.Window.WindowButton.Button_Horror());
                    break;

                case (int)TYPE.TYPE_FILESAVE:
                    m_fileInput.gameObject.SetActive(false);                                   // 저장 패널 비활성화
                    m_lastInterval.sizeDelta = new Vector2(m_lastInterval.sizeDelta.x, 1.62f); // 간격 수정
                    break;
            }
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Folder/Panel_Folder", canvas.GetChild(3));
        m_object.SetActive(m_select);

        // 버튼 이벤트 추가
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region 기본 셋팅
        m_childPopup = new List<Panel_Popup>();
        m_childPopup.Add(GameManager.Ins.Window.FolderDelete);

        m_folderTransform = m_object.transform.GetChild(3).GetChild(0).GetChild(0);
        m_favoriteTransform = m_object.transform.GetChild(2).GetChild(0).GetChild(1);
        m_lastInterval = m_folderTransform.GetChild(m_folderTransform.childCount - 1).GetComponent<RectTransform>();
        m_pathText = m_object.transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<TMP_InputField>();
        m_scrollRect = m_object.transform.GetChild(3).GetComponent<ScrollRect>();
        m_fileInput = m_object.transform.GetChild(4).GetComponent<FileInput>();
        m_fileInput.Start_FileInput();

        if (m_foldersData != null)
            Set_FolderData(m_path, m_foldersData);
        #endregion
    }

    #region
    #region 폴더 이벤트
    public void Start_Event(EVENT type)
    {
        if (m_eventBool[(int)type] == true)
            return;

        m_eventBool[(int)type] = true;
        switch (type)
        {
            case EVENT.EVENT_GAMEZIP:
                GameManager.Ins.StartCoroutine(Destroy_GameIcon());
                break;
        }
    }

    private IEnumerator Destroy_GameIcon()
    {
        while (true)
        {
            m_inputPopupButton = false;
            yield return new WaitForSeconds(1.0f);

            m_scrollRect.verticalNormalizedPosition = 1f;
            m_scrollRect.gameObject.transform.GetChild(1).gameObject.GetComponent<Scrollbar>().enabled = false;

            int currentIndex = 0;
            int childCount = m_folderTransform.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                GameObject obj = m_folderTransform.GetChild(currentIndex).gameObject;
                FolderBox folderBox = obj.GetComponent<FolderBox>();
                if (folderBox != null)
                {
                    int fileIndex = (int)folderBox.FileData.FileData.fileType;
                    if (fileIndex >= (int)WindowManager.FILETYPE.TYPE_BLACKOUT && fileIndex <= (int)WindowManager.FILETYPE.TYPE_THET || fileIndex == 18)
                    {
                        // 삭제 박스 활성화
                        folderBox.Set_ClickImage(FolderBox.BOXIMAGE.BT_DESTROY);
                        yield return new WaitForSeconds(0.5f);

                        // 하얀 빈칸으로 변경
                        folderBox.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        folderBox.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        folderBox.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                        yield return new WaitForSeconds(0.25f);

                        // 항목 삭제
                        m_foldersData[0].childFolders.RemoveAt(currentIndex - 1);
                        GameManager.Ins.Resource.Destroy(folderBox.gameObject);
                        yield return new WaitForSeconds(0.3f);
                    }
                    else
                    {
                        currentIndex++;
                    }
                }
                else
                {
                    currentIndex++;
                }
            }

            m_inputPopupButton = true;
            m_scrollRect.gameObject.transform.GetChild(1).gameObject.GetComponent<Scrollbar>().enabled = true;
            break;
        }
    }
    #endregion

    #region 생성
    private void Set_FolderPath(string path) // 폴더 경로 설정
    {
        m_path = path;
        m_pathText.text = m_path;
    }

    private void Remove_FolderData() // 폴더 데이터 삭제
    {
        int childCount = m_folderTransform.childCount - 1;
        for (int i = 1; i < childCount; i++)
            GameManager.Ins.Resource.Destroy(m_folderTransform.GetChild(i).gameObject);
    }

    public void Set_FolderData(string path, List<FoldersData> foldersDatas) // 폴더 정보 셋팅
    {
        // 경로 수정
        Set_FolderPath(path);

        // 자식 삭제
        Remove_FolderData();

        // 패널 생성
        m_foldersData = foldersDatas;
        Create_FileList(path);
    }

    private void Set_WindowBackgroundData() // 배경 파일 정보 셋팅
    {
        // 경로 수정
        string path = GameManager.Ins.Window.BackgroundPath;
        Set_FolderPath(path);

        // 자식 삭제
        Remove_FolderData();

        // 패널 생성
        m_foldersData = new List<FoldersData>();
        m_foldersData.Add(new FoldersData(GameManager.Ins.Window.FileIconSlots.Get_WindowFileData()));
        Create_FileList(path);
    }

    private void Create_FileList(string path) // 폴더 데이터에 따른 파일 생성
    {
        int count = m_foldersData[0].childFolders.Count;
        for (int i = 0; i < count; ++i)
            Create_File(path, m_foldersData[0].childFolders[i]);
    }

    public void Create_File(string path, WindowFileData windowFileData) // 폴더 파일 생성
    {
        GameObject obj = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Folder/Folders/Folder_FileList", m_folderTransform);
        if (obj != null)
        {
            WindowFile file = GameManager.Ins.Window.Get_WindowFile(GameManager.Ins.Window.Get_FullFilePath(path, windowFileData.fileName), windowFileData);

            obj.transform.SetSiblingIndex(m_folderTransform.childCount - 2);
            FolderBox folderbox = obj.GetComponent<FolderBox>();
            if (folderbox != null)
                folderbox.Set_FolderBox(file);
        }
    }
    #endregion

    #region 폴더 선택
    public void Set_SelectBox(FolderBox box)
    {
        if (box == null || m_folderBox == box)
            return;

        Reset_SelectBox();

        m_folderBox = box;
        m_folderBox.Set_ClickImage(FolderBox.BOXIMAGE.BT_BASIC);
    }

    public void Reset_SelectBox()
    {
        if (m_folderBox == null)
            return;

        m_folderBox.Set_ClickImage(FolderBox.BOXIMAGE.BI_NONE);
        m_folderBox = null;
    }
    #endregion
    #endregion
}
