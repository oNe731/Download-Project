using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Popup : WindowData
{
    protected FILETYPE m_fileType = FILETYPE.TYPE_END;
    protected bool m_select = false;
    protected IconSlot m_slot = null;
    protected List<Panel_Popup> m_childPopup;
    protected int m_index;

    public FILETYPE FileType => m_fileType;
    public bool Select => m_select;
    public IconSlot Slot { set => m_slot = value; }

    public Panel_Popup() : base()
    {
    }

    public void Active_Popup(bool active)
    {
        if (m_object == null)
            return;

        if (active == true)
        {
            m_select = true;

            if(m_slot == null)
                GameManager.Ins.Window.Taskbar.Add_TaskbarSlot(this);
            else
                m_slot.Set_Select(m_select);
            GameManager.Ins.Window.Sort_PopupIndex(m_fileType);
        }
        else
        {
            m_select = false;

            GameManager.Ins.Window.Taskbar.Remove_TaskbarSlot(this);
            m_slot = null;

            Fold_Child();
        }

        m_object.SetActive(m_select);
    }

    public void Active_ChildPopup(bool active)
    {
        if (m_object == null)
            return;

        if (active == true)
        {
            m_select = true;
            GameManager.Ins.Window.Sort_PopupIndex(m_fileType);
        }
        else
        {
            m_select = false;
        }

        m_object.SetActive(m_select);
    }

    public void Putdown_Popup()
    {
        if (m_object == null || m_slot == null)
            return;

        m_select = !m_select;
        m_slot.Set_Select(m_select);
        m_object.SetActive(m_select);

        Fold_Child();
    }

    private void Fold_Child()
    {
        if (m_childPopup != null)
        {
            for (int i = 0; i < m_childPopup.Count; ++i)
                m_childPopup[i].Object.SetActive(false);
        }
    }
}
