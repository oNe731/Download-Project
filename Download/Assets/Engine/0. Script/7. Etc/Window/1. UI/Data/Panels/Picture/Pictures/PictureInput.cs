using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureInput : MonoBehaviour
{
    #region 즐겨찾기
    public void Active_Favorite() // 즐겨찾기 비/활성화 버튼
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Picture.IsButtonClick == false)
            return;

        // 즐겨찾기 목록 비/ 활성화
        Transform favoriteTr = WM.Picture.FavoriteTransform;
        if (favoriteTr.gameObject.activeSelf == false)
            Update_Picture();
        favoriteTr.gameObject.SetActive(!favoriteTr.gameObject.activeSelf);
    }

    public void Favorite_Picture() // 파일 즐겨찾기
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Picture.IsButtonClick == false)
            return;

        // 파일 즐겨찾기
        WindowFile windowFile = WM.Get_WindowFile(WM.Picture.ActiveType);
        windowFile.Favorite = !windowFile.Favorite;

        // 즐겨찾기 패널 업데이트
        Update_Picture();
    }

    private void Update_Picture()
    {
        Transform favoriteTr = GameManager.Ins.Window.Picture.FavoriteTransform;

        // 0번 자식 제외한 자식 삭제
        for (int i = favoriteTr.childCount - 1; i > 0; i--)
            Destroy(favoriteTr.GetChild(i).gameObject);

        GameObject prefab = GameManager.Ins.Resource.Load<GameObject>("5. Prefab/0. Window/UI/Picture/Pictures/Button_Popup_Bookmark");
        Dictionary<string, WindowFile> filedatas = GameManager.Ins.Window.FileData;
        foreach (var file in filedatas)
        {
            if (file.Value.FileData.fileType == WindowManager.FILETYPE.TYPE_PICTURE && file.Value.Favorite == true)
            {
                GameObject newFavorite = GameManager.Ins.Resource.Create(prefab, favoriteTr);
                if (newFavorite != null)
                {
                    PictureBookmark bookmark = newFavorite.GetComponent<PictureBookmark>();
                    if (bookmark != null)
                        bookmark.Set_Bookmark(file.Value);
                }
            }
        }
    }
    #endregion

    #region 폴더 삭제
    public void Delete_Picture() // 폴더(파일) 삭제하기
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Picture.IsButtonClick == false)
            return;

        WindowFile file = WM.Get_WindowFile(WM.Picture.Get_Path());
        if (file == null)
            return;

        GameManager.Ins.Window.PictureDelete.Set_FileDelete(file);
        GameManager.Ins.Window.PictureDelete.Active_ChildPopup(true);
    }
    #endregion

    #region 뒤로가기/ 앞으로 가기
    public void Button_Back() // 뒤로가기
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Picture.IsButtonClick == false)
            return;

        WM.Picture.Folder_Back();
    }

    public void Button_Again() // 앞으로가기
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Picture.IsButtonClick == false)
            return;

        WM.Picture.Folder_Again();
    }
    #endregion
}
