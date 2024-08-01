using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSlot : MonoBehaviour
{
    private Note m_note = null;
    private NoteItem m_item = null;

    private UINoteIcon m_uIItem = null;

    public NoteItem Item { get => m_item; set => m_item = value; }

    public void Initialize_Slot(Note note)
    {
        m_note = note;

        GameObject gameObject = Instantiate(Resources.Load<GameObject>("5. Prefab/3. Horror/UI/UI_NoteIcon"), transform);
        m_uIItem = gameObject.GetComponent<UINoteIcon>();
        m_uIItem.gameObject.SetActive(false);
    }

    public void Add_Item(NoteItem noteItem, bool reset)
    {
        if(m_item == null || reset)
            m_item = noteItem;
        else
        {
            // 개수 증가
            if (m_item.m_itemType == noteItem.m_itemType)
                m_item.m_count += noteItem.m_count;
        }

        if(noteItem.m_count <= 0) // 초기화
        {
            m_uIItem.gameObject.SetActive(false);
            m_item = null;
            return;
        }

        m_uIItem.Initialize_Icon(m_item);
    }

    public void Use_Item()
    {
        //m_item.count -= 1;
        //transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = m_item.count.ToString();
        //if (m_item.count == 1)
        //{
        //    transform.GetChild(0).gameObject.SetActive(false);
        //}
        //else if (m_item.count <= 0)
        //{
        //    Reset_Slot();
        //    inventory.Sort_Inventory();
        //}
    }

    public void Reset_Slot()
    {
        //transform.GetChild(0).gameObject.SetActive(false);

        //m_empty = true;
        //m_item = null;

        //if (m_uIItem != null) { Destroy(m_uIItem); }
    }

    public void Click_Slot()
    {
        //inventory.Selct_Slot(this);
        //GameManager.Instance.Open_InventoryItem(m_item);
    }
}
