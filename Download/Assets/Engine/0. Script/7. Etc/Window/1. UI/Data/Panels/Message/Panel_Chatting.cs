using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Chatting : Panel_Popup
{
    public Panel_Chatting() : base()
    {
        m_fileType = FILETYPE.TYPE_CHATTING;
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Panels/Panel_Chatting", canvas.GetChild(2).GetChild(2));
        m_object.SetActive(m_select);
    }

    public override void Update_Data()
    {

    }

    public override void Unload_Scene()
    {

    }
}
