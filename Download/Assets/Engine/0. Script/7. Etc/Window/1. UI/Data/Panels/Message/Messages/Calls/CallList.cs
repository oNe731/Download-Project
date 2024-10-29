using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CallList : WindowData
{
    private List<CallData> m_calls;
    //private bool m_isClick = false;

    public List<CallData> Call => m_calls;

    public CallList() : base()
    {
    }

    public override void Load_Scene()
    {
        if (m_calls != null)
            Set_Call(m_calls);
    }

    public void Set_Call(List<CallData> call)
    {
        m_calls = call;

        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Message/Messages/Messages_CallList", GameManager.Ins.Window.Message.MessageTransform);
        m_object.transform.SetSiblingIndex(GameManager.Ins.Window.Message.MessageTransform.childCount - 2);
        m_object.GetComponent<CallBox>().Set_Owner(this);

        #region 기본 셋팅
        m_object.transform.GetChild(2).GetComponent<TMP_Text>().text = call[0].name;
        m_object.transform.GetChild(3).GetComponent<TMP_Text>().text = call[0].detail;
        m_object.transform.GetChild(4).GetComponent<TMP_Text>().text = call[0].time;
        #endregion
    }

    public override void Update_Data()
    {

    }

    public override void Unload_Scene()
    {

    }
}
