using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_RecycleBin : Panel_Popup
{
    public enum BUTTONTYPE { BT_RESTORE, BT_DELETE, BT_RESTOREALL, BT_DELETEALL, BT_END }

    private List<RecycleBinBox> m_folderBoxs = new List<RecycleBinBox>(); // 선택한 파일
    private Transform m_recycleBinTransform; // 폴더 패널 트랜스폼

    public List<RecycleBinBox> SelectFolderBoxs => m_folderBoxs;

    public Panel_RecycleBin() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_TRASHBIN;
    }

    protected override void Active_Event(bool active)
    {
        if(active == true)
        {
            Set_RecycleBinData();
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Recycle/Panel_RecycleBin", canvas.GetChild(3));
        m_object.SetActive(m_select);

        // 버튼 이벤트 추가
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region 기본 셋팅
        m_childPopup = new List<Panel_Popup>();
        m_childPopup.Add(GameManager.Ins.Window.RecyclebinSub);

        m_recycleBinTransform = m_object.transform.GetChild(3).GetChild(0).GetChild(0);

        WindowManager WM = GameManager.Ins.Window;
        Transform buttonTransform = m_object.transform.GetChild(2).GetChild(0);
        buttonTransform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Active_SubPaenl(BUTTONTYPE.BT_RESTORE));
        buttonTransform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Active_SubPaenl(BUTTONTYPE.BT_DELETE));
        buttonTransform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => Active_SubPaenl(BUTTONTYPE.BT_RESTOREALL));
        buttonTransform.GetChild(6).GetComponent<Button>().onClick.AddListener(() => Active_SubPaenl(BUTTONTYPE.BT_DELETEALL));
        #endregion
    }

    public void Active_SubPaenl(BUTTONTYPE type)
    {
        switch (type)
        {
            case BUTTONTYPE.BT_DELETE: // 단일 파일 삭제
                if (m_folderBoxs == null || m_folderBoxs.Count != 1)
                    return;
                break;

            case BUTTONTYPE.BT_RESTORE:
                if (m_folderBoxs == null || m_folderBoxs.Count != 1)
                    return;
                Button_Event(BUTTONTYPE.BT_RESTORE); // 팝업창 없이 바로 실행
                return;

            case BUTTONTYPE.BT_DELETEALL:
                if (m_folderBoxs == null)
                    return;
                if (m_folderBoxs.Count == 0)
                    Set_SelectAll();
                break;

            case BUTTONTYPE.BT_RESTOREALL:
                if (m_folderBoxs == null)
                    return;
                if (m_folderBoxs.Count == 0)
                    Set_SelectAll();
                break;
        }

        WindowManager WM = GameManager.Ins.Window;
        WM.RecyclebinSub.Active_ChildPopup(true, (int)type);
    }

    #region
    public void Button_Event(BUTTONTYPE type)
    {
        switch (type)
        {
            case BUTTONTYPE.BT_DELETE: // Debug.Log("단일 파일 영구 삭제");
                Delete_Files();
                break;

            case BUTTONTYPE.BT_RESTORE: // Debug.Log("단일 파일 영구 복원");
                Restore_Files();
                break;

            case BUTTONTYPE.BT_DELETEALL: // Debug.Log("여러 파일 영구 삭제");
                Delete_Files();
                break;

            case BUTTONTYPE.BT_RESTOREALL: // Debug.Log("여러 파일 복원");
                Restore_Files();
                break;
        }
        GameManager.Ins.Window.RecyclebinSub.Button_Cancel();
    }


    private void Delete_Files() // 영구 삭제 
    {
        if (m_folderBoxs == null || m_folderBoxs.Count == 0)
            return;

        WindowManager WM = GameManager.Ins.Window;
        WindowFile parentfile = WM.Get_WindowFile(WM.BackgroundPath + "\\" + "휴지통");
        for (int i = 0; i < m_folderBoxs.Count; ++i)
        {
            WindowFile windowFile = m_folderBoxs[i].FileData;

            // 휴지통 자식 리스트에서 삭제
            parentfile.Remove_ChildFile(windowFile.FileData);

            // 휴지통에서 삭제 및 딕셔너리에서 해당 키값 삭제
            GameManager.Ins.Resource.Destroy(m_folderBoxs[i].gameObject);
            WM.FileData.Remove(windowFile.FilePath);
        }
    }

    private void Restore_Files() // 복원
    {
        if (m_folderBoxs == null || m_folderBoxs.Count == 0)
            return;

        WindowManager WM = GameManager.Ins.Window;
        WindowFile trashBinFile = WM.Get_WindowFile(WM.BackgroundPath + "\\" + "휴지통");
        for (int i = 0; i < m_folderBoxs.Count; ++i)
        {
            WindowFile windowFile = m_folderBoxs[i].FileData;

            // 휴지통 자식 리스트에서 삭제
            trashBinFile.Remove_ChildFile(windowFile.FileData);

            // 휴지통에서 삭제 및 딕셔너리에서 해당 키값 삭제
            GameManager.Ins.Resource.Destroy(m_folderBoxs[i].gameObject);
            WM.FileData.Remove(windowFile.FilePath);

            // 이전 경로로 키값 재추가
            string prevFullPath      = windowFile.FileData.fileprevfilePath;
            string prevDirectoryPath = prevFullPath.Substring(0, prevFullPath.LastIndexOf("\\"));
            if (prevDirectoryPath == WM.BackgroundPath) // 바탕화면일 시
            {
                // 바탕화면 아이콘 추가 + 파일 생성
                WM.FileIconSlots.Add_FileIcon(windowFile.FileData.fileType, windowFile.FileData.fileName, windowFile.FileData.fileAction, windowFile.FileData.windowSubData, windowFile.FileData.fileprevfilePath);
            }
            else // 바탕화면이 아닐 시
            {
                // 현재 경로인 부모 폴더 자식 리스트에 추가
                WindowFile prevParentfile = WM.Get_WindowFile(prevDirectoryPath);
                prevParentfile.Add_ChildFile(windowFile.FileData);

                // 파일 생성
                WM.Get_WindowFile(WM.Get_FullFilePath(prevDirectoryPath, windowFile.FileData.fileName), windowFile.FileData);
            }
        }
    }
    #endregion

    #region 생성
    private void Set_RecycleBinData() // 휴지통 파일 정보 셋팅
    {
        // 자식 삭제
        int childCount = m_recycleBinTransform.childCount - 1;
        for (int i = 1; i < childCount; i++)
            GameManager.Ins.Resource.Destroy(m_recycleBinTransform.GetChild(i).gameObject);

        // 패널 생성
        WindowManager WM = GameManager.Ins.Window;
        string trashBinPath = WM.BackgroundPath + "\\" + "휴지통";
        Create_FileList(trashBinPath, WM.Get_WindowFile(trashBinPath));
    }

    private void Create_FileList(string path, WindowFile trashbinFile) // 휴지통 데이터에 따른 파일 생성
    {
        FolderData folderData = (FolderData)trashbinFile.FileData.windowSubData;
        int count = folderData.childFolders.Count;
        for (int i = 0; i < count; ++i)
            Create_File(path, folderData.childFolders[i]);
    }

    public void Create_File(string path, WindowFileData windowFileData) // 폴더 파일 생성
    {
        GameObject obj = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Recycle/Recycles/Recycles_FileList", m_recycleBinTransform);
        if (obj != null)
        {
            WindowFile file = GameManager.Ins.Window.Get_WindowFile(GameManager.Ins.Window.Get_FullFilePath(path, windowFileData.fileName));

            obj.transform.SetSiblingIndex(m_recycleBinTransform.childCount - 2);
            RecycleBinBox folderbox = obj.GetComponent<RecycleBinBox>();
            if (folderbox != null)
                folderbox.Set_RecycleBinBox(file);
        }
    }
    #endregion

    #region 폴더 선택
    public void Set_SelectBox(RecycleBinBox box, bool reset = true)
    {
        for (int i = 0; i < m_folderBoxs.Count; ++i)
        {
            if (m_folderBoxs[i] == box)
            {
                m_folderBoxs[i].Set_ClickImage(RecycleBinBox.BOXIMAGE.BI_NONE);
                m_folderBoxs.RemoveAt(i);
                return;
            }
        }

        if(reset == true)
            Reset_SelectBox();

        box.Set_ClickImage(RecycleBinBox.BOXIMAGE.BT_BASIC);
        m_folderBoxs.Add(box);
    }

    public void Reset_SelectBox()
    {
        if (m_folderBoxs == null || m_folderBoxs.Count == 0)
            return;

        for(int i = 0; i < m_folderBoxs.Count; ++i)
        {
            if(m_folderBoxs[i] != null)
                m_folderBoxs[i].Set_ClickImage(RecycleBinBox.BOXIMAGE.BI_NONE);
        }
        m_folderBoxs.Clear();
    }

    public void Set_SelectAll()
    {
        int childCount = m_recycleBinTransform.childCount - 1;
        for (int i = 1; i < childCount; i++)
        {
            RecycleBinBox box = m_recycleBinTransform.GetChild(i).GetComponent<RecycleBinBox>();
            box.Set_ClickImage(RecycleBinBox.BOXIMAGE.BT_BASIC);
            m_folderBoxs.Add(box);
        }
    }
    #endregion
}
