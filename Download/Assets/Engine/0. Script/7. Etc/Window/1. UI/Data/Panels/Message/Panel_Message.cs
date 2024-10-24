using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Message : Panel_Popup
{
    private List<MessageList> m_messageLists = new List<MessageList>();

    private Transform m_messageTransform;

    public Transform MessageTransform => m_messageTransform;

    public Panel_Message() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_MESSAGE;
    }

    protected override void Active_Event(bool active)
    {
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

        // 메시지 셋팅
        for (int i = 0; i < m_messageLists.Count; ++i)
            m_messageLists[i].Load_Scene();
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
    }
}
