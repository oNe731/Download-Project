using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Message : Panel_Popup
{
    public enum TYPE { TYPE_CHATT, TYPE_CALL, TYPE_CONTACT, TYPE_ALAM, TYPE_END }

    private TYPE m_currentPage = TYPE.TYPE_CHATT;
    private List<MessageList> m_messageLists = new List<MessageList>();
    private List<CallList>    m_callLists    = new List<CallList>();
    private List<ContactList> m_contactLists = new List<ContactList>();
    private List<AlamList>    m_alamLists    = new List<AlamList>();

    private Transform m_messageTransform;

    public Transform MessageTransform => m_messageTransform;

    public Panel_Message() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_MESSAGE;
    }

    protected override void Active_Event(bool active)
    {
        if(active == true)
        {
            Change_Page(TYPE.TYPE_CHATT);
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Panels/Panel_Message", canvas.GetChild(3));
        m_object.SetActive(m_select);

        // 버튼 이벤트 추가
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region 기본 셋팅
        m_childPopup = new List<Panel_Popup>();
        m_childPopup.Add(GameManager.Ins.Window.CHATTING);

        m_messageTransform = m_object.transform.GetChild(4).GetChild(0).GetChild(0);
        m_object.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = GameManager.Ins.PlayerName; // 플레이어 이름 셋팅

        // 정보 셋팅
        for (int i = 0; i < m_messageLists.Count; ++i)
            m_messageLists[i].Load_Scene();
        for (int i = 0; i < m_callLists.Count; ++i)
            m_callLists[i].Load_Scene();
        for (int i = 0; i < m_contactLists.Count; ++i)
            m_contactLists[i].Load_Scene();
        for (int i = 0; i < m_alamLists.Count; ++i)
            m_alamLists[i].Load_Scene();
        #endregion
    }

    public override void Update_Data()
    {

    }

    public override void Unload_Scene()
    {
    }

    public void Add_Message(List<ChattingData> chattings)
    {
        MessageList messageList = new MessageList();
        messageList.Set_Message(chattings);

        m_messageLists.Add(messageList);

        if (m_currentPage == TYPE.TYPE_CHATT)
            messageList.Object.SetActive(true);
        else
            messageList.Object.SetActive(false);
    }

    public void Add_Call(List<CallData> callData)
    {
        CallList callList = new CallList();
        callList.Set_Call(callData);

        m_callLists.Add(callList);

        if (m_currentPage == TYPE.TYPE_CALL)
            callList.Object.SetActive(true);
        else
            callList.Object.SetActive(false);
    }

    public void Add_Contact(List<ContactData> contactData)
    {
        ContactList contactList = new ContactList();
        contactList.Set_Contact(contactData);

        m_contactLists.Add(contactList);

        if (m_currentPage == TYPE.TYPE_CONTACT)
            contactList.Object.SetActive(true);
        else
            contactList.Object.SetActive(false);
    }

    public void Add_Alam(List<CallData> callData)
    {
        CallList callList = new CallList();
        callList.Set_Call(callData);

        m_callLists.Add(callList);

        if (m_currentPage == TYPE.TYPE_ALAM)
            callList.Object.SetActive(true);
        else
            callList.Object.SetActive(false);
    }

    public void Change_Page(TYPE type)
    {
        if (m_currentPage == type)
            return;

        // 현재 패널 오브젝트 비활성화
        switch (m_currentPage)
        {
            case TYPE.TYPE_CHATT:// 채팅
                for(int i = 0; i < m_messageLists.Count; ++i)
                    m_messageLists[i].Object.SetActive(false);
                break;

            case TYPE.TYPE_CALL:// 통화
                for (int i = 0; i < m_callLists.Count; ++i)
                    m_callLists[i].Object.SetActive(false);
                break;

            case TYPE.TYPE_CONTACT:// 연락처
                for (int i = 0; i < m_contactLists.Count; ++i)
                    m_contactLists[i].Object.SetActive(false);
                break;

            case TYPE.TYPE_ALAM:// 알림
                for (int i = 0; i < m_alamLists.Count; ++i)
                    m_alamLists[i].Object.SetActive(false);
                break;
        }

        // 현재 패널 오브젝트 활성화
        m_currentPage = type;
        switch(m_currentPage)
        {
            case TYPE.TYPE_CHATT:// 채팅
                for (int i = 0; i < m_messageLists.Count; ++i)
                    m_messageLists[i].Object.SetActive(true);
                break;

            case TYPE.TYPE_CALL:// 통화
                for (int i = 0; i < m_callLists.Count; ++i)
                    m_callLists[i].Object.SetActive(true);
                break;

            case TYPE.TYPE_CONTACT:// 연락처
                for (int i = 0; i < m_contactLists.Count; ++i)
                    m_contactLists[i].Object.SetActive(true);
                break;

            case TYPE.TYPE_ALAM:// 알림
                for (int i = 0; i < m_alamLists.Count; ++i)
                    m_alamLists[i].Object.SetActive(true);
                break;
        }
    }
}
