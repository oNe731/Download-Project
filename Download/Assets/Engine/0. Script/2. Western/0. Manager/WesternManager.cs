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

    [Header("[ LS_START ]")]
    [SerializeField] GameObject m_StartPanel;

    [Header("[ LS_INTRO ]")]
    [SerializeField] Dialog_WT m_dialog;


    private LevelController m_levelController = null;

    public static WesternManager Instance => m_instance;
    public Dialog_WT Dialog => m_dialog;
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

    private void Load_Resource()
    {
        // 배경 이미지 할당
        m_backgroundSpr.Add("Background_01", Resources.Load<Sprite>("1. Graphic/2D/2. Western/UI/ChatScript/Background/Background_01"));
        m_backgroundSpr.Add("Background_02", Resources.Load<Sprite>("1. Graphic/2D/2. Western/UI/ChatScript/Background/Background_02"));
    }

    public void Button_Start()
    {
        Destroy(m_StartPanel);
        m_levelController.Change_Level((int)LEVELSTATE.LS_IntroLv1); // LS_IntroLv1
    }

    public void Button_Exit()
    {
        SceneManager.LoadScene("Window");
    }
}
