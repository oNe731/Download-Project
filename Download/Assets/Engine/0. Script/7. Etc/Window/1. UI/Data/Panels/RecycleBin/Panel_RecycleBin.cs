using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_RecycleBin : Panel_Popup
{
    public Panel_RecycleBin() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_TRASHBIN;
    }

    protected override void Active_Event(bool active)
    {
    }

    public override void Load_Scene()
    {
    }

    public override void Update_Data()
    {
    }

    public override void Unload_Scene()
    {

    }
}
