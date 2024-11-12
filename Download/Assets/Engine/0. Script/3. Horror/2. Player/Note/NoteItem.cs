using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NoteItem
{
    public enum NOTETYPE { TYPE_WEAPON, TYPE_ITEM, TYPE_CLUE, TYPE_END  };
    public enum ITEMTYPE 
    {
        TYPE_NOTE,

        TYPE_GUN, TYPE_PIPE, TYPE_FLASHLIGHT,

        TYPE_BULLET, TYPE_DRUG, TYPE_1FKEY,

        TYPE_S1, TYPE_S2, TYPE_S3, TYPE_S4, TYPE_S5, TYPE_S6, TYPE_S7, TYPE_S8, TYPE_S9, TYPE_S10, TYPE_S11, TYPE_S12, TYPE_S13, TYPE_S14, 
        TYPE_A306FILE, TYPE_A440FILE, TYPE_EP14_2,

        TYPE_KEYPADNUMBER,

        TYPE_END
    };

    public interface itemInfo
    {
    }

    public NOTETYPE m_noteType;
    public ITEMTYPE m_itemType;
    public itemInfo m_itemInfo;
    public string m_name;
    public string m_details;
    public string m_imageName;
    public int m_count;
}
