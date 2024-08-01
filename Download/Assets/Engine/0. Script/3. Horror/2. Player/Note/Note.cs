using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private UINote m_uiNote = null;
    private bool   m_open = false;

    private List<NoteSlot> m_weaponItems = new List<NoteSlot>();
    private List<NoteSlot> m_itemItems = new List<NoteSlot>();
    private List<NoteSlot> m_clueItems = new List<NoteSlot>();

    private int m_weaponMax = 3;
    private int m_itemMax = 18;
    private int m_clueMax = 24;

    private GameObject m_itemPageItems;
    private GameObject m_cluePageItems;
    public GameObject ItemPageItems => m_itemPageItems;
    public GameObject CluePageItems => m_cluePageItems;

    private void Start()
    {
        m_uiNote = GetComponent<UINote>();

        m_itemPageItems = gameObject.transform.GetChild(4).gameObject;
        m_cluePageItems = gameObject.transform.GetChild(3).gameObject;

        // 무기 슬롯 할당
        Transform weaponTransform = m_itemPageItems.transform.GetChild(0);
        for (int i = 0; i < weaponTransform.childCount; i++)
        {
            NoteSlot component = weaponTransform.GetChild(i).GetComponent<NoteSlot>();
            if (component != null)
            {
                component.Initialize_Slot(this);
                m_weaponItems.Add(component); // 3
            }
        }

        // 아이템 슬롯 할당
        Transform itemTransform = m_itemPageItems.transform.GetChild(1);
        for (int i = 0; i < itemTransform.childCount; i++)
        {
            NoteSlot component = itemTransform.GetChild(i).GetComponent<NoteSlot>();
            if (component != null)
            {
                component.Initialize_Slot(this);
                m_itemItems.Add(component); // 18
            }
        }

        // 단서 슬롯 할당
        Transform clueTransform = m_cluePageItems.transform;
        for (int i = 0; i < clueTransform.childCount; i++)
        {
            NoteSlot component = clueTransform.GetChild(i).GetComponent<NoteSlot>();
            if (component != null)
            {
                component.Initialize_Slot(this);
                m_clueItems.Add(component); // 24
            }
        }

        // 노트 무기함에 현재 가지고 있는 무기 정보 업데이트
        for (int i = 0; i < HorrorManager.Instance.Player.WeaponManagement.Weapons.Count; ++i)
            Add_Weapon(HorrorManager.Instance.Player.WeaponManagement.Weapons[i].ItemInfo);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            m_open = !m_open;
            m_uiNote.Opne_Note(m_open);
        }
    }

    public void Add_Weapon(NoteItem noteItem) // 무기
    {
        // 중복일 시 추가X
        for (int i = 0; i < m_weaponMax; i++)
        {
            if (m_weaponItems[i].Item == null)
                continue;

            if (m_weaponItems[i].Item.m_itemType == noteItem.m_itemType)
                return;
        }

        // 아이템 추가
        for (int i = 0; i < m_weaponMax; i++)
        {
            if (m_weaponItems[i].Item != null)
                continue;

            m_weaponItems[i].Add_Item(noteItem, true);
            break;
        }
    }

    public void Add_Item(NoteItem noteItem) // 아이템
    {
        // 드래그 엔 드랍으로 버리기
        // 빈 공간은 버리자마자 땡겨짐


        // 중복 아이템 검사
        for (int i = 0; i < m_itemMax; i++)
        {
            if (m_itemItems[i].Item == null)
                continue;

            if (m_itemItems[i].Item.m_itemType == noteItem.m_itemType)
            {
                m_itemItems[i].Add_Item(noteItem, false);
                return;
            }
        }

        // 아이템 추가
        for (int i = 0; i < m_itemMax; i++)
        {
            if (m_itemItems[i].Item != null)
                continue;

            m_itemItems[i].Add_Item(noteItem, true);
            break;
        }
    }

    public void Add_Proviso(NoteItem noteItem) // 단서
    {
        // 중복 아이템 검사
        for (int i = 0; i < m_itemMax; i++)
        {
            if (m_itemItems[i].Item == null)
                continue;

            if (m_itemItems[i].Item.m_itemType == noteItem.m_itemType)
            {
                m_itemItems[i].Add_Item(noteItem, false);
                return;
            }
        }

        // 아이템 추가
        for (int i = 0; i < m_clueMax; i++)
        {
            if (m_clueItems[i].Item != null)
                continue;

            m_clueItems[i].Add_Item(noteItem, true);
            break;
        }
    }

    public NoteItem Get_Item(NoteItem.ITEMTYPE itemType)
    {
        for (int i = 0; i < m_itemMax; i++)
        {
            if (m_itemItems[i].Item == null)
                continue;

            if (m_itemItems[i].Item.m_itemType == itemType)
                return m_itemItems[i].Item;
        }

        return null;
    }

    public void Set_Item(NoteItem.ITEMTYPE itemType, NoteItem noteItem)
    {
        for (int i = 0; i < m_itemMax; i++)
        {
            if (m_itemItems[i].Item == null)
                continue;

            if (m_itemItems[i].Item.m_itemType == itemType)
                m_itemItems[i].Add_Item(noteItem, true);
        }
    }
}
