using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FolderBookmark : MonoBehaviour, IPointerClickHandler
{
    public void Set_Bookmark(WindowFileData filedata)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Ins.Window.Get_FileSprite(filedata.fileType);
        transform.GetChild(1).GetComponent<TMP_Text>().text = filedata.fileName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 목록 클릭 시 해당 파일 경로로 이동
    }
}
