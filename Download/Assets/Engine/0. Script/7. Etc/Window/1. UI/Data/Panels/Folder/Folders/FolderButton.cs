using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderButton : MonoBehaviour
{
    public void Update()
    {
        if (GameManager.Ins == null || GameManager.Ins.Window.FOLDER.SelectFolderBox == null)
            return;

        // 컨트롤 C/ V 텓스트 잘라내기 붙여넣기 구현 폴더 잘라내기 붙여넣기는 할수있으면 하기 // 내문서1, 2 이렇게 생김
        if(Input.GetKeyDown(KeyCode.C))
        {

        }
        else if (Input.GetKeyDown(KeyCode.C))
        {

        }
    }

    public void Active_Favorite()
    {
        // 즐겨찾기 목록 활성화
        Transform favoriteTr = GameManager.Ins.Window.FOLDER.FavoriteTransform;
        if (favoriteTr.gameObject.activeSelf == true) // 즐겨찾기 목록 비활성화
        {
            favoriteTr.gameObject.SetActive(false);
            return;
        }

        Update_Favorite();
        favoriteTr.gameObject.SetActive(true);
    }


    public void Create_Folder() // 폴더 생성하기
    {
        // 생성하기 누르면 기존 페이지 안에 제일 하단에 새 폴더가 생기면서 이름 입력받기
        // 현재 경로에 파일 추가
           // 바탕화면이면 아이콘 추가
    }

    public void Delete_Folder() // 폴더(파일) 삭제하기
    {
        // 폴더 선택 후 삭제하기 누르면 팝업창 활성화
    }

    public void Favorite_Folder() 
    {
        if (GameManager.Ins == null || GameManager.Ins.Window.FOLDER.SelectFolderBox == null)
            return;

        // 파일 즐겨찾기
        FolderBox selectBox = GameManager.Ins.Window.FOLDER.SelectFolderBox;
        selectBox.FileData.Favorite = !selectBox.FileData.Favorite;
        selectBox.Set_Favorite();

        Update_Favorite();
    }


    public void Button_Back() // 뒤로가기
    {

    }

    public void Button_Again() // 앞으로가기
    {

    }

    private void Update_Favorite()
    {
        Transform favoriteTr = GameManager.Ins.Window.FOLDER.FavoriteTransform;

        // 0번 자식 제외한 자식 삭제
        for (int i = favoriteTr.childCount - 1; i > 0; i--)
            Destroy(favoriteTr.GetChild(i).gameObject);

        GameObject prefab = GameManager.Ins.Resource.Load<GameObject>("5. Prefab/0. Window/UI/Folder/Button_Popup_Bookmark");
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
                        bookmark.Set_Bookmark(file.Value.FileData);
                }
            }
        }
    }
}
