using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteItemInfo : MonoBehaviour
{
    [SerializeField] private Image m_Image;
    [SerializeField] private TMP_Text m_namtTxt;
    [SerializeField] private TMP_Text m_detailsTxt;

    [SerializeField] private NoteItemMagnifyingGlass m_panelMagnifyingGlass;
    private NoteItem m_item;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void MagnifyingGlass_Button()
    {
        m_panelMagnifyingGlass.gameObject.SetActive(true);
        m_panelMagnifyingGlass.Update_UIInfo(m_item);
    }

    public void Update_UIInfo(NoteItem item)
    {
        m_item   = item;

        m_Image.sprite    = HorrorManager.Instance.NoteElementIcon[item.m_imageName];
        m_namtTxt.text    = m_item.m_name;
        m_detailsTxt.text = m_item.m_details;
    }
}
