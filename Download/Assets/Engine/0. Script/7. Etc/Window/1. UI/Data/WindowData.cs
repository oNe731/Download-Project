using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowData
{
    protected GameObject m_object;
    public GameObject Object => m_object;

    public WindowData()
    {
    }

    public virtual void Load_Scene()
    {

    }

    public virtual void Update_Data()
    {

    }

    public virtual void Unload_Scene()
    {

    }
}
