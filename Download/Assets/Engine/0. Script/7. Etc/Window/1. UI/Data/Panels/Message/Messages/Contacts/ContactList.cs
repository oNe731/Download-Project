using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContactList : WindowData
{
    private List<ContactData> m_contact;
    //private bool m_isClick = false;

    public List<ContactData> Contact => m_contact;

    public ContactList() : base()
    {
    }

    public override void Load_Scene()
    {
        if (m_contact != null)
            Set_Contact(m_contact);
    }

    public void Set_Contact(List<ContactData> contact)
    {
        m_contact = contact;

        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Message/Messages/Messages_ContactList", GameManager.Ins.Window.Message.MessageTransform);
        m_object.transform.SetSiblingIndex(GameManager.Ins.Window.Message.MessageTransform.childCount - 2);
        m_object.GetComponent<ContactBox>().Set_Owner(this);

        #region 기본 셋팅
        m_object.transform.GetChild(2).GetComponent<TMP_Text>().text = contact[0].name;   // 이름
        m_object.transform.GetChild(3).GetComponent<TMP_Text>().text = contact[0].status; // 한줄상태
        #endregion
    }

    public override void Update_Data()
    {

    }

    public override void Unload_Scene()
    {

    }
}
