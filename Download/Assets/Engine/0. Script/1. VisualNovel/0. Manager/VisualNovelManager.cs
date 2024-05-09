using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class VisualNovelManager : MonoBehaviour
{
    public enum LEVELSTATE { LS_NOVELBEGIN, LS_SHOOTGAME, LS_NOVELEND, LS_CHASEGAME, LS_END }; // 미연시 -> 사격 -> 미연시 -> 추격 => 서부 레벨
    public enum NPCTYPE { OT_WHITE, OT_BLUE, OT_YELLOW, OT_PINK, OT_END };


    private static VisualNovelManager m_instance = null;

    [SerializeField] private LEVELSTATE m_startState = LEVELSTATE.LS_NOVELBEGIN;

    [Header("[ LS_START ]")]
    [SerializeField] GameObject m_StartPanel;

    [Header("[ LS_NOVEL ]")]
    [SerializeField] private GameObject m_dialog;
    [SerializeField] private GameObject m_likeabilityPanel;
    [SerializeField] private NpcLike[]  m_likeabilityHeartPanel;

    [Header("[ LS_SHOOT ]")]
    [SerializeField] private GameObject m_shootGame;
    [SerializeField] private TMP_Text   m_countTxt;
    [SerializeField] private ShootContainerBelt m_containerBelt;

    [Header("[ LS_CHASE ]")]
    [SerializeField] private GameObject m_chaseGame;
    [SerializeField] private TMP_Text   m_cdCountTxt;
    [SerializeField] private GameObject m_playerObj;
    [SerializeField] private Transform[] m_RandomPos;

    private int[] m_npcHeart;
    private LevelController m_levelController = null;


    public static VisualNovelManager Instance => m_instance;
    public int[] NpcHeart
    {
        get => m_npcHeart;
        set => m_npcHeart = value;
    }
    public LevelController LevelController => m_levelController; // 읽기
    public GameObject Dialog => m_dialog;
    public GameObject ShootGame => m_shootGame;
    public TMP_Text CountTxt => m_countTxt;
    public ShootContainerBelt Container => m_containerBelt;
    public GameObject ChaseGame => m_chaseGame;
    public TMP_Text CdTxt => m_cdCountTxt;
    public GameObject PlayerObj => m_playerObj;
    public Transform[] RandomPos => m_RandomPos;

    #region Resource
    private Dictionary<string, Sprite> m_backgroundSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_standingSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_portraitSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_boxISpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_ellipseSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_arrawSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_choiceButtonSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_heartSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, TMP_FontAsset> m_fontAst = new Dictionary<string, TMP_FontAsset>();

    public Dictionary<string, Sprite> BackgroundSpr { get { return m_backgroundSpr; }}
    public Dictionary<string, Sprite> StandingSpr { get { return m_standingSpr; }}
    public Dictionary<string, Sprite> PortraitSpr { get { return m_portraitSpr; }}
    public Dictionary<string, Sprite> BoxISpr { get { return m_boxISpr; }}
    public Dictionary<string, Sprite> EllipseSpr { get { return m_ellipseSpr; }}
    public Dictionary<string, Sprite> ArrawSpr { get { return m_arrawSpr; } }
    public Dictionary<string, Sprite> ChoiceButtonSpr { get { return m_choiceButtonSpr; }}
    public Dictionary<string, Sprite> HeartSpr { get { return m_heartSpr; } }
    public Dictionary<string, TMP_FontAsset> FontAst { get { return m_fontAst; } }
    #endregion

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;

        Create_NpcHeart();
        Load_Resource();

        m_levelController = new LevelController();

        List<Level> levels = new List<Level>();
        levels.Add(new Novel_Begin(m_levelController));
        levels.Add(new Novel_Shoot(m_levelController));
        levels.Add(new Novel_End(m_levelController));
        levels.Add(new Novel_Chase(m_levelController));

        m_levelController.Initialize_Level(levels);
    }

    private void Start()
    {
    }

    private void Update()
    {
        m_levelController.Update_Level();
    }

    private void Create_NpcHeart()
    {
        m_npcHeart = new int[(int)NPCTYPE.OT_END];
        for (int i = 0; i < (int)NPCTYPE.OT_END; i++)
            m_npcHeart[i] = 0;
    }

    private void Load_Resource()
    {
        // 배경 이미지 할당
        m_backgroundSpr.Add("BackGround_SchoolWay",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_SchoolWay"));
        m_backgroundSpr.Add("BackGround_School",      Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_School"));
        m_backgroundSpr.Add("BackGround_NightMarket", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_NightMarket"));
        m_backgroundSpr.Add("BackGround_Festival",    Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_Festival"));
        m_backgroundSpr.Add("BackGround_PlayerHome",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_PlayerHome"));
        m_backgroundSpr.Add("BackGround_PinkHome",    Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_PinkHome"));
        m_backgroundSpr.Add("BackGround_Cellar",      Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_Cellar"));
        
        // 스탠딩 이미지 할당
        m_standingSpr.Add("Blue",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Standing/Blue"));
        m_standingSpr.Add("Pink",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Standing/Pink"));
        m_standingSpr.Add("Yellow", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Standing/Yellow"));

        // 프로필 이미지 할당
        m_portraitSpr.Add("Blue",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Portrait/Blue"));
        m_portraitSpr.Add("Pink",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Portrait/Pink"));
        m_portraitSpr.Add("Yellow", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Portrait/Yellow"));
        m_portraitSpr.Add("White",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Portrait/White"));

        // 박스 이미지 할당
        m_boxISpr.Add("UI_VisualNovel_Blue_ChatBox",        Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/ChatBox/UI_VisualNovel_Blue_ChatBox"));
        m_boxISpr.Add("UI_VisualNovel_Pink_ChatBox",        Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/ChatBox/UI_VisualNovel_Pink_ChatBox"));
        m_boxISpr.Add("UI_VisualNovel_White_ChatBox",       Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/ChatBox/UI_VisualNovel_White_ChatBox"));
        m_boxISpr.Add("UI_VisualNovel_Yellow_ChatBox",      Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/ChatBox/UI_VisualNovel_Yellow_ChatBox"));
        m_boxISpr.Add("UI_VisualNovel_Blue_NarrationBox",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/NarrationBox/UI_VisualNovel_Blue_NarrationBox"));
        m_boxISpr.Add("UI_VisualNovel_Pink_NarrationBox",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/NarrationBox/UI_VisualNovel_Pink_NarrationBox"));
        m_boxISpr.Add("UI_VisualNovel_White_NarrationBox",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/NarrationBox/UI_VisualNovel_White_NarrationBox"));
        m_boxISpr.Add("UI_VisualNovel_Yellow_NarrationBox", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/NarrationBox/UI_VisualNovel_Yellow_NarrationBox"));

        // 원 아이콘 이미지 할당
        m_ellipseSpr.Add("UI_VisualNovel_Blue_Ellipse",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Blue_Ellipse"));
        m_ellipseSpr.Add("UI_VisualNovel_Pink_Ellipse",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Pink_Ellipse"));
        m_ellipseSpr.Add("UI_VisualNovel_White_Ellipse",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_White_Ellipse"));
        m_ellipseSpr.Add("UI_VisualNovel_Yellow_Ellipse", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Yellow_Ellipse"));

        // 넘김표시 이미지 할당
        m_arrawSpr.Add("UI_VisualNovel_Blue_Ellipse",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Blue_Ellipse"));
        m_arrawSpr.Add("UI_VisualNovel_Pink_Ellipse",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Pink_Ellipse"));
        m_arrawSpr.Add("UI_VisualNovel_White_Ellipse",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_White_Ellipse"));
        m_arrawSpr.Add("UI_VisualNovel_Yellow_Ellipse", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Yellow_Ellipse"));

        // 버튼 이미지 할당
        m_choiceButtonSpr.Add("UI_VisualNovel_White_ButtonOFF", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Button/UI_VisualNovel_White_ButtonOFF"));
        m_choiceButtonSpr.Add("UI_VisualNovel_White_ButtonON",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Button/UI_VisualNovel_White_ButtonON"));

        // 하트 이미지 할당
        m_heartSpr.Add("UI_VisualNovel_Blue_FriendshipHeartOFF",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Blue_FriendshipHeartOFF"));
        m_heartSpr.Add("UI_VisualNovel_Blue_FriendshipHeartON",    Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Blue_FriendshipHeartON"));
        m_heartSpr.Add("UI_VisualNovel_Pink_FriendshipHeartOFF",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Pink_FriendshipHeartOFF"));
        m_heartSpr.Add("UI_VisualNovel_Pink_FriendshipHeartON",    Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Pink_FriendshipHeartON"));
        m_heartSpr.Add("UI_VisualNovel_Yellow_FriendshipHeartOFF", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Yellow_FriendshipHeartOFF"));
        m_heartSpr.Add("UI_VisualNovel_Yellow_FriendshipHeartON",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Yellow_FriendshipHeartON"));

        // 폰트 에셋 할당
        m_fontAst.Add("VN_Basic_Blue",    Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Blue/VN_Basic_Blue"));
        m_fontAst.Add("VN_Basic_RBlue",   Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Blue/VN_Basic_RBlue"));
        m_fontAst.Add("VN_Basic_Pink",    Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Pink/VN_Basic_Pink"));
        m_fontAst.Add("VN_Basic_RPink",   Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Pink/VN_Basic_RPink"));
        m_fontAst.Add("VN_Basic_White",   Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/White/VN_Basic_White"));
        m_fontAst.Add("VN_Basic_RWhite",  Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/White/VN_Basic_RWhite"));
        m_fontAst.Add("VN_Basic_Yellow",  Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Yellow/VN_Basic_Yellow"));
        m_fontAst.Add("VN_Basic_RYellow", Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Yellow/VN_Basic_RYellow"));
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

    public void Active_Popup()
    {
        // 호감도창 비/ 활성화
        m_likeabilityPanel.SetActive(!m_likeabilityPanel.activeSelf);
        if (true == m_likeabilityPanel.activeSelf)
        {
            // 호감도창 NPC정보 업데이트
            for (int i = 0; i < m_likeabilityHeartPanel.Length; i++)
                m_likeabilityHeartPanel[i].Update_Heart();
        }
    }
}