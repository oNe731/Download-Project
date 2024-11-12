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

        // ������ �̿��� ��
        DET_DELETGAME, DET_DELETFINISH, DET_CREATEGAME, DET_WAITGAME, DET_CLICKNOVEL, // 3 4 5 6 7

        // �̿���
        DET_NOVELEXIT, DET_NOVELRETURN, DET_NOVELRESTART, // 8 9 10

        // ������ ���� ��
        DET_AYAKA, DET_AYAKADIALOG, DET_ATTACKAYAKA, DET_DESTROYBOX, DET_GOLFSWING, DET_GOLFRESULT, DET_MOVES, DET_MOVESSTOP, DET_CLICKWESTERN,

        DET_SOUND, DET_CLICKHORROR,

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

