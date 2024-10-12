using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSlot : WindowData
{
    private bool m_empty = true;
    private Panel_Popup m_panel = null;

    private Image m_selectImage = null;
    private Image m_iconImage = null;

    public bool Empty => m_empty;
    public Panel_Popup Panel => m_panel;

    public override void Load_Scene()
    {
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Taskbar/Slot", GameManager.Ins.Window.Taskbar.Object.transform.GetChild(2));

        m_selectImage = m_object.GetComponent<Image>();
        m_iconImage = m_object.transform.GetChild(0).GetComponent<Image>();

        m_object.transform.GetChild(0).GetComponent<SlotIcon>().Initialize_Icon(this);

        if (m_panel != null)
            Add_Icon(m_panel);
    }

    public void Add_Icon(Panel_Popup panel)
    {
        m_empty = false;

        m_panel = panel;
        m_panel.Slot = this;

        Set_Select(m_panel.Select);

        m_iconImage.sprite = GameManager.Ins.Window.Get_IconSprite(m_panel.FileType);
        m_iconImage.gameObject.SetActive(true);
    }

    public void Remove_Icon()
    {
        m_empty = true;

        if (m_panel != null)
        {
            m_panel.Slot = null;
            m_panel = null;
        }

        m_selectImage.color = new Color(m_selectImage.color.r, m_selectImage.color.g, m_selectImage.color.b, 0f);
        m_iconImage.sprite  = null;
        m_iconImage.gameObject.SetActive(false);
    }

    public void Set_Select(bool select)
    {
        Color color = m_selectImage.color;
        if (select == true)
            color.a = 0.3f;
        else if(select == false)
            color.a = 0f;
        m_selectImage.color = color;
    }

    public void Click_Icon()
    {
        if (m_panel == null)
            return;

        m_panel.Putdown_Popup();
    }
}
