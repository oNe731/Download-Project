using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialog_VN : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject m_darkPanelObj;
    [SerializeField] private GameObject m_backgroundObj;
    [SerializeField] private GameObject[] m_standingObj;
    [SerializeField] private GameObject m_portraitObj;
    [SerializeField] private GameObject m_dialogBoxObj;
    [SerializeField] private GameObject m_ellipseObj;
    [SerializeField] private GameObject m_arrowObj;
    [SerializeField] private TMP_Text m_nameTxt;
    [SerializeField] private TMP_Text m_dialogTxt;
    [SerializeField] private NpcLike m_heartScr;

    [Header("Prefab")]
    [SerializeField] private GameObject m_choiceButtonPre;

    public DialogData_VN[] m_dialogs;

    private Image m_backgroundImg;
    private Image[] m_standingImg;
    private Image m_portraitImg;
    private Image m_dialogBoxImg;
    private Image m_ellipseImg;
    private Image m_arrowImg;

    private bool m_isTyping = false;
    private bool m_cancelTyping = false;
    private int  m_dialogIndex = 0;
    private float m_typeSpeed = 0.05f;
    private float m_arrowSpeed = 0.5f;

    private int m_choiceIndex = 0;
    private List<GameObject> m_choice_Button = new List<GameObject>();

    private void Awake()
    {
        m_backgroundImg = m_backgroundObj.GetComponent<Image>();

        m_standingImg = new Image[m_standingObj.Length];
        for (int i = 0; i < m_standingObj.Length; i++)
            m_standingImg[i] = m_standingObj[i].GetComponent<Image>();

        m_portraitImg  = m_portraitObj.GetComponent<Image>();
        m_dialogBoxImg = m_dialogBoxObj.GetComponent<Image>();
        m_ellipseImg   = m_ellipseObj.GetComponent<Image>();
        m_arrowImg     = m_arrowObj.GetComponent<Image>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            Update_Dialog();

        if (0 < m_choice_Button.Count && false == m_isTyping)
            Update_Button();
    }

    private void Update_Dialog(bool IsPonter = true)
    {
        if (IsPonter && EventSystem.current.IsPointerOverGameObject()) // 커서가 UI 위치상에 존재할 시 반환
            return;

        if (m_isTyping)
            m_cancelTyping = true;
        else if (!m_isTyping)
        {
            // 다이얼로그 진행
            if (m_dialogIndex < m_dialogs.Length)
            {
                switch(m_dialogs[m_dialogIndex].dialogEvent)
                {
                    case DIALOGEVENT_TYPE.DET_NONE:
                        Update_None();
                        break;

                    case DIALOGEVENT_TYPE.DET_FADEIN:
                        Update_FadeIn();
                        break;

                    case DIALOGEVENT_TYPE.DET_FADEOUT:
                        Update_FadeOut();
                        break;

                    case DIALOGEVENT_TYPE.DET_FADEOUTIN:
                        Update_FadeOutIn();
                        break;

                    case DIALOGEVENT_TYPE.DET_STARTSHOOT:
                        Start_ShootGame();
                        break;

                    case DIALOGEVENT_TYPE.DET_STARTCHASE:
                        Start_ChaseGame();
                        break;

                    case DIALOGEVENT_TYPE.DET_PLAYCHASE:
                        Play_ChaseGame();
                        break;

                    case DIALOGEVENT_TYPE.DET_SHAKING:
                        Update_Shaking();
                        break;

                    case DIALOGEVENT_TYPE.DET_LIKEADD:
                        Update_None();
                        break;
                }
            }
            else // 다이얼로그 종료
            {
                if (0 < m_choice_Button.Count)
                    return;

                Close_Dialog();
            }
        }
    }

    private void Update_Basic(int index)
    {
        if (!string.IsNullOrEmpty(m_dialogs[index].nameFont))
            m_nameTxt.font = VisualNovelManager.Instance.FontAst[m_dialogs[index].nameFont];
        if (!string.IsNullOrEmpty(m_dialogs[index].dialogFont))
            m_dialogTxt.font = VisualNovelManager.Instance.FontAst[m_dialogs[index].dialogFont];

        // 다이얼로그 업데이트
        m_arrowObj.SetActive(false);
        m_nameTxt.text = m_dialogs[index].nameText;
        m_heartScr.Set_Owner(m_dialogs[index].owner); // 호감도 업데이트

        // 리소스 업데이트
        if (!string.IsNullOrEmpty(m_dialogs[index].backgroundSpr))
            m_backgroundImg.sprite = VisualNovelManager.Instance.BackgroundSpr[m_dialogs[index].backgroundSpr];
        Update_Standing(index);
        if (!string.IsNullOrEmpty(m_dialogs[index].portraitSpr))
        {
            m_portraitObj.SetActive(true);
            m_portraitImg.sprite = VisualNovelManager.Instance.PortraitSpr[m_dialogs[index].portraitSpr];
        }
        else
            m_portraitObj.SetActive(false);
        if (!string.IsNullOrEmpty(m_dialogs[index].boxSpr))
            m_dialogBoxImg.sprite = VisualNovelManager.Instance.BoxISpr[m_dialogs[index].boxSpr];
        if (!string.IsNullOrEmpty(m_dialogs[index].ellipseSpr))
            m_ellipseImg.sprite = VisualNovelManager.Instance.EllipseSpr[m_dialogs[index].ellipseSpr];
        if (!string.IsNullOrEmpty(m_dialogs[index].arrawSpr))
            m_arrowImg.sprite = VisualNovelManager.Instance.ArrawSpr[m_dialogs[index].arrawSpr];
    }

    private void Update_None()
    {
        Update_Basic(m_dialogIndex);

        StartCoroutine(Type_Text(m_dialogTxt, m_arrowObj));
        m_dialogIndex++;
    }

    private void Update_FadeIn()
    {
        Update_Basic(m_dialogIndex + 1);

        m_dialogTxt.text = "";
        UIManager.Instance.Start_FadeIn(1f, Color.black, () => Next_FadeIn());
    }

    private void Next_FadeIn()
    {
        m_dialogIndex++;
        Update_Dialog();
    }

    private void Update_FadeOut()
    {
        if (!string.IsNullOrEmpty(m_dialogs[m_dialogIndex].choiceDialog[0]))
        {   
            UIManager.Instance.Start_FadeOut(1f, Color.black, 
                () => Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>(m_dialogs[m_dialogIndex].choiceDialog[0])), 0f, false);
        }
        else
        {
            // 비어있을 시 페이드 아웃만 진행
            UIManager.Instance.Start_FadeOut(1f, Color.black);
        }

    }

    private void Update_FadeOutIn()
    {
        UIManager.Instance.Start_FadeOut(1f, Color.black, () => Update_FadeIn(), 0.5f, false);
    }

    private void Start_ShootGame()
    {
        UIManager.Instance.Start_FadeOut(1f, Color.black,
            () => VisualNovelManager.Instance.Change_Level(LEVELSTATE.LS_SHOOTGAME), 0.5f, false);
    }

    private void Start_ChaseGame()
    {
        UIManager.Instance.Start_FadeOut(1f, Color.black,
            () => VisualNovelManager.Instance.Change_Level(LEVELSTATE.LS_CHASEGAME), 0.5f, false);
    }

    private void Play_ChaseGame()
    {
        UIManager.Instance.Start_FadeOut(1f, Color.black,
            () => VisualNovelManager.Instance.Play_ChaseGame(), 0.5f, false);
    }

    private void Update_Shaking()
    {
        Update_None();

        // 카메라 쉐이킹
    }

