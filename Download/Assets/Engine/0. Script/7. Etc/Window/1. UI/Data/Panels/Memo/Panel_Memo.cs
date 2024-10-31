using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Memo : Panel_Popup
{
    private Dictionary<int, string> m_memos = new Dictionary<int, string>();

    private TMP_InputField m_inputField;

    public Dictionary<int, string> Memos => m_memos;
    public TMP_InputField InputField => m_inputField;

    public Panel_Memo() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_MEMO;
    }

    protected override void Active_Event(bool active)
    {
        if(active == true)
        {
            // 파일 인덱스
            if(m_activeType > 0)
            {
                m_inputField.text = m_memos[m_activeType];
            }
            // 기본 메모장
            else
            {
                m_inputField.text = "";
            }
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Memo/Panel_Memo", canvas.GetChild(3));
        m_object.SetActive(m_select);

        // 버튼 이벤트 추가
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));
        m_object.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => Button_Save());

        #region 기본 셋팅
        m_inputField = m_object.transform.GetChild(3).GetComponent<TMP_InputField>();
        #endregion
    }

    public override void Update_Data()
    {
    }

    public override void Unload_Scene()
    {
    }

    public void Button_Save() // 저장 버튼
    {
        GameManager.Ins.Window.Folder.Active_Popup(true, (int)Panel_Folder.TYPE.TYPE_FILESAVE);
    }
}
