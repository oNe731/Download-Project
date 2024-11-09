using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    private GameObject m_panel; // ������������ ���� �ɼ��� �ٸ�

    private bool m_isOpen = false;
    private bool m_preIsGame;
    private CursorLockMode m_preCursorMode;

    private Image m_header;

    private Slider m_soundBgm;
    private Slider m_soundEffect;

    private Slider m_luminosity;

    private float m_bgmValue;
    private float m_effectValue;
    private float m_luminosityValue;
    private Dictionary<string, Sprite> m_headerSprite = new Dictionary<string, Sprite>();

    public bool IsOpen => m_isOpen;


    private void Start()
    {
        // ���ҽ� �Ҵ�
        m_headerSprite.Add("Header_VisualNovel", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/0. Window/Window Option/Header/UI_Window_VisualNovelHeader"));
        m_headerSprite.Add("Header_Western",     GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/0. Window/Window Option/Header/UI_Window_WesternHeader"));
        m_headerSprite.Add("Header_Horror",      GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/0. Window/Window Option/Header/UI_Window_HorrorHeader"));

        // ���� �г� ����
        m_panel = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Common/SettingCanvas", transform);
        m_panel.SetActive(false);

        // ���� �Ҵ�
        m_panel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Panel(false)); // x ��ư (â �ݱ�)
        m_panel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Return_Setting());    // �⺻ �������� ����
        m_header = m_panel.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>(); // �̹��� ���
        m_panel.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Button_Window());     // ���� �����ϱ� (������)
        m_panel.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(() => Active_Panel(false)); // ���� ���ư��� (â �ݱ�)

        Transform basicPanel = m_panel.transform.Find("Panel_Basic");
        m_soundBgm = basicPanel.GetChild(0).GetComponent<Slider>();
        m_soundBgm.onValueChanged.AddListener(Change_BGMSliderValue); // �����̴� ���� ����� ������ OnSliderValueChanged �Լ� ȣ��
        m_bgmValue = GameManager.Ins.Sound.BgmSound;
        m_soundBgm.value = m_bgmValue;

        m_soundEffect = basicPanel.GetChild(1).GetComponent<Slider>();
        m_soundEffect.onValueChanged.AddListener(Change_EffectSliderValue);
        m_effectValue = GameManager.Ins.Sound.EffectSound;
        m_soundEffect.value = m_effectValue;
        GameManager.Ins.Sound.Update_AllAudioSources();

        Transform horrorPanel = m_panel.transform.Find("Panel_Horror");
        m_luminosity = horrorPanel.GetChild(0).GetComponent<Slider>();
        m_luminosity.onValueChanged.AddListener(Change_LuminositySliderValue);
        m_luminosityValue = RenderSettings.ambientLight.r;
        m_luminosity.value = m_luminosityValue;
    }

    private void Update()
    {
        if (GameManager.Ins.CurStage.StageLevel == StageManager.STAGE.LEVEL_LOADING || GameManager.Ins.CurStage.StageLevel == StageManager.STAGE.LEVEL_LOGIN || GameManager.Ins.CurStage.StageLevel == StageManager.STAGE.LEVEL_WINDOW)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            Active_Panel(!m_isOpen);
    }

    public void Active_Panel(bool active)
    {
        if(active == true)
        {
            m_isOpen = true;

            m_preIsGame = GameManager.Ins.IsGame;
            m_preCursorMode = Cursor.lockState;

            Cursor.lockState = CursorLockMode.None;
            GameManager.Ins.Set_Pause(true, false);


            // �������� �� ���� Ȱ��ȭ
            switch(GameManager.Ins.CurStage.StageLevel)
            {
                //case StageManager.STAGE.LEVEL_LOADING:
                //    m_header.sprite = m_headerSprite["Header_Window"];
                //    m_panel.transform.GetChild(2).gameObject.SetActive(false); // ���� �г�
                //    break;

                //case StageManager.STAGE.LEVEL_LOGIN:
                //    m_header.sprite = m_headerSprite["Header_Window"];
                //    m_panel.transform.GetChild(2).gameObject.SetActive(false); // ���� �г�
                //    break;

                //case StageManager.STAGE.LEVEL_WINDOW:
                //    m_header.sprite = m_headerSprite["Header_Window"];
                //    m_panel.transform.GetChild(2).gameObject.SetActive(false); // ���� �г�
                //    break;

                case StageManager.STAGE.LEVEL_VISUALNOVEL:
                    m_header.sprite = m_headerSprite["Header_VisualNovel"];
                    m_panel.transform.GetChild(2).gameObject.SetActive(false); // ���� �г�
                    break;

                case StageManager.STAGE.LEVEL_WESTERN:
                    m_header.sprite = m_headerSprite["Header_Western"];
                    m_panel.transform.GetChild(2).gameObject.SetActive(false); // ���� �г�
                    break;

                case StageManager.STAGE.LEVEL_HORROR:
                    m_header.sprite = m_headerSprite["Header_Horror"];
                    m_panel.transform.GetChild(2).gameObject.SetActive(true); // ���� �г�
                    break;
            }

            m_panel.SetActive(true);
        }
        else
        {
            m_isOpen = false;

            Cursor.lockState = m_preCursorMode;
            GameManager.Ins.Set_Pause(!m_preIsGame, false);

            m_panel.SetActive(false);
        }
    }

    public void Button_Window() // ���� �����ϱ�
    {
        // ��������
        StageManager.STAGE level = GameManager.Ins.CurStage.StageLevel;
        if (level == StageManager.STAGE.LEVEL_VISUALNOVEL || level == StageManager.STAGE.LEVEL_WESTERN || level == StageManager.STAGE.LEVEL_HORROR)
        {
            // ������� �̵�
            GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WINDOW);
        }
        else // �ε�, �α���, ������
        {
            // ���� ���� ����
            GameManager.Ins.End_Game();
        }
    }

    public void Return_Setting() // �⺻ �������� ����
    {
        m_soundBgm.value = m_bgmValue;
        Change_BGMSliderValue(m_soundBgm.value);

        m_soundEffect.value = m_effectValue;
        Change_EffectSliderValue(m_soundEffect.value);

        m_luminosity.value = m_luminosityValue;
        Change_LuminositySliderValue(m_luminosity.value);
    }

    public void Change_BGMSliderValue(float value)
    {
        GameManager.Ins.Sound.BgmSound = value;
        GameManager.Ins.Sound.Update_AllAudioSources();
    }

    public void Change_EffectSliderValue(float value)
    {
        GameManager.Ins.Sound.EffectSound = value;
        GameManager.Ins.Sound.Update_AllAudioSources();
    }

    public void Change_LuminositySliderValue(float value)
    {
        RenderSettings.ambientLight = new Color(value, value, value, 1f);
    }
}
