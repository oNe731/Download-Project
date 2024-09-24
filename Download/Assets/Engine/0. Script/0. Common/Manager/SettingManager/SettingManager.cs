using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    private GameObject m_panel; // 스테이지마다 세부 옵션이 다름

    private bool m_isOpen = false;
    private bool m_preIsGame;
    private CursorLockMode m_preCursorMode;

    private Slider m_soundBgm;
    private Slider m_soundEffect;

    private Slider m_luminosity;

    public bool IsOpen => m_isOpen;


    private void Start()
    {
        // 설정 패널 생성
        m_panel = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Common/SettingCanvas", transform);
        m_panel.SetActive(false);

        // 변수 할당
        m_panel.transform.GetChild(2).GetChild(0).GetComponent<Button>().onClick.AddListener(Button_Window);
        Transform basicPanel = m_panel.transform.Find("Panel_Basic");
        m_soundBgm = basicPanel.GetChild(0).GetComponent<Slider>();
        m_soundBgm.onValueChanged.AddListener(Change_BGMSliderValue); // 슬라이더 값이 변경될 때마다 OnSliderValueChanged 함수 호출
        m_soundBgm.value = GameManager.Ins.Sound.BgmSound;
        m_soundEffect = basicPanel.GetChild(1).GetComponent<Slider>();
        m_soundEffect.onValueChanged.AddListener(Change_EffectSliderValue);
        m_soundEffect.value = GameManager.Ins.Sound.EffectSound;
        GameManager.Ins.Sound.Update_AllAudioSources();

        Transform horrorPanel = m_panel.transform.Find("Panel_Horror");
        m_luminosity = horrorPanel.GetChild(0).GetComponent<Slider>();
        m_luminosity.onValueChanged.AddListener(Change_LuminositySliderValue);
        m_luminosity.value = RenderSettings.ambientLight.r;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
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


            // 레벨별로 각 세팅 활성화
            string basicPanelName = "Panel_Basic";
            string stagePanelName = "";
            switch(GameManager.Ins.CurStage.StageLevel)
            {
                case StageManager.STAGE.LEVEL_HORROR:
                    stagePanelName = "Panel_Horror";
                    break;
            }

            m_panel.SetActive(true);
            for (int i = 0; i < m_panel.transform.childCount; i++)
            {
                Transform child = m_panel.transform.GetChild(i);
                if (child.name == basicPanelName || child.name == stagePanelName)
                    child.gameObject.SetActive(true);
                else
                    child.gameObject.SetActive(false);
            }
        }
        else
        {
            m_isOpen = false;

            Cursor.lockState = m_preCursorMode;
            GameManager.Ins.Set_Pause(!m_preIsGame, false);

            m_panel.SetActive(false);
        }
    }

    public void Button_Window()
    {
        GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WINDOW);
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
