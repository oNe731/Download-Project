using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePanelButton : MonoBehaviour
{
    public void Button_Chatting() // 채팅
    {
        GameManager.Ins.Window.MESSAGE.Change_Page(Panel_Message.TYPE.TYPE_CHATT);
    }

    public void Button_Call() // 통화
    {
        GameManager.Ins.Window.MESSAGE.Change_Page(Panel_Message.TYPE.TYPE_CALL);
    }

    public void Button_Contact() // 연락처
    {
        GameManager.Ins.Window.MESSAGE.Change_Page(Panel_Message.TYPE.TYPE_CONTACT);
    }

    public void Button_Alam() // 알림
    {
        GameManager.Ins.Window.MESSAGE.Change_Page(Panel_Message.TYPE.TYPE_ALAM);
    }
}
