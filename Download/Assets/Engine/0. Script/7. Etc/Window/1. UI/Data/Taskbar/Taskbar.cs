using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taskbar : WindowData
{
    private int m_slotCount = 23;
    private List<IconSlot> m_slots = new List<IconSlot>();

    public Taskbar() : base()
    {
        // 아이콘 슬롯 초기 생성
        for (int i = 0; i < m_slotCount; ++i)
            m_slots.Add(new IconSlot());
    }

    public override void Load_Scene()
    {
        // 하단바 할당
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = canvas.GetChild(1).gameObject;

        // 아이콘 슬롯 생성
        for (int i = 0; i < m_slotCount; ++i)
            m_slots[i].Load_Scene();
    }

    public void Add_TaskbarSlot(Panel_Popup panel)
    {
        if (Check_TaskbarSlot(panel) == true)
            return;

        for(int i = 0; i < m_slots.Count; ++i)
        {
            if(m_slots[i].Empty == true)
            {
                m_slots[i].Add_Icon(panel);
                break;
            }
        }

        return;
    }

    private bool Check_TaskbarSlot(Panel_Popup panel)
    {
        for (int i = 0; i < m_slots.Count; ++i)
        {
            if (m_slots[i].Empty == false)
            {
                if(m_slots[i].Panel == panel)
                    return true;
            }
        }

        return false;
    }

    public void Remove_TaskbarSlot(Panel_Popup panel)
    {
        for (int i = 0; i < m_slots.Count; ++i)
        {
            if (m_slots[i].Empty == false)
            {
                if(m_slots[i].Panel == panel)
                {
                    m_slots[i].Remove_Icon();
                    break;
                }
            }
        }

        Sort_TaskbarSlot();
    }

    private void Sort_TaskbarSlot()
    {
        for (int i = 0; i < m_slots.Count; ++i)
        {
            if (m_slots[i].Empty == true)
            {
                int nextIndex = i + 1;
                for (int j = nextIndex; j < m_slots.Count; ++j)
                {
                    if (m_slots[j].Empty == false)
                    {
                        m_slots[i].Add_Icon(m_slots[j].Panel);
                        m_slots[j].Remove_Icon();
                        break;
                    }
                }
            }
        }
    }
}
