using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MessageList : WindowData
{
    private List<ChattingData> m_chattings;

    public MessageList() : base()
    {
    }

    public override void Load_Scene()
    {
        if (m_chattings != null)
            Set_Message(m_chattings);
    }

    public void Set_Message(List<ChattingData> chattings)
    {
        m_chattings = chattings;

        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/ChatingApp/ChatingApp_MessageList", GameManager.Ins.Window.MESSAGE.MessageTransform);
        m_object.transform.SetSiblingIndex(GameManager.Ins.Window.MESSAGE.MessageTransform.childCount - 2);
        m_object.GetComponent<MessageBox>().Set_ChattingsData(m_chattings);

        #region 기본 셋팅
        //* // 프로필 이미지 : 1
        m_object.transform.GetChild(2).GetComponent<TMP_Text>().text = chattings[0].senderName; // 발신자 이름 텍스트
        m_object.transform.GetChild(3).GetComponent<TMP_Text>().text = chattings[0].chattings[chattings[0].chattings.Count - 1].text; // 마지막 텍스트
        m_object.transform.GetChild(4).GetComponent<TMP_Text>().text = chattings[0].chattings[chattings[0].chattings.Count - 1].time; // 마지막 텍스트 시간
        m_object.transform.GetChild(5).GetChild(0).GetComponent<TMP_Text>().text = chattings[0].chattings.Count.ToString(); // 전체 텍스트 개수
        #endregion
    }

    public override void Update_Data()
    {

    }

    public override void Unload_Scene()
    {

    }
}
