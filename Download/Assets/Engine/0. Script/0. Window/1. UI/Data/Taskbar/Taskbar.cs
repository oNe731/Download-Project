using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Taskbar : WindowData
{
    private int m_slotCount = 30;
    private List<IconSlot> m_slots = new List<IconSlot>();

    public Taskbar() : base()
    {
        // ������ ���� �ʱ� ����
        for (int i = 0; i < m_slotCount; ++i)
            m_slots.Add(new IconSlot());
    }

    public override void Load_Scene()
    {
        // �ϴܹ� �Ҵ�
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = canvas.GetChild(1).gameObject;

        // ��ư �̺�Ʈ ���
        m_object.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => Button_Exit());

        // ������ ���� ����
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

    private void Button_Exit()
    {
        GameManager.Ins.End_Game();
    }
}
