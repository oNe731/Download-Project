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

        TYPE_CLUE1, TYPE_CLUE2, TYPE_CLUE3, TYPE_CLUE4, TYPE_CLUE5,
        TYPE_KEYPAD,

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
