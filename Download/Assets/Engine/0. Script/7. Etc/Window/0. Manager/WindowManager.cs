using System.Collections.Generic;
using UnityEngine;

public enum FILETYPE
{
    TYPE_FOLDER, TYPE_CHATTING, TYPE_MESSAGE, TYPE_INTERNET, TYPE_MEMO, TYPE_PICTURE, TYPE_VIDEO, TYPE_RECYCLEBIN,
    TYPE_NOVEL, TYPE_WESTERN, TYPE_HORROR,

    TYPE_END
}

public class WindowManager : StageManager
{
    private Taskbar m_taskbar = new Taskbar();

    private List<Panel_Popup> m_popups = new List<Panel_Popup>();

    private Dictionary<string, Sprite> m_fileIcon = new Dictionary<string, Sprite>();

    public Taskbar Taskbar => m_taskbar;

    public Panel_Folder     Folder => (Panel_Folder)m_popups[(int)FILETYPE.TYPE_FOLDER];
    public Panel_Message    MESSAGE => (Panel_Message)m_popups[(int)FILETYPE.TYPE_MESSAGE];
    public Panel_Chatting   CHATTING => (Panel_Chatting)m_popups[(int)FILETYPE.TYPE_CHATTING];
    public Panel_Internet   INTERNET => (Panel_Internet)m_popups[(int)FILETYPE.TYPE_INTERNET];
    public Panel_Memo       MEMO => (Panel_Memo)m_popups[(int)FILETYPE.TYPE_MEMO];
    public Panel_Picture    PICTURE => (Panel_Picture)m_popups[(int)FILETYPE.TYPE_PICTURE];
    public Panel_Video      VIDEO => (Panel_Video)m_popups[(int)FILETYPE.TYPE_VIDEO];
    public Panel_RecycleBin RECYCLEBIN => (Panel_RecycleBin)m_popups[(int)FILETYPE.TYPE_RECYCLEBIN];

    public Dictionary<string, Sprite> FileIcon => m_fileIcon;

    public WindowManager() : base()
    {
        m_stageLevel = STAGE.LEVEL_WINDOW;
        m_sceneName  = "Window";

        m_popups.Add(new Panel_Folder());
        m_popups.Add(new Panel_Chatting());
        m_popups.Add(new Panel_Message());
    }

    protected override void Load_Resource()
    {
        string basicPath = "1. Graphic/2D/0. Window/Icon/";
        m_fileIcon.Add("Icon_Folder",          GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_Folder"));
        m_fileIcon.Add("Icon_Internet",        GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_Internet"));
        m_fileIcon.Add("Icon_Message",         GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_masage"));
        m_fileIcon.Add("Icon_Memo",            GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_Memo"));
        m_fileIcon.Add("Icon_Picture",         GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_Picture"));
        m_fileIcon.Add("Icon_Video",           GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_Video"));
        m_fileIcon.Add("Icon_RecycleBin",      GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_RecycleBin"));
        m_fileIcon.Add("Icon_RecycleBin_Full", GameManager.Ins.Resource.Load<Sprite>(basicPath + "WindowIcon/UI_Window_Icon_RecycleBin_Full_ver"));
    }

    public Sprite Get_IconSprite(FILETYPE type, int index = 0)
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

            case FILETYPE.TYPE_RECYCLEBIN:
                if(index == 0)
                    return m_fileIcon["Icon_RecycleBin"];
                else if(index == 1)
                    return m_fileIcon["Icon_RecycleBin_Full"];
                break;
        }

        return null;
    }

    public override void Enter_Stage()
    {
        base.Enter_Stage();
    }

    protected override void Load_Scene()
    {
        m_taskbar.Load_Scene();
        for (int i = 0; i < m_popups.Count; ++i)
            m_popups[i].Load_Scene();

        // 게임 시작
        Cursor.lockState = CursorLockMode.None;
        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => In_Game());
    }

    public override void Update_Stage()
    {
    }

    public override void LateUpdate_Stage()
    {
    }

    public override void Exit_Stage()
    {
        base.Exit_Stage();

        m_taskbar.Unload_Scene();
        for (int i = 0; i < m_popups.Count; ++i)
            m_popups[i].Unload_Scene();
    }

    public void Sort_PopupIndex(FILETYPE filetype)
    {
        for (int i = 0; i < m_popups.Count; ++i)
        {
            if(m_popups[i].FileType == filetype)
                m_popups[i].Object.transform.SetAsLastSibling();
        }
    }

    public Panel_Popup Get_Popup(GameObject gameObject)
    {
        for (int i = 0; i < m_popups.Count; ++i)
        {
            if (m_popups[i].Object == gameObject)
                return m_popups[i];
        }

        return null;
    }
}
