using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderButton : MonoBehaviour
{
    /*#region 키보드 입력 (잘라내기/ 복사하기/ 붙여넣기)
    public void Update()
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false || WM.Folder.SelectFolderBox == null)
            return;

        if(Input.GetKeyDown(KeyCode.X)) // 파일 잘라내기
        {

        }
        if(Input.GetKeyDown(KeyCode.C)) // 파일 복사하기 ... 파일이름 (1)
        {

        }
        else if (Input.GetKeyDown(KeyCode.V)) // 파일 붙여넣기
        {

        }
    }
    #endregion*/

    #region 즐겨찾기
    public void Active_Favorite() // 즐겨찾기 비/활성화 버튼
    {
        WindowManager WM = GameManager.Ins.Window;
        WM.Folder.DropDownPanel.SetActive(false);
        if (WM.Folder.IsButtonClick == false)
            return;

        // 즐겨찾기 목록 비/ 활성화
        Transform favoriteTr = WM.Folder.FavoriteTransform;
        if (favoriteTr.gameObject.activeSelf == false)
            Update_Favorite();
        favoriteTr.gameObject.SetActive(!favoriteTr.gameObject.activeSelf);
    }

    public void Favorite_Folder() // 파일 즐겨찾기
    {
        WindowManager WM = GameManager.Ins.Window;
        WM.Folder.DropDownPanel.SetActive(false);
        if (WM.Folder.IsButtonClick == false || WM.Folder.SelectFolderBox == null)
            return;

        // 파일 즐겨찾기
        FolderBox selectBox = WM.Folder.SelectFolderBox;
        selectBox.FileData.Favorite = !selectBox.FileData.Favorite;
        selectBox.Set_Favorite();

        // 즐겨찾기 패널 업데이트
        Update_Favorite();
    }

    private void Update_Favorite()
    {
        Transform favoriteTr = GameManager.Ins.Window.Folder.FavoriteTransform;

        // 0번 자식 제외한 자식 삭제
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

    #region 폴더 생성/ 삭제
    public void Create_Folder() // 현재 경로에 폴더 생성하기
    {
        WindowManager WM = GameManager.Ins.Window;
        WM.Folder.DropDownPanel.SetActive(false);
        if (WM.Folder.IsButtonClick == false)
            return;

        Panel_Folder folder = WM.Folder;

        WindowFileData windowFileData = new WindowFileData();
        windowFileData.fileType = WindowManager.FILETYPE.TYPE_FOLDER;
        windowFileData.fileName = WM.Get_FileName(folder.Path, "새 폴더");
        FolderData data = new FolderData();
        data.childFolders = new List<WindowFileData>();
        windowFileData.windowSubData = data;

        // 경로에 따른 생성
        if (folder.Path == WM.BackgroundPath) // 바탕화면일 시
        {
            // 바탕화면 아이콘 추가 + 파일 생성
            WM.FileIconSlots.Add_FileIcon(windowFileData.fileType, windowFileData.fileName, windowFileData.fileAction, windowFileData.windowSubData, windowFileData.fileprevfilePath);
        }
        else // 바탕화면이 아닐 시
        {
            // 현재 경로인 부모 폴더 자식 리스트에 추가
            WindowFile parentfile = WM.Get_WindowFile(folder.Path);
            parentfile.Add_ChildFile(windowFileData);
        }

        // 폴더에 파일 추가 // + 아닐 시 파일 생성
        folder.Create_File(folder.Path, windowFileData);

        // 해당 파일 데이터, 액션 추가
        WindowFile file = WM.Get_WindowFile(WM.Get_FullFilePath(folder.Path, windowFileData.fileName));
        file.Set_FileData(windowFileData);
        file.Set_FileAction(() => WM.Folder.Active_Popup(true, file.FileIndex));
    }

    public void Delete_Folder() // 폴더(파일) 삭제하기
    {
        
        WindowManager WM = GameManager.Ins.Window;
        WM.Folder.DropDownPanel.SetActive(false);
        if (WM.Folder.IsButtonClick == false || WM.Folder.SelectFolderBox == null)
            return;

        WindowFile file = WM.Folder.SelectFolderBox.FileData;
        if (file.FileData.fileType == WindowManager.FILETYPE.TYPE_TRASHBIN || 
            file.FileData.fileType == WindowManager.FILETYPE.TYPE_NOVEL || file.FileData.fileType == WindowManager.FILETYPE.TYPE_WESTERN || file.FileData.fileType == WindowManager.FILETYPE.TYPE_HORROR) // 삭제 가능 여부
            return;

        GameManager.Ins.Window.FolderDelete.Set_FileDelete(file);
        GameManager.Ins.Window.FolderDelete.Active_ChildPopup(true);
    }
    #endregion

    #region 뒤로가기/ 앞으로 가기
    public void Button_Back() // 뒤로가기
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false)
            return;

        WM.Folder.Folder_Back();
    }

    public void Button_Again() // 앞으로가기
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false)
            return;

        WM.Folder.Folder_Again();
    }
    #endregion
}
