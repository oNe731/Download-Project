using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FolderBookmark : MonoBehaviour, IPointerClickHandler
{
    private WindowFile m_filedata;

    public void Set_Bookmark(WindowFile filedata)
    {
        m_filedata = filedata;

        transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Ins.Window.Get_FileSprite(m_filedata.FileData.fileType);
        transform.GetChild(1).GetComponent<TMP_Text>().text = m_filedata.FileData.fileName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_filedata == null || m_filedata.Action == null)
            return;

        GameManager.Ins.Window.Folder.FolderButton.Active_Favorite();
        m_filedata.Action();
    }
}
