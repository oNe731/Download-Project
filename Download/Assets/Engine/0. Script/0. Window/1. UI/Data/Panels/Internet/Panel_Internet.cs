using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Internet : Panel_Popup
{
    public enum TYPE { TYPE_NONE, TYPE_GAMESITE, TYPE_END } // �⺻ ����, ���� �ҹ� ����Ʈ
    public enum EVENT { EVENT_GAMESITE, EVENT_ERROR, EVENT_END } // ���� ����Ʈ, ���� ����

    private Transform m_siteTransform; // ����Ʈ �г� Ʈ������
    private ScrollRect m_scrollRect;   // ����Ʈ ��ũ��
    private TMP_InputField m_siteText; // ����Ʈ ���

    private List<bool> m_eventBool; // �̺�Ʈ ����

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
            m_scrollRect.verticalNormalizedPosition = 1f; // ��ũ�� �ʱ�ȭ
            switch (m_activeType)
            {
                case (int)TYPE.TYPE_NONE: // �⺻ â ����
                    Set_InternetData("https://www.internet.com/", m_activeType);
                    break;
            }
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Internet/Panel_Internet", canvas.GetChild(3));
        m_object.SetActive(m_select);

        // ��ư �̺�Ʈ �߰�
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region �⺻ ����
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
    public void Set_InternetData(string siteText, int activeType) // ���ͳ� ���� ����
    {
        // �ڽ� ����
        int childCount = m_siteTransform.childCount;
        for (int i = 0; i < childCount; i++)
            GameManager.Ins.Resource.Destroy(m_siteTransform.GetChild(i).gameObject);

        // �ּ� �Է�
        m_siteText.text = siteText;
        m_siteText.enabled = false;

        // �г� ����
        m_activeType = activeType;
        switch (m_activeType)
        {
            case (int)TYPE.TYPE_NONE:
                m_inputPopupButton = true;
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
            case EVENT.EVENT_GAMESITE: // â �⺻ ��ư ��Ȱ��ȭ
                m_inputPopupButton = false;
                break;

            case EVENT.EVENT_ERROR: // ����â ����
                GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Internet/Error/ErrorPopup_Panel", GameObject.Find("Canvas").transform.GetChild(3));
                break;
        }
    }
    #endregion
}
