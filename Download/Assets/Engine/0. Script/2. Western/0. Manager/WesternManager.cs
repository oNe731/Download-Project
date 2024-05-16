using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WesternManager : MonoBehaviour
{
    public enum LEVELSTATE
    {  // 게임 로고 -> 인트로컷씬(클릭/스페이스/엔터로 넘김) -> 수배지확인화면 -> 게임 시작
        LS_IntroLv1, LS_MainLv1, LS_PlayLv1,
        LS_IntroLv2, LS_MainLv2, LS_PlayLv2,
        LS_IntroLv3, LS_MainLv3, LS_PlayLv3,
        LS_END
    };


    private static WesternManager m_instance = null;

    [SerializeField] private LEVELSTATE m_startState = LEVELSTATE.LS_IntroLv1;

    [Header("[ LS_START ]")]
    [SerializeField] GameObject m_StartPanel;

    [Header("[ LS_INTRO ]")]
    [SerializeField] GameObject m_introPanel;
    [SerializeField] Dialog_IntroWT m_dialogIntro;

    [Header("[ LS_MAIN ]")]
    [SerializeField] GameObject m_MainPanel;
    [SerializeField] Dialog_PlayWT m_dialogPlay;
    [SerializeField] GameObject m_playButton;

    private bool m_isShoot = false;
    private LevelController m_levelController = null;

    public static WesternManager Instance => m_instance;
    public GameObject IntroPanel => m_introPanel;
    public Dialog_IntroWT DialogIntro => m_dialogIntro;
    public GameObject MainPanel => m_MainPanel;
    public Dialog_PlayWT DialogPlay => m_dialogPlay;
    public GameObject PlayButton => m_playButton;
    public bool IsShoot
    {
        get => m_isShoot;
        set => m_isShoot = value;
    }
    public LevelController LevelController => m_levelController;

    #region Resource
    private Dictionary<string, Sprite> m_backgroundSpr = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> BackgroundSpr { get { return m_backgroundSpr; } }
    #endregion

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;

        Load_Resource();

        m_levelController = new LevelController();

        List<Level> levels = new List<Level>();
        levels.Add(new Western_IntroLv1(m_levelController));
        levels.Add(new Western_MainLv1(m_levelController));
        levels.Add(new Western_PlayLv1(m_levelController));
        levels.Add(new Western_IntroLv2(m_levelController));
        levels.Add(new Western_MainLv2(m_levelController));
        levels.Add(new Western_PlayLv2(m_levelController));
        levels.Add(new Western_IntroLv3(m_levelController));
        levels.Add(new Western_MainLv3(m_levelController));
        levels.Add(new Western_PlayLv3(m_levelController));

        m_levelController.Initialize_Level(levels);
    }

    private void Start()
    {

    }

    private void Update()
    {
        m_levelController.Update_Level();
    }

    private void LateUpdate()
    {
        m_levelController.LateUpdate_Level();
    }

    private void Load_Resource()
    {
        // 배경 이미지 할당
        m_backgroundSpr.Add("Background_01", Resources.Load<Sprite>("1. Graphic/2D/2. Western/UI/IntroChatScript/Background/Background_01"));
        m_backgroundSpr.Add("Background_02", Resources.Load<Sprite>("1. Graphic/2D/2. Western/UI/IntroChatScript/Background/Background_02"));
    }

    public void Button_Start()
    {
        Destroy(m_StartPanel);
        m_levelController.Change_Level((int)m_startState);
    }

    public void Button_Exit()
    {
        SceneManager.LoadScene("Window");
    }

    public void Button_Play()
    {
        m_levelController.Get_CurrentLevel<Western_Main>().Button_Play();
    }
}
