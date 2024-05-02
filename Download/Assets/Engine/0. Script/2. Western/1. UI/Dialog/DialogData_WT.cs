using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class DialogData_WT
{
    public enum DIALOGEVENT_TYPE
    {
        DET_NONE, // 0
        DET_FADEIN, DET_FADEOUT, DET_FADEINOUT, DET_FADEOUTIN, // 1 2 3 4
        DET_NEXTMAIN, // 5

        DET_END
    };

    public DIALOGEVENT_TYPE dialogEvent;
    public string dialogText;

    // 리소스 관련
    public float[] fontColor; // HTML 색상 문자열로 저장됩니다.
    public string backgroundSpr;

    // 기타 관련
    public string nextInfo;
}
