using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlamList : WindowData
{
    private List<AlamData> m_alams;
    //private bool m_isClick = false;

    public List<AlamData> Alams => m_alams;

    public AlamList() : base()
    {
    }

    public override void Load_Scene()
    {
        if (m_alams != null)
            Set_Alam(m_alams);
    }

    public void Set_Alam(List<AlamData> alams)
    {
        m_alams = alams;

        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Message/Messages/Messages_AlamList", GameManager.Ins.Window.Message.MessageTransform);
        m_object.transform.SetSiblingIndex(GameManager.Ins.Window.Message.MessageTransform.childCount - 2);
        m_object.GetComponent<AlamBox>().Set_Owner(this);

        #region �⺻ ����
        #endregion
    }

    public override void Update_Data()
    {

    }

    public override void Unload_Scene()
    {

    }
}
