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
        // ���� ����
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Taskbar/Slot", GameManager.Ins.Window.Taskbar.Object.transform.GetChild(2));
        m_object.transform.GetChild(0).GetComponent<SlotIcon>().Initialize_Icon(this);

        // ���� �Ҵ�
        m_selectImage = m_object.GetComponent<Image>();
        m_iconImage = m_object.transform.GetChild(0).GetComponent<Image>();

        // ���� �Ҵ�� �г� ���� �� ���Ҵ�
        if (m_panel != null)
            Add_Icon(m_panel);
    }

    public void Add_Icon(Panel_Popup panel)
    {
        if (m_empty == false)
            return;

        m_empty = false;
        m_panel = panel;
        m_panel.Slot = this; // ���� ���� �Ҵ�

        Set_SelectColor(m_panel.Select); // ���õ� ���� ǥ��
        // ������ �Ҵ�
        m_iconImage.sprite = GameManager.Ins.Window.Get_FileSprite(m_panel.FileType);
        m_iconImage.gameObject.SetActive(true);
    }

    public void Set_SelectColor(bool select)
    {
        Color color = m_selectImage.color;
        if (select == true)
            color.a = 0.3f;
        else if (select == false)
            color.a = 0f;
        m_selectImage.color = color;
    }

    public void Remove_Icon()
    {
        m_empty = true;
        if (m_panel != null)
        {
            m_panel.Slot = null;
            m_panel = null;
        }

        Set_SelectColor(false);
        m_iconImage.sprite  = null;
        m_iconImage.gameObject.SetActive(false);
    }

    public void Click_Icon()
    {
        if (m_panel == null)
            return;

        m_panel.Putdown_Popup();
    }
}
