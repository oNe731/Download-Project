using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Folder : Panel_Popup
{
    public enum TYPE { TYPE_NONE, TYPE_ZIP, TYPE_SAVE, TYPE_END }
    public enum EVENT { EVENT_GAMEICON, EVENT_END }

    private List<FoldersData> m_foldersData;
    private TMP_InputField m_pathText;
    private ScrollRect m_scrollRect;
    private FileInput m_fileInput;

    private string m_path;
    private List<bool> m_eventBool;

    private Transform m_folderTransform;
    public Transform FolderTransform => m_folderTransform;

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
            m_scrollRect.verticalNormalizedPosition = 1f;
            switch (m_activeType)
            {
                case (int)TYPE.TYPE_NONE:
                    // 배경 파일 읽기
                    Set_WindowBackgroundData();
                    break;

                case (int)TYPE.TYPE_ZIP:
                    // zip 파일 자식 파일 읽기
                    WindowFile file = GameManager.Ins.Window.Get_WindowFile(GameManager.Ins.Window.Get_FullFilePath(GameManager.Ins.Window.BackgroundPath, "Zip"), new WindowFileData(WindowManager.FILETYPE.TYPE_ZIP, "Zip"));
                    List<FoldersData> foldersDatas = new List<FoldersData>();
                    foldersDatas.Add(new FoldersData(file.FileData.childFolders));
                    Set_FolderData("C:\\Users\\user\\Desktop\\Zip", foldersDatas);

                    // 삭제 이벤트 실행
                    if (m_eventBool[(int)EVENT.EVENT_GAMEICON] == false)
                        Start_Event(EVENT.EVENT_GAMEICON);
                    break;

                case (int)TYPE.TYPE_SAVE:
                    // 배경 파일 읽기
                    Set_WindowBackgroundData();
                    // 저장 패널 활성화
                    m_object.transform.GetChild(4).gameObject.SetActive(true);
                    // 간격 수정
                    Transform lastInterval = m_folderTransform.GetChild(m_folderTransform.childCount - 1);
                    RectTransform rectTransform = lastInterval.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 170f);
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
                case (int)TYPE.TYPE_ZIP:
                    GameManager.Ins.Window.FileIconSlots.Add_NewFileIcon(WindowManager.FILETYPE.TYPE_NOVEL, "오싹오싹 밴드부", () => GameManager.Ins.Window.WindowButton.Button_VisualNovel());
                    GameManager.Ins.Window.FileIconSlots.Add_NewFileIcon(WindowManager.FILETYPE.TYPE_WESTERN, "THE LEGEND COWBOY", () => GameManager.Ins.Window.WindowButton.Button_Western());
                    GameManager.Ins.Window.FileIconSlots.Add_NewFileIcon(WindowManager.FILETYPE.TYPE_HORROR, "THE HOSPITAL", () => GameManager.Ins.Window.WindowButton.Button_Horror());
                    break;

                case (int)TYPE.TYPE_SAVE: 
                    // 저장 패널 비활성화
                    m_object.transform.GetChild(4).gameObject.SetActive(false);
                    // 간격 수정
                    Transform lastInterval = m_folderTransform.GetChild(m_folderTransform.childCount - 1);
                    RectTransform rectTransform = lastInterval.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 1.62f);
                    break;
            }
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Panels/Panel_Folder", canvas.GetChild(3));
        m_object.SetActive(m_select);

        // 버튼 이벤트 추가
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region 기본 셋팅
        m_folderTransform = m_object.transform.GetChild(3).GetChild(0).GetChild(0);
        m_pathText = m_object.transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<TMP_InputField>();
        m_scrollRect = m_object.transform.GetChild(3).GetComponent<ScrollRect>();
        m_fileInput = m_object.transform.GetChild(4).GetComponent<FileInput>();
        m_fileInput.Start_FileInput();

        if (m_foldersData != null)
            Set_FolderData(m_path, m_foldersData);
        #endregion
    }

    public override void Update_Data()
    {
    }

    public override void Unload_Scene()
    {
    }

    private void Remove_FolderData()
    {
        int childCount = m_folderTransform.childCount - 1;
        for (int i = 1; i < childCount; i++)
            GameManager.Ins.Resource.Destroy(m_folderTransform.GetChild(i).gameObject);
    }

    public void Set_FolderData(string path, List<FoldersData> foldersDatas) // 창 열기 전 정보 셋팅
    {
        // 경로 수정
        Set_FolderPath(path);

        // 자식 삭제
        Remove_FolderData();

        // 패널 생성
        m_foldersData = foldersDatas;
        Create_FileList(path);
    }

    private void Set_WindowBackgroundData()
    {
        string path = GameManager.Ins.Window.BackgroundPath;
        Set_FolderPath(path);

        Remove_FolderData();

        m_foldersData = new List<FoldersData>();
        m_foldersData.Add(new FoldersData(GameManager.Ins.Window.FileIconSlots.Get_WindowFileData()));
        Create_FileList(path);
    }

    private void Create_FileList(string path)
    {
        int count = m_foldersData[0].childFolders.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject obj = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Folder/Folder_FileList", m_folderTransform);
            if (obj != null)
            {
                WindowFile file = GameManager.Ins.Window.Get_WindowFile(GameManager.Ins.Window.Get_FullFilePath(path, m_foldersData[0].childFolders[i].fileName), m_foldersData[0].childFolders[i]);

                obj.transform.SetSiblingIndex(m_folderTransform.childCount - 2);
                FolderBox folderbox = obj.GetComponent<FolderBox>();
                if (folderbox != null)
                    folderbox.Set_FolderBox(file);
            }
        }
    }

    private void Set_FolderPath(string path)
    {
        m_path = path;
        m_pathText.text = m_path;
    }

    #region 폴더 이벤트
    public void Start_Event(EVENT type)
    {
        if (m_eventBool[(int)type] == true)
            return;

        m_eventBool[(int)type] = true;
        switch (type)
        {
            case EVENT.EVENT_GAMEICON:
                GameManager.Ins.StartCoroutine(Destroy_GameIcon());
                break;
        }
    }

    private IEnumerator Destroy_GameIcon()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            m_InputPopupButton = false;
            int currentIndex = 0;
            int childCount = m_folderTransform.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                GameObject obj = m_folderTransform.GetChild(currentIndex).gameObject;
                FolderBox folderBox = obj.GetComponent<FolderBox>();
                if(folderBox != null)
                {
                    int fileIndex = (int)folderBox.FileData.FileData.fileType;
                    if (fileIndex >= (int)WindowManager.FILETYPE.TYPE_BLACKOUT && fileIndex <= (int)WindowManager.FILETYPE.TYPE_THET || fileIndex == 18)
                    {
                        // 디스트로이 박스 활성화
                        folderBox.Set_ClickImage(FolderBox.BOXIMAGE.BT_DESTROY);
                        yield return new WaitForSeconds(0.5f);

                        // 빈칸으로 변경
                        folderBox.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        folderBox.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        folderBox.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                        yield return new WaitForSeconds(0.25f);

                        // 삭제
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
            m_InputPopupButton = true;
            break;
        }
    }
    #endregion
}
