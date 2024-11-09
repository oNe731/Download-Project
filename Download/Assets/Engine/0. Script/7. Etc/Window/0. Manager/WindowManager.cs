using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : StageManager
{
    public enum FILETYPE
    {
        TYPE_FILEDELETE, TYPE_PICTUREDELETE, TYPE_TRASHSUB,
        TYPE_FOLDER, TYPE_CHATTING, TYPE_MESSAGE, TYPE_INTERNET, TYPE_MEMO, TYPE_PICTURE, TYPE_VIDEO, TYPE_TRASHBIN,

        TYPE_NOVEL, TYPE_WESTERN, TYPE_HORROR,

        TYPE_ZIP,
        TYPE_BLACKOUT, TYPE_BLOBFISH, TYPE_GATEGUADIAN, TYPE_MAGICALGIRLS, TYPE_NEGATIVENARRATIVE, TYPE_RESCUEUNION, TYPE_THET,

        TYPE_TXT,

        TYPE_END
    }

    private Taskbar           m_taskbar = new Taskbar();
    private FileIconSlots     m_fileIconSlots = new FileIconSlots();
    private List<Panel_Popup> m_popups = new List<Panel_Popup>();
    private WindowButton m_windowButton = new WindowButton();

    private string m_statusText = "한줄로 당신의 상태를 전해보세요!"; // 상태 메시지
    private string m_backgroundPath = "C:\\Users\\user\\Desktop"; // 배경화면 경로
    private Dictionary<string, WindowFile> m_fileData = new Dictionary<string, WindowFile>(); // 파일 데이터
    private Dictionary<string, Sprite> m_fileIcon = new Dictionary<string, Sprite>();         // 파일 이미지

    #region Property
    public Taskbar Taskbar => m_taskbar;
    public FileIconSlots FileIconSlots => m_fileIconSlots;
    public WindowButton WindowButton => m_windowButton;

    // 패널
    public Panel_FolderDelete FolderDelete => (Panel_FolderDelete)m_popups[(int)FILETYPE.TYPE_FILEDELETE];
    public Panel_PictureDelete PictureDelete => (Panel_PictureDelete)m_popups[(int)FILETYPE.TYPE_PICTUREDELETE];
    public Panel_RecycleBinSub RecyclebinSub => (Panel_RecycleBinSub)m_popups[(int)FILETYPE.TYPE_TRASHSUB];

    public Panel_Folder     Folder => (Panel_Folder)m_popups[(int)FILETYPE.TYPE_FOLDER];
    public Panel_Chatting   Chatting => (Panel_Chatting)m_popups[(int)FILETYPE.TYPE_CHATTING];
    public Panel_Message    Message => (Panel_Message)m_popups[(int)FILETYPE.TYPE_MESSAGE];
    public Panel_Internet   Internet => (Panel_Internet)m_popups[(int)FILETYPE.TYPE_INTERNET];
    public Panel_Memo       Memo => (Panel_Memo)m_popups[(int)FILETYPE.TYPE_MEMO];
    public Panel_Picture    Picture => (Panel_Picture)m_popups[(int)FILETYPE.TYPE_PICTURE];
    public Panel_Video      Video => (Panel_Video)m_popups[(int)FILETYPE.TYPE_VIDEO];
    public Panel_RecycleBin Recyclebin => (Panel_RecycleBin)m_popups[(int)FILETYPE.TYPE_TRASHBIN];

    public string StatusText { get => m_statusText; set => m_statusText = value; }
    public string BackgroundPath { get => m_backgroundPath; }
    public Dictionary<string, WindowFile> FileData => m_fileData;
    #endregion

    public WindowManager() : base()
    {
        m_stageLevel = STAGE.LEVEL_WINDOW;
        m_sceneName  = "Window";

        // 패널 정보 생성
        m_popups.Add(new Panel_FolderDelete());
        m_popups.Add(new Panel_PictureDelete());
        m_popups.Add(new Panel_RecycleBinSub());

        m_popups.Add(new Panel_Folder());
        m_popups.Add(new Panel_Chatting());
        m_popups.Add(new Panel_Message());
        m_popups.Add(new Panel_Internet());
        m_popups.Add(new Panel_Memo());
        m_popups.Add(new Panel_Picture());
        m_popups.Add(new Panel_Video());
        m_popups.Add(new Panel_RecycleBin());
    }

    protected override void Load_Resource()
    {
        string basicPath = "1. Graphic/2D/0. Window/Icon/";
        m_fileIcon.Add("Icon_Folder",            GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_Folder"));
        m_fileIcon.Add("Icon_Internet",          GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_Internet"));
        m_fileIcon.Add("Icon_Message",           GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_masage"));
        m_fileIcon.Add("Icon_Memo",              GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_Memo"));
        m_fileIcon.Add("Icon_Picture",           GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_Picture"));
        m_fileIcon.Add("Icon_Video",             GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_Video"));
        m_fileIcon.Add("Icon_RecycleBin",        GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/RecycleBin/UI_Window_Icon_RecycleBin"));
        m_fileIcon.Add("Icon_RecycleBin_Full",   GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/RecycleBin/UI_Window_Icon_RecycleBin_Full_ver"));
        m_fileIcon.Add("Icon_Zip",               GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_icon_ZIP"));

        m_fileIcon.Add("Icon_VisualNovel",       GameManager.Ins.Resource.Load<Sprite>(basicPath + "GameIcon/UI_Window_Icon_VisualNovel"));
        m_fileIcon.Add("Icon_Western",           GameManager.Ins.Resource.Load<Sprite>(basicPath + "GameIcon/UI_Window_Icon_Western"));
        m_fileIcon.Add("Icon_Horror",            GameManager.Ins.Resource.Load<Sprite>(basicPath + "GameIcon/UI_Window_Icon_Horror"));

        m_fileIcon.Add("Icon_Blackout",          GameManager.Ins.Resource.Load<Sprite>(basicPath + "EtcGameIcon/UI_Window_ZIP_Blackout"));
        m_fileIcon.Add("Icon_Blobfish",          GameManager.Ins.Resource.Load<Sprite>(basicPath + "EtcGameIcon/UI_Window_ZIP_Blobfish"));
        m_fileIcon.Add("Icon_GateGuadian",       GameManager.Ins.Resource.Load<Sprite>(basicPath + "EtcGameIcon/UI_Window_ZIP_GateGuadian"));
        m_fileIcon.Add("Icon_MagicalGirls",      GameManager.Ins.Resource.Load<Sprite>(basicPath + "EtcGameIcon/UI_Window_ZIP_MagicalGirls"));
        m_fileIcon.Add("Icon_NegativeNarrative", GameManager.Ins.Resource.Load<Sprite>(basicPath + "EtcGameIcon/UI_Window_ZIP_NegativeNarrative"));
        m_fileIcon.Add("Icon_RescueUnion",       GameManager.Ins.Resource.Load<Sprite>(basicPath + "EtcGameIcon/UI_Window_ZIP_RescueUnion"));
        m_fileIcon.Add("Icon_Thet",              GameManager.Ins.Resource.Load<Sprite>(basicPath + "EtcGameIcon/UI_Window_ZIP_Thet"));

        m_fileIcon.Add("Icon_TxtFifle",          GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_txtFifle"));
    }

    public Sprite Get_FileSprite(FILETYPE type, int index = 0)
    {
        switch(type)
        {
            case FILETYPE.TYPE_FOLDER:
                return m_fileIcon["Icon_Folder"];

            case FILETYPE.TYPE_INTERNET:
                return m_fileIcon["Icon_Internet"];

            case FILETYPE.TYPE_MESSAGE:
                return m_fileIcon["Icon_Message"];

            case FILETYPE.TYPE_MEMO:
                return m_fileIcon["Icon_Memo"];

            case FILETYPE.TYPE_PICTURE:
                return m_fileIcon["Icon_Picture"];

            case FILETYPE.TYPE_VIDEO:
                return m_fileIcon["Icon_Video"];

            case FILETYPE.TYPE_TRASHBIN:
                if(index == 0)
                    return m_fileIcon["Icon_RecycleBin"];
                else if(index == 1)
                    return m_fileIcon["Icon_RecycleBin_Full"];
                break;

            case FILETYPE.TYPE_NOVEL:
                return m_fileIcon["Icon_VisualNovel"];
            case FILETYPE.TYPE_WESTERN:
                return m_fileIcon["Icon_Western"];
            case FILETYPE.TYPE_HORROR:
                return m_fileIcon["Icon_Horror"];

            case FILETYPE.TYPE_ZIP:
                return m_fileIcon["Icon_Zip"];

            case FILETYPE.TYPE_BLACKOUT:
                return m_fileIcon["Icon_Blackout"];
            case FILETYPE.TYPE_BLOBFISH:
                return m_fileIcon["Icon_Blobfish"];
            case FILETYPE.TYPE_GATEGUADIAN:
                return m_fileIcon["Icon_GateGuadian"];
            case FILETYPE.TYPE_MAGICALGIRLS:
                return m_fileIcon["Icon_MagicalGirls"];
            case FILETYPE.TYPE_NEGATIVENARRATIVE:
                return m_fileIcon["Icon_NegativeNarrative"];
            case FILETYPE.TYPE_RESCUEUNION:
                return m_fileIcon["Icon_RescueUnion"];
            case FILETYPE.TYPE_THET:
                return m_fileIcon["Icon_Thet"];

            case FILETYPE.TYPE_TXT:
                return m_fileIcon["Icon_TxtFifle"];
        }

        return null;
    }

    public override void Enter_Stage()
    {
        base.Enter_Stage();
    }

    protected override void Load_Scene()
    {
        // 하단바, 배경아이콘, 팝업 정보 불러오기
        m_taskbar.Load_Scene();
        m_fileIconSlots.Load_Scene();
        for (int i = 0; i < m_popups.Count; ++i)
            m_popups[i].Load_Scene();

        // 첫 방문이라면 배경화면 아이콘 추가
        if (m_isVisit == false) 
        {
            m_isVisit = true;

            m_fileIconSlots.Add_FileIcon(0, 0, FILETYPE.TYPE_INTERNET, "인터넷",  () => Internet.Active_Popup(true, 0));
            m_fileIconSlots.Add_FileIcon(1, 0, FILETYPE.TYPE_FOLDER,   "내 폴더", () => Folder.Active_Popup(true, 0));
            m_fileIconSlots.Add_FileIcon(2, 0, FILETYPE.TYPE_MESSAGE,  "메시지",  () => Message.Active_Popup(true, 0));
            m_fileIconSlots.Add_FileIcon(3, 0, FILETYPE.TYPE_MEMO,     "메모장",  () => Memo.Active_Popup(true, 0));
            #region 사진
            m_fileIconSlots.Add_FileIcon(4, 0, FILETYPE.TYPE_PICTURE, "사진");
            WindowFile wallpaperFile = Get_WindowFile(m_backgroundPath + "\\" + "사진");
            WindowFileData wallpaperData = wallpaperFile.FileData;
            ImageData Imagedata = new ImageData();
            Imagedata.fileName = "사진";
            Imagedata.fileType  = "PNG (.png)";
            Imagedata.imageSize = "2.38MB (2,505,547 바이트)";
            Imagedata.diskSize  = "2.39MB (2,506,752 바이트)";
            wallpaperData.windowSubData = Imagedata;
            wallpaperFile.Set_FileData(wallpaperData);
            wallpaperFile.Set_FileAction(() => Picture.Active_Popup(true, wallpaperFile.FileIndex));
            #endregion
            #region 휴지통
            m_fileIconSlots.Add_FileIcon(5, 0, FILETYPE.TYPE_TRASHBIN, "휴지통",  () => Recyclebin.Active_Popup(true, 0));
            WindowFile trashbinFile = Get_WindowFile(m_backgroundPath + "\\" + "휴지통");
            WindowFileData data = trashbinFile.FileData;
            FolderData folderdata = new FolderData();
            folderdata.childFolders = new List<WindowFileData>();
            data.windowSubData = folderdata;
            trashbinFile.Set_FileData(data);
            #endregion

            Message.Add_Message(GameManager.Ins.Load_JsonData<ChattingData>("4. Data/0. Window/Chatting/Chatting_GameSite"));
            //Message.Add_Call(GameManager.Ins.Load_JsonData<CallData>("4. Data/0. Window/Chatting/Call_Temp"));
            //Message.Add_Contact(GameManager.Ins.Load_JsonData<ContactData>("4. Data/0. Window/Chatting/Contact_Temp"));
        }
        else
        {
            // 이전 씬에 따른 처리
            //*
        }

        // 게임 시작
        Cursor.lockState = CursorLockMode.None;
        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => In_Game());
    }

    public override void Update_Stage()
    {
        for (int i = 0; i < m_popups.Count; ++i)
            m_popups[i].Update_Data();
    }

    public override void LateUpdate_Stage()
    {
    }

    public override void Exit_Stage()
    {
        base.Exit_Stage();

        m_taskbar.Unload_Scene();
        m_fileIconSlots.Unload_Scene();
        for (int i = 0; i < m_popups.Count; ++i)
            m_popups[i].Unload_Scene();
    }

    #region
    public Panel_Popup Get_Popup(GameObject gameObject) // 팝업 반환
    {
        for (int i = 0; i < m_popups.Count; ++i)
        {
            if (m_popups[i].Object == gameObject)
                return m_popups[i];
        }

        return null;
    }

    public void Sort_PopupIndex(FILETYPE filetype) // 레이어 정렬
    {
        for (int i = 0; i < m_popups.Count; ++i)
        {
            if(m_popups[i].FileType == filetype)
                m_popups[i].Object.transform.SetAsLastSibling();
        }
    }
 
    public string Get_FullFilePath(string originPath, string fileName) // 파일 경로 반환
    {
        return originPath + "\\" + fileName;
    }

    public bool Check_File(string filePath) // 파일 존재 여부 반환
    {
        WindowFile file;
        return GameManager.Ins.Window.FileData.TryGetValue(filePath, out file);
    }

    public WindowFile Get_WindowFile(string filePath, WindowFileData fileData) // 파일 생성 또는 반환
    {
        WindowFile file;
        if (GameManager.Ins.Window.FileData.TryGetValue(filePath, out file) == false)
        {
            file = new WindowFile(filePath, fileData);
            GameManager.Ins.Window.FileData.Add(filePath, file);
        }

        return file;
    }

    public WindowFile Get_WindowFile(string filePath) // 파일 반환
    {
        WindowFile file;
        if (GameManager.Ins.Window.FileData.TryGetValue(filePath, out file) == true)
            return file;

        return null;
    }

    public WindowFile Get_WindowFile(int fileIndex) // 파일 반환
    {
        foreach (var file in m_fileData)
        {
            if (file.Value.FileIndex == fileIndex)
            {
                return file.Value;
            }
        }

        return null;
    }

    public void Set_WindowFileChildFile(string originPath, string fileName, string jsonPath) // Json 정보로 자식 파일 설정
    {
        List<FoldersData> jsonData = GameManager.Ins.Load_JsonData<FoldersData>(jsonPath);

        WindowFile file = Get_WindowFile(Get_FullFilePath(originPath, fileName));
        WindowFileData data = file.FileData;
        FolderData folderData = new FolderData();
        folderData.childFolders = jsonData[0].childFolders;
        data.windowSubData = folderData;
        file.Set_FileData(data);

        // 자식 파일 생성
        for(int i = 0; i < folderData.childFolders.Count; ++i)
            Get_WindowFile(Get_FullFilePath(file.FilePath, folderData.childFolders[i].fileName), folderData.childFolders[i]);
    }

    public string Get_FileName(string folderPath, string fileName) // 중복 파일 검사
    {
        string name = fileName;

        int fileCount = 1;
        string fullPath = Get_FullFilePath(folderPath, name);
        while (Check_File(fullPath) == true) // 파일 이름 중복일 시
        {
            fileCount++;

            name = $"{fileName} ({fileCount})"; // fileName (2), fileName (3) ...
            fullPath = Get_FullFilePath(folderPath, name);
        }

        return name;
    }

    public string Get_ParentPath(string path)
    {
        int lastSlashIndex = path.LastIndexOf('\\');
        return lastSlashIndex >= 0 ? path.Substring(0, lastSlashIndex) : path;
    }
    #endregion
}
