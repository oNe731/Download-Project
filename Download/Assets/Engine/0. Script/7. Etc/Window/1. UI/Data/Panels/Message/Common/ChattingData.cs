using System;
using System.Collections.Generic;

[Serializable]
public class ChattingData
{
    public enum COMMUNICANTSTYPE 
    { 
        CT_SENDER, CT_RECEIVER, 
        CT_END 
    }

    public string senderName;
    public List<Chatting> chattings;
}

public struct Chatting
{
    public ChattingData.COMMUNICANTSTYPE type;

    public string text;
    public string time;
    public List<float> fontColor;
    public ChatBox.CLICKTYPE clickType;
}