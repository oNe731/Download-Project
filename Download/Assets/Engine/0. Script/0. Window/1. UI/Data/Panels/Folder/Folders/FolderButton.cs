using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderButton : MonoBehaviour
{
    /*#region Ű���� �Է� (�߶󳻱�/ �����ϱ�/ �ٿ��ֱ�)
    public void Update()
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false || WM.Folder.SelectFolderBox == null)
            return;

        if(Input.GetKeyDown(KeyCode.X)) // ���� �߶󳻱�
        {

        }
        if(Input.GetKeyDown(KeyCode.C)) // ���� �����ϱ� ... �����̸� (1)
        {

        }
        else if (Input.GetKeyDown(KeyCode.V)) // ���� �ٿ��ֱ�
        {

        }
    }
    #endregion*/

    #region ���ã��
    public void Active_Favorite() // ���ã�� ��/Ȱ��ȭ ��ư
    {
        WindowManager WM = GameManager.Ins.Window;
        WM.Folder.DropDownPanel.SetActive(false);
        if (WM.Folder.IsButtonClick == false)
            return;

        // ���ã�� ��� ��/ Ȱ��ȭ
        Transform favoriteTr = WM.Folder.FavoriteTransform;
        if (favoriteTr.gameObject.activeSelf == false)
            Update_Favorite();
        favoriteTr.gameObject.SetActive(!favoriteTr.gameObject.activeSelf);
    }

    public void Favorite_Folder() // ���� ���ã��
    {
        WindowManager WM = GameManager.Ins.Window;
        WM.Folder.DropDownPanel.SetActive(false);
        if (WM.Folder.IsButtonClick == false || WM.Folder.SelectFolderBox == null)
            return;

        // ���� ���ã��
        FolderBox selectBox = WM.Folder.SelectFolderBox;
        selectBox.FileData.Favorite = !selectBox.FileData.Favorite;
        selectBox.Set_Favorite();

        // ���ã�� �г� ������Ʈ
        Update_Favorite();
    }

    private void Update_Favorite()
    {
        Transform favoriteTr = GameManager.Ins.Window.Folder.FavoriteTransform;

        // 0�� �ڽ� ������ �ڽ� ����
        for (int i = favoriteTr.childCount - 1; i > 0; i--)
            Destroy(favoriteTr.GetChild(i).gameObject);

        GameObject prefab = GameManager.Ins.Resource.Load<GameObject>("5. Prefab/0. Window/UI/Folder/Folders/Button_Popup_Bookmark");
        Dictionary<string, WindowFile> filedatas = GameManager.Ins.Window.FileData;
        foreach (var file in filedatas)
        {
            if (file.Value.Favorite == true)
            {
                GameObject newFavorite = GameManager.Ins.Resource.Create(prefab, favoriteTr);
                if (newFavorite != null)
                {
                    FolderBookmark bookmark = newFavorite.GetComponent<FolderBookmark>();
                    if (bookmark != null)
                        bookmark.Set_Bookmark(file.Value);
                }
            }
        }
    }
    #endregion

    #region ���� ����/ ����
    public void Create_Folder() // ���� ��ο� ���� �����ϱ�
    {
        WindowManager WM = GameManager.Ins.Window;
        WM.Folder.DropDownPanel.SetActive(false);
        if (WM.Folder.IsButtonClick == false)
            return;

        Panel_Folder folder = WM.Folder;

        WindowFileData windowFileData = new WindowFileData();
        windowFileData.fileType = WindowManager.FILETYPE.TYPE_FOLDER;
        windowFileData.fileName = WM.Get_FileName(folder.Path, "�� ����");
        FolderData data = new FolderData();
        data.childFolders = new List<WindowFileData>();
        windowFileData.windowSubData = data;

        // ��ο� ���� ����
        if (folder.Path == WM.BackgroundPath) // ����ȭ���� ��
        {
            // ����ȭ�鿡 ������ ������ �Ǵ��� �˻�
            if (GameManager.Ins.Window.Get_BackgroundFileCount() == false)
                return;

            // ����ȭ�� ������ �߰� + ���� ����
            WM.FileIconSlots.Add_FileIcon(windowFileData.fileType, windowFileData.fileName, windowFileData.fileAction, windowFileData.windowSubData, windowFileData.fileprevfilePath);
        }
        else // ����ȭ���� �ƴ� ��
        {
            // ���� ����� �θ� ���� �ڽ� ����Ʈ�� �߰�
            WindowFile parentfile = WM.Get_WindowFile(folder.Path);
            parentfile.Add_ChildFile(windowFileData);
        }

        // ������ ���� �߰� // + �ƴ� �� ���� ����
        folder.Create_File(folder.Path, windowFileData);

        // �ش� ���� ������, �׼� �߰�
        WindowFile file = WM.Get_WindowFile(WM.Get_FullFilePath(folder.Path, windowFileData.fileName));
        file.Set_FileData(windowFileData);
        file.Set_FileAction(() => WM.Folder.Active_Popup(true, file.FileIndex));
    }

    public void Delete_Folder() // ����(����) �����ϱ�
    {
        
        WindowManager WM = GameManager.Ins.Window;
        WM.Folder.DropDownPanel.SetActive(false);
        if (WM.Folder.IsButtonClick == false || WM.Folder.SelectFolderBox == null)
            return;

        WindowFile file = WM.Folder.SelectFolderBox.FileData;
        if (file.FileData.fileType == WindowManager.FILETYPE.TYPE_TRASHBIN || 
            file.FileData.fileType == WindowManager.FILETYPE.TYPE_NOVEL || file.FileData.fileType == WindowManager.FILETYPE.TYPE_WESTERN || file.FileData.fileType == WindowManager.FILETYPE.TYPE_HORROR) // ���� ���� ����
            return;

        GameManager.Ins.Window.FolderDelete.Set_FileDelete(file);
        GameManager.Ins.Window.FolderDelete.Active_ChildPopup(true);
    }
    #endregion

    #region �ڷΰ���/ ������ ����
    public void Button_Back() // �ڷΰ���
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false)
            return;

        WM.Folder.Folder_Back();
    }

    public void Button_Again() // �����ΰ���
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false)
            return;

        WM.Folder.Folder_Again();
    }
    #endregion
}
