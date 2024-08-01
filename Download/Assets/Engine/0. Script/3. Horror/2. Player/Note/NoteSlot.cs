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


    public void Add_Item(string itemName, int Count = 1)
    {
        //Reset_Slot();

        //m_uIItem = Instantiate(Resources.Load<GameObject>("Prefabs/MainGame/Inventory/Item/" + itemName), gameObject.transform);
        //if (m_uIItem == null)
        //    return;

        //m_uIItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        //m_uIItem.transform.localScale = new Vector3(2f, 2f, 1f);
        //m_item = m_uIItem.GetComponent<ItemData>();

        //m_item.count = Count;
        //if (m_item.count > 1)
        //{
        //    transform.GetChild(0).gameObject.SetActive(true);
        //    transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = m_item.count.ToString();
        //}

        //m_empty = false;
    }

    public void Add_Item(int count)
    {
        //m_item.count += count;
        //transform.GetChild(0).gameObject.SetActive(true);
        //transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = m_item.count.ToString();
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
