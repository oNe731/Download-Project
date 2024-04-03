using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{
    [Header("Standing")]
    [SerializeField] private GameObject[] m_standing;

    [Header("Effect")]
    [SerializeField] private GameObject m_darkPanel;

    [Header("DialogBox_Sp")]
    [SerializeField] private GameObject m_dialogBox_Sp;
    [SerializeField] private TMP_Text m_nameTxt_Sp;
    [SerializeField] private TMP_Text m_dialogTxt_Sp;

    [Header("DialogBox_Bs")]
    [SerializeField] private GameObject m_dialogBox_Bs;
    [SerializeField] private GameObject m_portrait_Bs;
    [SerializeField] private TMP_Text m_nameTxt_Bs;
    [SerializeField] private TMP_Text m_dialogTxt_Bs;
    [SerializeField] private DialogHeart m_dialogHeart;

    [Header("Prefab")]
    [SerializeField] private GameObject m_choiceButton;

    [Header("Resource")]
    [SerializeField] private Sprite[] m_standingImage;
    [SerializeField] private Sprite[] m_portraitImage;
    [SerializeField] private DialogData[] m_dialogs;

    private Image[] m_standing_Image;
    private Image m_portrait_Bs_Image = null;

    private bool m_isTyping = false;
    private bool m_cancelTyping = false;
    private int m_dialogIndex = 0;
    private float m_typeSpeed = 0.05f;
    private List<GameObject> m_choice_Button = new List<GameObject>();

    private void Awake()
    {
        m_standing_Image = new Image[m_standing.Length];
        for (int i = 0; i < m_standing.Length; i++)
            m_standing_Image[i] = m_standing[i].GetComponent<Image>();
        m_portrait_Bs_Image = m_portrait_Bs.GetComponent<Image>();
    }

    private void OnEnable()
    {
        m_isTyping     = false;
        m_cancelTyping = false;
        m_dialogIndex  = 0;

        for(int i = 0; i < m_choice_Button.Count; ++i)
            Destroy(m_choice_Button[i]);
        m_choice_Button.Clear();

        Update_Dialog(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            Update_Dialog();
    }

    private void Update_Dialog(bool indexUpdate = true)
    {
        if (m_isTyping && !m_cancelTyping)
            m_cancelTyping = true;
        else if (!m_isTyping)
        {
            // 다이얼로그 진행
            if (m_dialogIndex < m_dialogs.Length - 1)
            {
                if (true == indexUpdate)
                    m_dialogIndex++;

                // 공통 업데이트
                Update_Standing();
                Create_ChoiceButton();

                // 개별 업데이트
                if (DIALOG_TYPE.DT_Simple == m_dialogs[m_dialogIndex].dialogType)
                {
                    m_dialogBox_Sp.SetActive(true);
                    m_dialogBox_Bs.SetActive(false);

                    m_nameTxt_Sp.text = m_dialogs[m_dialogIndex].nameText;

                    // 다이얼로그 업데이트
                    StartCoroutine(TypeText(m_dialogTxt_Sp));
                }
                else if (DIALOG_TYPE.DT_Basic == m_dialogs[m_dialogIndex].dialogType)
                {
                    m_dialogBox_Sp.SetActive(false);
                    m_dialogBox_Bs.SetActive(true);

                    m_nameTxt_Bs.text = m_dialogs[m_dialogIndex].nameText;

                    // 다이얼로그 업데이트
                    StartCoroutine(TypeText(m_dialogTxt_Bs));

                    // 캐릭터 이미지 업데이트
                    m_portrait_Bs_Image.sprite = m_portraitImage[(int)m_dialogs[m_dialogIndex].portraitIndex];

                    // 호감도 업데이트
                    m_dialogHeart.Set_Owner(m_dialogs[m_dialogIndex].Owner);
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

    private void Update_Standing()
    {
        switch(m_dialogs[m_dialogIndex].standingCount)
        {
            case 1:
                m_standing[0].SetActive(true);
                m_standing[0].transform.localPosition = new Vector3(0.0f, -460.0f, 0.0f);
                m_standing_Image[0].sprite = m_standingImage[(int)m_dialogs[m_dialogIndex].standingIndex[0]];
                m_standing[1].SetActive(false);
                m_standing[2].SetActive(false);
                break;

            case 2:
                m_standing[0].SetActive(true);
                m_standing[0].transform.localPosition = new Vector3(-300.0f, -460.0f, 0.0f);
                m_standing_Image[0].sprite = m_standingImage[(int)m_dialogs[m_dialogIndex].standingIndex[0]];
                m_standing[1].SetActive(true);
                m_standing[1].transform.localPosition = new Vector3(300.0f, -460.0f, 0.0f);
                m_standing_Image[1].sprite = m_standingImage[(int)m_dialogs[m_dialogIndex].standingIndex[1]];
                m_standing[2].SetActive(false);
                break;

            case 3:
                m_standing[0].SetActive(true);
                m_standing[0].transform.localPosition = new Vector3(-500.0f, -460.0f, 0.0f);
                m_standing_Image[0].sprite = m_standingImage[(int)m_dialogs[m_dialogIndex].standingIndex[0]];
                m_standing[1].SetActive(true);
                m_standing[1].transform.localPosition = new Vector3(0.0f, -460.0f, 0.0f);
                m_standing_Image[1].sprite = m_standingImage[(int)m_dialogs[m_dialogIndex].standingIndex[1]];
                m_standing[2].SetActive(true);
                m_standing[2].transform.localPosition = new Vector3(500.0f, -460.0f, 0.0f);
                m_standing_Image[2].sprite = m_standingImage[(int)m_dialogs[m_dialogIndex].standingIndex[2]];
                break;

            default:
                m_standing[0].SetActive(false);
                m_standing[1].SetActive(false);
                m_standing[2].SetActive(false);
                break;
        }
    }

    private void Create_ChoiceButton()
    {
        if (0 >= m_dialogs[m_dialogIndex].choiceText.Count)
            return;

        m_darkPanel.SetActive(true);

        // 선택지 버튼 생성
        for (int i = 0; i < m_dialogs[m_dialogIndex].choiceText.Count; ++i)
        {
            int ButtonIndex = i + 1; // 버튼 고유 인덱스

            GameObject Clone = Instantiate(m_choiceButton);
            if (Clone)
            {
                Clone.transform.SetParent(gameObject.transform);
                Clone.transform.localPosition = new Vector3(10f, (-100 * (i)), 0f);
                Clone.transform.localScale    = new Vector3(1f, 1f, 1f);

                TMP_Text TextCom = Clone.GetComponentInChildren<TMP_Text>();
                if (TextCom)
                {
                    TextCom.text = m_dialogs[m_dialogIndex].choiceText[i];

                    Button button = Clone.GetComponent<Button>();
                    if (button) // 이벤트 핸들러 추가
                        button.onClick.AddListener(() => Click_Button(ButtonIndex));

                    m_choice_Button.Add(Clone);
                }
            }
        }
    }

    IEnumerator TypeText(TMP_Text currentText)
    {
        m_isTyping = true;
        m_cancelTyping = false;

        currentText.text = "";

        foreach (char letter in m_dialogs[m_dialogIndex].dialogText.ToCharArray())
        {
            if (m_cancelTyping)
            {
                currentText.text = m_dialogs[m_dialogIndex].dialogText;
                break;
            }

            currentText.text += letter;
            yield return new WaitForSeconds(m_typeSpeed);
        }

        m_isTyping = false;
    }

    private void Click_Button(int index)
    {
        Close_Dialog();

        // 선택된 버튼에 따른 해당 다이얼로그 생성
        //DialogManager.Instance.Create_Dialog(m_dialogs[m_dialogIndex].choiceDialog[index - 1]);
    }

    private void Close_Dialog()
    {
        m_standing[0].SetActive(false);
        m_standing[1].SetActive(false);
        m_standing[2].SetActive(false);

        m_darkPanel.SetActive(false);

        for (int i = 0; i < m_choice_Button.Count; ++i)
            Destroy(m_choice_Button[i]);
        m_choice_Button.Clear();

        m_dialogBox_Sp.SetActive(false);
        m_dialogBox_Bs.SetActive(false);
    }
}
