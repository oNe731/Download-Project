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

        TYPE_BULLET, TYPE_DRUG,

        TYPE_CLUE1,

        TYPE_END
    };

    public interface itemInfo
    {
    }

    public NOTETYPE m_noteType;
    public ITEMTYPE m_itemType;
    public itemInfo m_itemInfo;
    public string m_name;
    public int m_count;
}
