using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

using Western;
public class WesternManager : MonoBehaviour
{
    public enum LEVELSTATE
    {  // 게임 로고 -> 인트로컷씬(클릭/스페이스/엔터로 넘김) -> 수배지확인화면 -> 게임 시작
        LS_IntroLv1, LS_MainLv1, LS_PlayLv1, LS_ClearLv1, 
        LS_IntroLv2, LS_MainLv2, LS_PlayLv2, LS_ClearLv2,
        LS_IntroLv3, LS_MainLv3, LS_PlayLv3, LS_ClearLv3,
        // LS_FINISH
        LS_END
    };


    private static WesternManager m_instance = null;

    [Header("[ LS_INTRO ]")]
    [SerializeField] GameObject m_introPanel;
    [SerializeField] Dialog_IntroWT m_dialogIntro;

    [Header("[ LS_MAIN ]")]
    [SerializeField] GameObject m_MainPanel;
    [SerializeField] Dialog_PlayWT m_dialogPlay;
    [SerializeField] GameObject m_playButton;

    [Header("[ LS_PLAY ]")]
    [SerializeField] HeartUI m_heartUI;
    [SerializeField] StatusBarUI m_statusBarUI;
    [SerializeField] Gun m_gun;

    private bool m_isShoot = false;
    private LevelController m_levelController = null;

    public static WesternManager Instance => m_instance;
    public GameObject IntroPanel => m_introPanel;
    public Dialog_IntroWT DialogIntro => m_dialogIntro;
    public GameObject MainPanel => m_MainPanel;
    public Dialog_PlayWT DialogPlay => m_dialogPlay;
    public GameObject PlayButton => m_playButton;
    public HeartUI HeartUI => m_heartUI;
    public StatusBarUI StatusBarUI => m_statusBarUI;
    public Gun Gun => m_gun;
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

        m_levelController = gameObject.AddComponent<LevelController>();

        List<Level> levels = new List<Level>
        { 
            gameObject.AddComponent<Western_IntroLv1>(),
            gameObject.AddComponent<Western_MainLv1>(),
            gameObject.AddComponent<Western_PlayLv1>(),
            gameObject.AddComponent<Western_ClearLv1>(),

            gameObject.AddComponent<Western_IntroLv2>(),
            gameObject.AddComponent<Western_MainLv2>(),
            gameObject.AddComponent<Western_PlayLv2>(),
            gameObject.AddComponent<Western_ClearLv2>(),

            gameObject.AddComponent<Western_IntroLv3>(),
            gameObject.AddComponent<Western_MainLv3>(),
            gameObject.AddComponent<Western_PlayLv3>(),
            gameObject.AddComponent<Western_ClearLv3>(),
        };

        gameObject.GetComponent<Western_IntroLv1>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Western_MainLv1>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Western_PlayLv1>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Western_ClearLv1>().Initialize_Level(m_levelController);

        gameObject.GetComponent<Western_IntroLv2>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Western_MainLv2>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Western_PlayLv2>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Western_ClearLv2>().Initialize_Level(m_levelController);

        gameObject.GetComponent<Western_IntroLv3>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Western_MainLv3>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Western_PlayLv3>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Western_ClearLv3>().Initialize_Level(m_levelController);

        m_levelController.Initialize_Level(levels);
    }

    private void Start()
    {
        Camera.main.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("2. Sound/2. Western/BGM/메인화면 BGM");
        Camera.main.GetComponent<AudioSource>().Play();

        GameManager.Instance.UI.Start_FadeIn(1f, Color.black);
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

    public void Button_Play()
    {
        m_levelController.Get_CurrentLevel<Western_Main>().Button_Play();
    }
}
