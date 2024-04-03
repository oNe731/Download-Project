using System;
using System.Collections.Generic;

public enum OWNER_TYPE { OT_SIA, OT_SOYUL, OT_JIU, OT_END };
public enum DIALOG_TYPE { DT_Simple, DT_Basic, DT_END };
public enum STANDING_INDEX { SI_SIA, SI_SOYUL, SI_JIU, SI_END };
public enum PORTRAT_INDEX { PI_SIA, PI_SOYUL, PI_JIU, PI_END };

[Serializable]
public class DialogData
{
    public OWNER_TYPE Owner;

    public int standingCount;
    public List<STANDING_INDEX> standingIndex;

    public DIALOG_TYPE dialogType;

    public string nameText;           // 이름 정보
    public string dialogText;         // 다이얼로그 정보

    public PORTRAT_INDEX portraitIndex;
    public bool darkPanel;

    public List<string> choiceText;   // 선택지 문항 텍스트
    public List<string> choiceDialog; // 선택지 문항에 따른 다음 다이얼로그 경로
}
