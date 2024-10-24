using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    public enum CLICKTYPE { CT_NONE, CT_GAMESITE, CT_END };

    private CLICKTYPE m_clickType = CLICKTYPE.CT_END;

    public void Set_ChattingsData(string senderName, Chatting chatting)
    {
        if (chatting.type == ChattingData.COMMUNICANTSTYPE.CT_SENDER)
        {
            //* // 프로필 이미지 0

            transform.GetChild(1).GetComponent<TMP_Text>().text = senderName; // 이름 텍스트

            transform.GetChild(2).GetComponent<ChatBoxClick>().Set_ChatBox(this);
            TMP_Text text = transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
            text.text = chatting.text; // 내용 텍스트
            text.color = new Color(chatting.fontColor[0], chatting.fontColor[1], chatting.fontColor[2], chatting.fontColor[3]);

            transform.GetChild(3).GetComponent<TMP_Text>().text = chatting.time; // 시간 텍스트
            GameManager.Ins.StartCoroutine(Update_TimeTextPosition(transform.GetChild(2).GetComponent<RectTransform>(), transform.GetChild(3).GetComponent<RectTransform>(), 1f, -165f));
        }
        else
        {
            transform.GetChild(0).GetComponent<ChatBoxClick>().Set_ChatBox(this);
            TMP_Text text = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            text.text = chatting.text; // 내용 텍스트
            text.color = new Color(chatting.fontColor[0], chatting.fontColor[1], chatting.fontColor[2], chatting.fontColor[3]);
            
            transform.GetChild(1).GetComponent<TMP_Text>().text = chatting.time; // 시간 텍스트
            GameManager.Ins.StartCoroutine(Update_TimeTextPosition(transform.GetChild(0).GetComponent<RectTransform>(), transform.GetChild(1).GetComponent<RectTransform>(), -1f, 227f));
        }

        m_clickType = chatting.clickType;
    }

    private IEnumerator Update_TimeTextPosition(RectTransform boxRectTransform, RectTransform timeTextRectTransform, float dir, float xOffset)
    {
        yield return null;

        float objectWidth = boxRectTransform.rect.width;
        timeTextRectTransform.anchoredPosition = new Vector2((dir * objectWidth) + xOffset, timeTextRectTransform.anchoredPosition.y);
    }

    public void OnPointerClick()
    {
        if (m_clickType == CLICKTYPE.CT_NONE || m_clickType == CLICKTYPE.CT_END)
            return;

        switch (m_clickType)
        {
            case CLICKTYPE.CT_GAMESITE: // 게임 사이트 클릭 이벤트
                GameManager.Ins.Window.MESSAGE.Active_Popup(false);
                GameManager.Ins.Window.CHATTING.Active_Popup(false);
                GameManager.Ins.Window.GAMESITE.Active_Popup(true);
                break;
        }
    }
}
