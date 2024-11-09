using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Western
{
    [Serializable]
    public class DialogData_PlayWT
    {
        public enum DIALOGEVENT_TYPE // ���̾�α� �ڽ��� ��鸮�� �� ȿ�� �߰� ����
        {
            DET_NONE, // 0
            DET_FADEIN, DET_FADEOUT, DET_FADEINOUT, DET_FADEOUTIN, // 1 2 3 4
            DET_WAIT, DET_BOMB, DET_TUTORIAL, // 5 6 7

            DET_END
        }

        public enum DIALOGTALK_TYPE
        {
            DTT_NONE, // 0
            DTT_SHACK, // 1

            DTT_END
        }

        public enum DIALOGCLOSE_TYPE
        {
            DCT_NONE, DET_MOVE, // 0 1
            DET_END
        }

        public DIALOGEVENT_TYPE dialogEvent;
        public DIALOGTALK_TYPE dialogTalk;
        public string dialogText;

        // ���ҽ� ����
        public string profileSpr;
        public float[] fontColor; // HTML ���� ���ڿ��� ����˴ϴ�.

        // ��Ÿ ����
        public DIALOGCLOSE_TYPE closeType;
        public string eventInfo;
    }
}

