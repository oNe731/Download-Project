using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Message : Panel_Popup
{
    private TMP_Text[] m_playerName;

    public Panel_Message() : base()
    {
        m_fileType = FILETYPE.TYPE_MESSAGE;
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Panels/Panel_Message", canvas.GetChild(2).GetChild(2));
        m_object.SetActive(m_select);

        // 버튼 이벤트 추가
        canvas.GetChild(2).GetChild(1).Find("Button_Message").GetComponent<Button>().onClick.AddListener(() => Active_Popup(true));
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        m_childPopup = new List<Panel_Popup>();
        m_childPopup.Add(GameManager.Ins.Window.CHATTING);

        //for (int i = 0; i < m_playerName.Length; ++i)
        //    m_playerName[i].text = GameManager.Ins.PlayerName;
    }

    public override void Update_Data()
    {

    }

    public override void Unload_Scene()
    {

    }
}