#region Etc
    private void Update_Standing(int index)
    {
        switch (m_dialogs[index].standingSpr.Count)
        {
            case 0:
                m_standingObj[0].SetActive(false);
                m_standingObj[1].SetActive(false);
                m_standingObj[2].SetActive(false);
                break;

            case 1:
                m_standingObj[0].SetActive(true);
                m_standingObj[0].transform.localPosition = new Vector3(0.0f, -460.0f, 0.0f);
                m_standingImg[0].sprite = VisualNovelManager.Instance.StandingSpr[m_dialogs[index].standingSpr[0]];
                m_standingObj[1].SetActive(false);
                m_standingObj[2].SetActive(false);
                break;

            case 2:
                m_standingObj[0].SetActive(true);
                m_standingObj[0].transform.localPosition = new Vector3(-300.0f, -460.0f, 0.0f);
                m_standingImg[0].sprite = VisualNovelManager.Instance.StandingSpr[m_dialogs[index].standingSpr[0]];
                m_standingObj[1].SetActive(true);
                m_standingObj[1].transform.localPosition = new Vector3(300.0f, -460.0f, 0.0f);
                m_standingImg[1].sprite = VisualNovelManager.Instance.StandingSpr[m_dialogs[index].standingSpr[1]];
                m_standingObj[2].SetActive(false);
                break;

            case 3:
                m_standingObj[0].SetActive(true);
                m_standingObj[0].transform.localPosition = new Vector3(-500.0f, -460.0f, 0.0f);
                m_standingImg[0].sprite = VisualNovelManager.Instance.StandingSpr[m_dialogs[index].standingSpr[0]];
                m_standingObj[1].SetActive(true);
                m_standingObj[1].transform.localPosition = new Vector3(0.0f, -460.0f, 0.0f);
                m_standingImg[1].sprite = VisualNovelManager.Instance.StandingSpr[m_dialogs[index].standingSpr[1]];
                m_standingObj[2].SetActive(true);
                m_standingObj[2].transform.localPosition = new Vector3(500.0f, -460.0f, 0.0f);
                m_standingImg[2].sprite = VisualNovelManager.Instance.StandingSpr[m_dialogs[index].standingSpr[2]];
                break;
        }
    }

    private void Update_Button()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            Click_Button(m_choiceIndex);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_choiceIndex--;
            if (m_choiceIndex < 0)
                m_choiceIndex = m_choice_Button.Count - 1;
            Set_Button();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_choiceIndex++;
            if (m_choiceIndex > m_choice_Button.Count - 1)
                m_choiceIndex = 0;
            Set_Button();
        }
    }

    IEnumerator Type_Text(TMP_Text currentText, GameObject arrow)
    {
        m_isTyping = true;
        m_cancelTyping = false;

        currentText.text = "";
        foreach (char letter in m_dialogs[m_dialogIndex].dialogText.ToCharArray())
        {
            if (m_cancelTyping)
            {
                currentText.text = m_dialogs[m_dialogIndex - 1].dialogText;
                break;
            }

            currentText.text += letter;
            yield return new WaitForSeconds(m_typeSpeed);
        }

        m_isTyping = false;
        StartCoroutine(Use_Arrow(arrow));

        // 타이핑 끝난 상태일 시 선택지 생성
        if (0 < m_dialogs[m_dialogIndex - 1].choiceText.Count)
            Create_ChoiceButton();

        // 다이얼로그 이벤트가 호감도 증가일 시 타이핑 종료 시 호감도 증가
        if (m_dialogs[m_dialogIndex - 1].dialogEvent == DIALOGEVENT_TYPE.DET_LIKEADD)
        {
            VisualNovelManager.Instance.NpcHeart[(int)m_dialogs[m_dialogIndex - 1].owner]++;
            m_heartScr.Set_Owner(m_dialogs[m_dialogIndex - 1].owner);
        }
    }

    IEnumerator Use_Arrow(GameObject arrow)
    {
        while (false == m_isTyping)
        {
            arrow.SetActive(!arrow.activeSelf);
            yield return new WaitForSeconds(m_arrowSpeed);
        }
    }

    private void Create_ChoiceButton()
    {
        m_darkPanelObj.SetActive(true);

        // 선택지 버튼 생성
        for (int i = 0; i < m_dialogs[m_dialogIndex - 1].choiceText.Count; ++i)
        {
            int ButtonIndex = i + 1; // 버튼 고유 인덱스

            GameObject Clone = Instantiate(m_choiceButtonPre);
            if (Clone)
            {
                Clone.transform.SetParent(gameObject.transform);
                Clone.transform.localPosition = new Vector3(0f, (130 + (i * -130)), 0f); // 130 / 0 / -130
                Clone.transform.localScale    = new Vector3(1f, 1f, 1f);

                ButtonChoice_VN ButtonChoice = Clone.GetComponent<ButtonChoice_VN>();
                ButtonChoice.ButtonIndex = i;
                ButtonChoice.Ownerdialog = this;

                TMP_Text TextCom = Clone.GetComponentInChildren<TMP_Text>();
                if (TextCom)
                {
                    TextCom.text = m_dialogs[m_dialogIndex - 1].choiceText[i];

                    Button button = Clone.GetComponent<Button>();
                    if (button) // 이벤트 핸들러 추가
                        button.onClick.AddListener(() => Click_Button(ButtonIndex));

                    m_choice_Button.Add(Clone);
                }
            }
        }

        m_choiceIndex = 0;
        m_choice_Button[m_choiceIndex].GetComponent<Image>().sprite = VisualNovelManager.Instance.ChoiceButtonSpr["UI_VisualNovel_White_ButtonON"];
    }

    public void Enter_Button(int index)
    {
        m_choiceIndex = index;
        Set_Button();
    }

    private void Click_Button(int index)
    {
        switch (m_dialogs[m_dialogIndex - 1].choiceEventType[index - 1])
        {
            case CHOICEEVENT_TYPE.CET_CLOSE: // 다이얼로그 종료
                Close_Dialog();
                break;

            case CHOICEEVENT_TYPE.CET_DIALOG: // 다음 다이얼로그 불러오고 해당 다이얼로그로 이어서 출력
                Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>(m_dialogs[m_dialogIndex - 1].choiceDialog[index - 1])); ;
                break;
        }
    }

    public void Set_Button()
    {
        // 현재 인덱스 버튼을 제외한 모든 버튼 Off 이미지로 초기화
        for (int i = 0; i < m_choice_Button.Count; ++i)
        {
            if (i == m_choiceIndex)
                m_choice_Button[i].GetComponent<Image>().sprite = VisualNovelManager.Instance.ChoiceButtonSpr["UI_VisualNovel_White_ButtonON"]; // 버튼 On
            else
                m_choice_Button[i].GetComponent<Image>().sprite = VisualNovelManager.Instance.ChoiceButtonSpr["UI_VisualNovel_White_ButtonOFF"]; // 버튼 Off
        }
    }

    public void Start_Dialog(DialogData_VN[] dialogs = null)
    {
        m_dialogs = dialogs;

        m_isTyping = false;
        m_cancelTyping = false;
        m_dialogIndex = 0;
        m_choiceIndex = 0;

        m_standingObj[0].SetActive(false);
        m_standingObj[1].SetActive(false);
        m_standingObj[2].SetActive(false);

        m_darkPanelObj.SetActive(false);

        for (int i = 0; i < m_choice_Button.Count; ++i)
            Destroy(m_choice_Button[i]);
        m_choice_Button.Clear();

        gameObject.SetActive(true);
        m_backgroundObj.SetActive(true);

        Update_Dialog(false);
    }

    private void Close_Dialog()
    {
        m_standingObj[0].SetActive(false);
        m_standingObj[1].SetActive(false);
        m_standingObj[2].SetActive(false);

        m_darkPanelObj.SetActive(false);

        for (int i = 0; i < m_choice_Button.Count; ++i)
            Destroy(m_choice_Button[i]);
        m_choice_Button.Clear();

        m_dialogBoxObj.SetActive(false);
    }

    public void Close_Background()
    {
        m_backgroundObj.SetActive(false);
    }
#endregion
}
