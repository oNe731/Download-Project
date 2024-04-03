using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Input_Name : MonoBehaviour
{
    [Header("Namebox")]
    [SerializeField] private GameObject m_namebox;
    [SerializeField] private TMP_InputField m_playerNameInput;

    [Header("Popupbox")]
    [SerializeField] private GameObject m_popup;
    [SerializeField] private TMP_Text m_guide;

    private string[] m_randomnames;

    private void Start()
    {
        // 추천 닉네임
        m_randomnames = new string[10];
        m_randomnames[0] = "나비";
        m_randomnames[1] = "곰";
        m_randomnames[2] = "호랑이";
        m_randomnames[3] = "뱀";
        m_randomnames[4] = "토끼";
        m_randomnames[5] = "여우";
        m_randomnames[6] = "하마";
        m_randomnames[7] = "얼룩말";
        m_randomnames[8] = "퓨마";
        m_randomnames[9] = "쿼카";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            Select_Name();
    }

    public void Select_Name()
    {
        if (m_playerNameInput.text.Length <= 0)
            return;

        m_guide.text = "\"" + m_playerNameInput.text + "\"" + "로 결정하시겠습니까?";
        m_popup.SetActive(true);
    }

    public void Random_Name()
    {
        float randomNumber = Random.Range(0.0f, 9.9f);
        m_playerNameInput.text = m_randomnames[(int)randomNumber];
    }

    public void Popup_Yes()
    {
        GameManager.Instance.PlayerName = m_playerNameInput.text;
        gameObject.SetActive(false);
    }

    public void Popup_No()
    {
        m_popup.SetActive(false);
    }
}
