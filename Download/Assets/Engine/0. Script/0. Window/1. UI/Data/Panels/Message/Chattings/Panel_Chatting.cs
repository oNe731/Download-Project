using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Chatting : Panel_Popup
{
    private MessageList m_owner; // Ŭ���� �޼��� ����Ʈ

    private Transform  m_chattingTransform; // ä�� �г� Ʈ������
    private ScrollRect m_scrollRect;        // ä�� ��ũ��

    // ���ҽ�
    private GameObject m_self;
    private GameObject m_other;

    public ScrollRect ScrollRect => m_scrollRect;

    public Panel_Chatting() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_CHATTING;

        // ������ �ε�
        m_self  = GameManager.Ins.Resource.Load<GameObject>("5. Prefab/0. Window/UI/Message/Chattings/Chattings_Self");
        m_other = GameManager.Ins.Resource.Load<GameObject>("5. Prefab/0. Window/UI/Message/Chattings/Chattings_OtherParty");
    }

    protected override void Active_Event(bool active)
    {
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Message/Panel_Chatting", canvas.GetChild(3));
        m_select = false; // �⺻ ��Ȱ��ȭ
        m_object.SetActive(false); //m_object.SetActive(m_select);

        #region �⺻ ����
        m_chattingTransform = m_object.transform.GetChild(3).GetChild(0).GetChild(0);
        m_scrollRect = m_object.transform.GetChild(3).GetComponent<ScrollRect>();
        m_object.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = GameManager.Ins.PlayerName;              // �÷��̾� �̸� ����
        m_object.transform.GetChild(2).GetChild(2).GetComponent<TMP_InputField>().text = GameManager.Ins.Window.StatusText; // ���� ���� ����
        #endregion
    }

    #region
    public void Set_ChattingsData(MessageList owner) // ä�� ���� ����
    {
        m_owner = owner;

        // Interval ���� �� ����
        int childCount = m_chattingTransform.childCount;
        for (int i = 0; i < childCount - 1; i++)
            GameManager.Ins.Resource.Destroy(m_chattingTransform.GetChild(i).gameObject);

        // ä�� ����
        int count = m_owner.Chattings[0].chattings.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject obj;
            if(m_owner.Chattings[0].chattings[i].type == ChattingData.COMMUNICANTSTYPE.CT_SENDER)
                obj = GameManager.Ins.Resource.Create(m_other, m_chattingTransform);
            else
                obj = GameManager.Ins.Resource.Create(m_self, m_chattingTransform);
            obj.transform.SetSiblingIndex(m_chattingTransform.childCount - 2);
            obj.GetComponent<ChatBox>().Set_ChattingsData(m_owner.Chattings[0].senderName, m_owner.Chattings[0].chattings[i]);
        }
    }

    public void Add_ChattingData(string senderName, Chatting chatting) // ä�� �Է�
    {
        if (m_owner == null)
            return;

        GameObject obj;
        string name;
        if (chatting.type == ChattingData.COMMUNICANTSTYPE.CT_SENDER)
        {
            obj = GameManager.Ins.Resource.Create(m_other, m_chattingTransform);
            name = m_owner.Chattings[0].senderName;
        }
        else
        {
            obj = GameManager.Ins.Resource.Create(m_self, m_chattingTransform);
            name = senderName;
        }
        obj.transform.SetSiblingIndex(m_chattingTransform.childCount - 2);
        obj.GetComponent<ChatBox>().Set_ChattingsData(name, chatting);

        // ä�� ����Ʈ�� �߰�
        m_owner.Chattings[0].chattings.Add(chatting);
    }
    #endregion
}
