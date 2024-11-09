using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePanelButton : MonoBehaviour
{
    public void Button_Chatting() // ä��
    {
        GameManager.Ins.Window.Message.Change_Page(Panel_Message.PAGETYPE.TYPE_CHATT);
    }

    public void Button_Call() // ��ȭ
    {
        GameManager.Ins.Window.Message.Change_Page(Panel_Message.PAGETYPE.TYPE_CALL);
    }

    public void Button_Contact() // ����ó
    {
        GameManager.Ins.Window.Message.Change_Page(Panel_Message.PAGETYPE.TYPE_CONTACT);
    }

    public void Button_Alam() // �˸�
    {
        GameManager.Ins.Window.Message.Change_Page(Panel_Message.PAGETYPE.TYPE_ALAM);
    }
}
