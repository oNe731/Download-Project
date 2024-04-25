using System;
using System.Collections.Generic;

public enum DIALOGEVENT_TYPE 
{ 
    DET_NONE, // 0
    DET_FADEIN, DET_FADEOUT, DET_FADEINOUT, DET_FADEOUTIN, // 1 2 3 4
    DET_STARTSHOOT, DET_STARTCHASE, // 5 6
    DET_SHAKING, DET_PLAYCHASE, // 7 8

    DET_END 
};

public enum CHOICEEVENT_TYPE 
{ 
    CET_CLOSE, // 0
    CET_DIALOG, // 1

    CET_END 
};

[Serializable]
public class DialogData_VN
{
    public DIALOGEVENT_TYPE dialogEvent;

    public NPCTYPE owner;
    public string nameText;
    public string dialogText;

    // 리소스 관련
    public string backgroundSpr;
    public List<string> standingSpr;
    public string portraitSpr;
    public string boxSpr;
    public string ellipseSpr;
    public string arrawSpr;
    public string nameFont;
    public string dialogFont;

    // 선택지 관련
    public List<CHOICEEVENT_TYPE> choiceEventType;
    public List<string> choiceText;
    public List<string> choiceDialog;
}
