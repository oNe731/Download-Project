using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FolderBox : MonoBehaviour, IPointerClickHandler
{
    public enum BOXIMAGE { BI_NONE, BT_BASIC, BT_DESTROY, BI_END }

    private Image m_clickImage;
    private WindowFile m_fileData;

    public WindowFile FileData => m_fileData;

    public void Set_FolderBox(WindowFile file)
    {
        m_fileData = file;

        transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Ins.Window.Get_FileSprite(m_fileData.FileData.fileType);
        transform.GetChild(1).GetComponent<TMP_Text>().text = m_fileData.FileData.fileName;

        m_clickImage = transform.GetChild(2).GetComponent<Image>();
    }

    public void Set_ClickImage(BOXIMAGE type)
    {
        switch(type)
        {
            case BOXIMAGE.BI_NONE:
                m_clickImage.gameObject.SetActive(false);
                break;

            case BOXIMAGE.BT_BASIC:
                m_clickImage.sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/0. Window/Window Original form (Document)/UI_Window_OriginalForm_ChooseOverlay");
                m_clickImage.gameObject.SetActive(true);
                break;

            case BOXIMAGE.BT_DESTROY:
                m_clickImage.sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/0. Window/Window Game site/Window Distroy/UI_Window_ZIP_DistroyBox");
                m_clickImage.gameObject.SetActive(true);
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_fileData == null || m_fileData.Action == null)
            return;

        m_fileData.Action();
    }
}
