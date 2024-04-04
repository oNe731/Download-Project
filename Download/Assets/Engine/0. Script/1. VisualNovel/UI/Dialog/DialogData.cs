using System;
using System.Collections.Generic;

public enum OWNER_TYPE { OT_SIA, OT_SOYUL, OT_JIU, OT_END };
public enum DIALOG_TYPE { DT_Simple, DT_Basic, DT_END };
public enum STANDING_INDEX { SI_SIA, SI_SOYUL, SI_JIU, SI_END };
public enum PORTRAT_INDEX { PI_SIA, PI_SOYUL, PI_JIU, PI_END };

[Serializable]
public class DialogData
{
    public OWNER_TYPE  owner;
    public DIALOG_TYPE dialogType;

    public bool useName;
    public string nameText;
    public string dialogText;

    public int standingCount;
    public List<STANDING_INDEX> standingIndex;

    public PORTRAT_INDEX portraitIndex;

    public List<string> choiceText;
    public List<string> choiceDialog;
    public bool darkPanel;
}
