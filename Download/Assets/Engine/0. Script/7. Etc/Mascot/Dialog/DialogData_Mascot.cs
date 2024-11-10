using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class DialogData_Mascot
{
    public enum DIALOGTYPE
    {
        DET_NONE, DET_ANIMWAIT, DET_MOVE, // 0 1 2
        DET_DELETGAME, DET_DELETFINISH, DET_CREATEGAME, DET_WAITNOVEL, DET_CLICKNOVEL, // 3 4 5 6

        DET_END
    };

    public DIALOGTYPE dialogEvent;

    public string dialogText; // 1
    public string animationTriger;

    public bool setPosition; // 0
    public List<float> startPosition;

    public bool movePosition; // 2
    public List<float> targetPosition;
}

