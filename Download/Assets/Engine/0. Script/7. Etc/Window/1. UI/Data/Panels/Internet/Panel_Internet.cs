using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Internet : Panel_Popup
{
    public enum TYPE { TYPE_NONE, TYPE_GAMESITE, TYPE_END }
    public enum EVENT { EVENT_GAMESITE, EVENT_ERROR, EVENT_END }

    private Transform m_siteTransform;
    private TMP_InputField m_siteText;
    private ScrollRect m_scrollRect;

    private List<bool> m_eventBool;

    public Transform SiteTransform => m_siteTransform;

    public Panel_Internet() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_INTERNET;

        m_eventBool = new List<bool>();
        for(int i = 0; i < (int)EVENT.EVENT_END; ++i)
            m_eventBool.Add(false);
    }

    protected override void Active_Event(bool active)
    {
        if(active == true)
        {
            m_scrollRect.verticalNormalizedPosition = 1f;
            switch (m_activeType)
            {
                case (int)TYPE.TYPE_NONE: // 기본 창 상태
                    m_InputPopupButton = true;
                    Set_InternetData("https://www.internet.com/", m_activeType);
                    break;
            }
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Panels/Panel_Internet", canvas.GetChild(3));
        m_object.SetActive(m_select);

        // 버튼 이벤트 추가
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region 기본 셋팅
        m_siteTransform = m_object.transform.GetChild(3).GetChild(0).GetChild(0);
        m_siteText = m_object.transform.GetChild(2).GetChild(3).GetComponent<TMP_InputField>();
        m_scrollRect = m_object.transform.GetChild(3).GetComponent<ScrollRect>();
        #endregion
    }

    public override void Update_Data()
    {
    }

    public override void Unload_Scene()
    {
    }

    public void Set_InternetData(string siteText, int activeType) // 창 열기 전 정보 셋팅
    {
        // 자식 삭제
        int childCount = m_siteTransform.childCount;
        for (int i = 0; i < childCount; i++)
            GameManager.Ins.Resource.Destroy(m_siteTransform.GetChild(i).gameObject);

        // 주소 입력
        m_siteText.text = siteText;

        // 패널 생성
        m_activeType = activeType;
        switch (m_activeType)
        {
            case (int)TYPE.TYPE_GAMESITE:
                Start_Event(EVENT.EVENT_GAMESITE);
                GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Internet/GameSite/Panel_Page1", m_siteTransform);
                GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Internet/GameSite/Panel_Page2", m_siteTransform);
                break;
        }
    }

    public void Start_Event(EVENT type)
    {
        if (m_eventBool[(int)type] == true)
            return;

        m_eventBool[(int)type] = true;
        switch (type)
        {
            case EVENT.EVENT_GAMESITE:
                m_InputPopupButton = false;
                break;

            case EVENT.EVENT_ERROR:
                GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Internet/ErrorPopup_Panel", GameObject.Find("Canvas").transform.GetChild(3));
                break;
        }
    }
}
