using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_RecycleBin : Panel_Popup
{
    public enum BUTTONTYPE { BT_RESTORE, BT_DELETE, BT_RESTOREALL, BT_DELETEALL, BT_END }

    private List<RecycleBinBox> m_folderBoxs = new List<RecycleBinBox>(); // ������ ����
    private Transform m_recycleBinTransform; // ���� �г� Ʈ������

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

        // ��ư �̺�Ʈ �߰�
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region �⺻ ����
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
            case BUTTONTYPE.BT_DELETE: // ���� ���� ����
                if (m_folderBoxs == null || m_folderBoxs.Count != 1)
                    return;
                break;

            case BUTTONTYPE.BT_RESTORE:
                if (m_folderBoxs == null || m_folderBoxs.Count != 1)
                    return;
                Button_Event(BUTTONTYPE.BT_RESTORE); // �˾�â ���� �ٷ� ����
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
            case BUTTONTYPE.BT_DELETE: // Debug.Log("���� ���� ���� ����");
                Delete_Files();
                break;

            case BUTTONTYPE.BT_RESTORE: // Debug.Log("���� ���� ���� ����");
                Restore_Files();
                break;

            case BUTTONTYPE.BT_DELETEALL: // Debug.Log("���� ���� ���� ����");
                Delete_Files();
                break;

            case BUTTONTYPE.BT_RESTOREALL: // Debug.Log("���� ���� ����");
                Restore_Files();
                break;
        }
        GameManager.Ins.Window.RecyclebinSub.Button_Cancel();
    }


    private void Delete_Files() // ���� ���� 
    {
        if (m_folderBoxs == null || m_folderBoxs.Count == 0)
            return;

        WindowManager WM = GameManager.Ins.Window;
        WindowFile parentfile = WM.Get_WindowFile(WM.BackgroundPath + "\\" + "������");
        for (int i = 0; i < m_folderBoxs.Count; ++i)
        {
            WindowFile windowFile = m_folderBoxs[i].FileData;

            // ������ �ڽ� ����Ʈ���� ����
            parentfile.Remove_ChildFile(windowFile.FileData);

            // �����뿡�� ���� �� ��ųʸ����� �ش� Ű�� ����
            GameManager.Ins.Resource.Destroy(m_folderBoxs[i].gameObject);
            WM.FileData.Remove(windowFile.FilePath);
        }
    }

    private void Restore_Files() // ����
    {
        if (m_folderBoxs == null || m_folderBoxs.Count == 0)
            return;

        WindowManager WM = GameManager.Ins.Window;
        WindowFile trashBinFile = WM.Get_WindowFile(WM.BackgroundPath + "\\" + "������");
        for (int i = 0; i < m_folderBoxs.Count; ++i)
        {
            WindowFile windowFile = m_folderBoxs[i].FileData;

            // ������ �ڽ� ����Ʈ���� ����
            trashBinFile.Remove_ChildFile(windowFile.FileData);

            // �����뿡�� ���� �� ��ųʸ����� �ش� Ű�� ����
            GameManager.Ins.Resource.Destroy(m_folderBoxs[i].gameObject);
            WM.FileData.Remove(windowFile.FilePath);

            // ���� ��η� Ű�� ���߰�
            string prevFullPath      = windowFile.FileData.fileprevfilePath;
            string prevDirectoryPath = prevFullPath.Substring(0, prevFullPath.LastIndexOf("\\"));
            if (prevDirectoryPath == WM.BackgroundPath) // ����ȭ���� ��
            {
                // ����ȭ�� ������ �߰� + ���� ����
                WM.FileIconSlots.Add_FileIcon(windowFile.FileData.fileType, windowFile.FileData.fileName, windowFile.FileData.fileAction, windowFile.FileData.windowSubData, windowFile.FileData.fileprevfilePath);
            }
            else // ����ȭ���� �ƴ� ��
            {
                // ���� ����� �θ� ���� �ڽ� ����Ʈ�� �߰�
                WindowFile prevParentfile = WM.Get_WindowFile(prevDirectoryPath);
                prevParentfile.Add_ChildFile(windowFile.FileData);

                // ���� ����
                WM.Get_WindowFile(WM.Get_FullFilePath(prevDirectoryPath, windowFile.FileData.fileName), windowFile.FileData);
            }
        }
    }
    #endregion

    #region ����
    private void Set_RecycleBinData() // ������ ���� ���� ����
    {
        // �ڽ� ����
        int childCount = m_recycleBinTransform.childCount - 1;
        for (int i = 1; i < childCount; i++)
            GameManager.Ins.Resource.Destroy(m_recycleBinTransform.GetChild(i).gameObject);

        // �г� ����
        WindowManager WM = GameManager.Ins.Window;
        string trashBinPath = WM.BackgroundPath + "\\" + "������";
        Create_FileList(trashBinPath, WM.Get_WindowFile(trashBinPath));
    }

    private void Create_FileList(string path, WindowFile trashbinFile) // ������ �����Ϳ� ���� ���� ����
    {
        FolderData folderData = (FolderData)trashbinFile.FileData.windowSubData;
        int count = folderData.childFolders.Count;
        for (int i = 0; i < count; ++i)
            Create_File(path, folderData.childFolders[i]);
    }

    public void Create_File(string path, WindowFileData windowFileData) // ���� ���� ����
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

    #region ���� ����
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
