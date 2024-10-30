using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderButton : MonoBehaviour
{
    #region 키보드 입력 (잘라내기/ 붙여넣기)
    public void Update()
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false || WM.Folder.SelectFolderBox == null)
            return;

        // 컨트롤 C/ V 텓스트 잘라내기 붙여넣기 구현 폴더 잘라내기 붙여넣기는 할수있으면 하기 // 내문서1, 2 이렇게 생김
        if(Input.GetKeyDown(KeyCode.C))
        {

        }
        else if (Input.GetKeyDown(KeyCode.C))
        {

        }
    }
    #endregion

    #region 즐겨찾기
    public void Active_Favorite() // 즐겨찾기 비/활성화 버튼
    {
        WindowManager WM = GameManager.Ins.Window;
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
        if (WM.Folder.IsButtonClick == false)
            return;

        Panel_Folder folder = WM.Folder;

        #region 파일 이름 결정 : 현재 경로 + 이름 경로 파일 존재 여부 검사
        string fileName = "새 폴더";
        int fileCount = 1;
        string fullPath = WM.Get_FullFilePath(folder.Path, fileName);
        while (WM.Check_File(fullPath) == true) // 파일 이름 중복일 시
        {
            fileCount++;

            fileName = $"새 폴더 ({fileCount})"; // 새 폴더 (2), 새 폴더 (3) ...
            fullPath = WM.Get_FullFilePath(folder.Path, fileName);
        }
        #endregion

        #region 파일 구조체 생성
        WindowFileData windowFileData = new WindowFileData();
        windowFileData.fileType = WindowManager.FILETYPE.TYPE_FOLDER;
        windowFileData.fileName = fileName;
        #endregion

        #region 생성
        if (folder.Path == WM.BackgroundPath) // 현재 경로가 바탕화면일 시
        {
            // 바탕화면 아이콘 추가
            WM.FileIconSlots.Add_FileIcon(windowFileData.fileType, windowFileData.fileName);

        }
        else
        {
            // 현재 경로 상의 부모 폴더에 자식 리스트에 추가
            WindowFile parentfile = WM.Get_WindowFile(folder.Path, windowFileData);
            parentfile.Add_ChildFile(windowFileData);
        }
        // 폴더에 파일 추가
        folder.Create_File(folder.Path, windowFileData);
        // 해당 파일 액션 추가
        WindowFile file = WM.Get_WindowFile(fullPath, windowFileData);
        file.Set_FileAction(() => WM.Folder.Active_Popup(true));
        #endregion

        // 폴더 클릭 시 이름 변경가능
        //*
    }

    public void Delete_Folder() 
    {
        // 폴더(파일) 삭제하기
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false || WM.Folder.SelectFolderBox == null)
            return;

        // 삭제할 수 있는 파일인지 검사
        //*

        GameManager.Ins.Window.FolderDelete.Set_FileDelete(WM.Folder.SelectFolderBox.FileData);
        GameManager.Ins.Window.FolderDelete.Active_ChildPopup(true);
    }
    #endregion

    #region 뒤로가기/ 앞으로 가기
    public void Button_Back() // 뒤로가기
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false)
            return;


    }

    public void Button_Again() // 앞으로가기
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false)
            return;


    }
    #endregion

    #region 주소

    #endregion
}
