using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Panel_Popup : WindowData
{
    protected WindowManager.FILETYPE m_fileType = WindowManager.FILETYPE.TYPE_END;
    protected bool m_select = false;
    protected bool m_inputPopupButton = true;
    protected bool m_isButtonClick = true;

    protected int m_activeType = -1;
    protected int m_prevActiveType = -1;

    protected IconSlot m_slot = null;
    protected List<Panel_Popup> m_childPopup;


    public WindowManager.FILETYPE FileType => m_fileType;
    public bool Select => m_select;
    public int ActiveType => m_activeType;
    public IconSlot Slot { set => m_slot = value; }
    public bool InputPopupButton { get => m_inputPopupButton; set => m_inputPopupButton = value; }
    public bool IsButtonClick { get => m_isButtonClick; set => m_isButtonClick = value; }

    public Panel_Popup() : base()
    {
    }

    public void Active_Popup(bool active, int activeType = -1)
    {
        if (m_object == null)
            return;

        if (active == true)
        {
            m_prevActiveType = m_activeType;
            m_activeType = activeType;
            m_select = true;

            if(m_slot == null)
                GameManager.Ins.Window.Taskbar.Add_TaskbarSlot(this);
            else
                m_slot.Set_SelectColor(m_select);
            GameManager.Ins.Window.Sort_PopupIndex(m_fileType);
        }
        else
        {
            if (m_inputPopupButton == false) // �ݱ� ��Ȱ��ȭ
                return;
            m_select = false;

            GameManager.Ins.Window.Taskbar.Remove_TaskbarSlot(this);
            m_slot = null;

            Fold_Child();
        }

        m_object.SetActive(m_select);
        Active_Event(active);
    }

    protected abstract void Active_Event(bool active);

    public void Active_ChildPopup(bool active, int activeType = -1)
    {
        if (m_object == null)
            return;

        if (active == true)
        {
            m_activeType = activeType;
            m_select = true;
            GameManager.Ins.Window.Sort_PopupIndex(m_fileType);
        }
        else
        {
            m_select = false;
        }

        m_object.SetActive(m_select);
        Active_Event(active);
    }

    public void Putdown_Popup()
    {
        if (m_object == null || m_slot == null || m_inputPopupButton == false)
            return;

        m_select = !m_select;
        m_slot.Set_SelectColor(m_select);
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
