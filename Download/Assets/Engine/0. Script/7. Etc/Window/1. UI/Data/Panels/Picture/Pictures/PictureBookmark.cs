using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PictureBookmark : MonoBehaviour, IPointerClickHandler
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
        if (m_filedata == null || m_filedata.FileData.fileAction == null)
            return;

        GameManager.Ins.Window.Picture.PictureButton.Active_Favorite();
        m_filedata.FileData.fileAction();
    }
}
