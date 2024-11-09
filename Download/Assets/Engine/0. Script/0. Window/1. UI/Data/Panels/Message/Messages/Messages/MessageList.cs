using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MessageList : WindowData
{
    private List<ChattingData> m_chattings;
    private bool m_isClick = false;

    public List<ChattingData> Chattings => m_chattings;

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

        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Message/Messages/Messages_MessageList", GameManager.Ins.Window.Message.MessageTransform);
        m_object.transform.SetSiblingIndex(GameManager.Ins.Window.Message.MessageTransform.childCount - 2);
        m_object.GetComponent<MessageBox>().Set_Owner(this);

        #region �⺻ ����
        //* // ������ �̹��� : 1
        m_object.transform.GetChild(2).GetComponent<TMP_Text>().text = m_chattings[0].senderName; // �߽��� �̸� �ؽ�Ʈ
        m_object.transform.GetChild(3).GetComponent<TMP_Text>().text = m_chattings[0].chattings[m_chattings[0].chattings.Count - 1].text; // ������ �ؽ�Ʈ
        m_object.transform.GetChild(4).GetComponent<TMP_Text>().text = m_chattings[0].chattings[m_chattings[0].chattings.Count - 1].time; // ������ �ؽ�Ʈ �ð�
        Set_Click(m_isClick);
        #endregion
    }

    public void Set_Click(bool click)
    {
        m_isClick = click;
        if (m_isClick == false)
        {
            m_object.transform.GetChild(5).gameObject.SetActive(true);
            m_object.transform.GetChild(5).GetChild(0).GetComponent<TMP_Text>().text = m_chattings[0].chattings.Count.ToString(); // ��ü �ؽ�Ʈ ����
        }
        else
        {
            m_object.transform.GetChild(5).gameObject.SetActive(false);
        }
    }
}
