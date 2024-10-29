using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Internet : Panel_Popup
{
    public enum TYPE { TYPE_NONE, TYPE_GAMESITE, TYPE_END } // 기본 상태, 게임 불법 사이트
    public enum EVENT { EVENT_GAMESITE, EVENT_ERROR, EVENT_END } // 게임 사이트, 에러 상태

    private Transform m_siteTransform; // 사이트 패널 트랜스폼
    private ScrollRect m_scrollRect;   // 사이트 스크롤
    private TMP_InputField m_siteText; // 사이트 경로

    private List<bool> m_eventBool; // 이벤트 여부

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
            m_scrollRect.verticalNormalizedPosition = 1f; // 스크롤 초기화
            switch (m_activeType)
            {
                case (int)TYPE.TYPE_NONE: // 기본 창 상태
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
        m_scrollRect = m_object.transform.GetChild(3).GetComponent<ScrollRect>();
        m_siteText = m_object.transform.GetChild(2).GetChild(3).GetComponent<TMP_InputField>();
        #endregion
    }

    public override void Update_Data()
    {
    }

    public override void Unload_Scene()
    {
    }

    #region
    public void Set_InternetData(string siteText, int activeType) // 인터넷 정보 설정
    {
        // 자식 삭제
        int childCount = m_siteTransform.childCount;
        for (int i = 0; i < childCount; i++)
            GameManager.Ins.Resource.Destroy(m_siteTransform.GetChild(i).gameObject);

        // 주소 입력
        m_siteText.text = siteText;
        m_siteText.enabled = false;

        // 패널 생성
        m_activeType = activeType;
        switch (m_activeType)
        {
            case (int)TYPE.TYPE_NONE:
                m_InputPopupButton = true;
                GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Internet/None/Panel_Page1", m_siteTransform);
                break;

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
            case EVENT.EVENT_GAMESITE: // 창 기본 버튼 비활성화
                m_InputPopupButton = false;
                break;

            case EVENT.EVENT_ERROR: // 에러창 생성
                GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Internet/Error/ErrorPopup_Panel", GameObject.Find("Canvas").transform.GetChild(3));
                break;
        }
    }
    #endregion
}
