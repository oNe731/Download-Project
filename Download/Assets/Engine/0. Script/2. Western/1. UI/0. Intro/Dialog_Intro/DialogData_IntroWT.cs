using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Western
{
    [Serializable]
    public class DialogData_IntroWT
    {
        public enum DIALOGEVENT_TYPE
        {
            DET_NONE, // 0
            DET_FADEIN, DET_FADEOUT, DET_FADEINOUT, DET_FADEOUTIN, // 1 2 3 4
            DET_NEXTMAIN, DET_STAGE2, // 5 6

            DET_END
        };

        public DIALOGEVENT_TYPE dialogEvent;
        public string dialogText;

        // ���ҽ� ����
        public float[] fontColor; // HTML ���� ���ڿ��� ����˴ϴ�.
        public string backgroundSpr;

        // ��Ÿ ����
        public string nextInfo;
    }
}

