using System.Collections.Generic;
using UnityEngine;

using VisualNovel;
public class VisualNovelManager : StageManager
{
    public enum LEVELSTATE { LS_DAY1, LS_DAY2, LS_DAY3BEFORE, LS_DAY3SHOOTGAME, LS_DAY3AFTER, LS_DAY3CHASEGAME, LS_END }; // 다이얼로그 - 새총 - 다이얼로그 - 추격 => 윈도우 레벨
    public enum OWNERTYPE { OT_WHITE, OT_BLUE, OT_YELLOW, OT_PINK, OT_END };

    private GameObject m_dialog;
    private GameObject m_likeabilityPanel;
    private NpcLike[]  m_likeabilityHeartPanel;
    private int[] m_npcHeart;

    public GameObject Dialog => m_dialog;
    public GameObject LikeabilityPanel => m_likeabilityPanel;
    public int[] NpcHeart
    {
        get => m_npcHeart;
        set => m_npcHeart = value;
    }

    #region Resource
    private Dictionary<string, Sprite> m_backgroundSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_standingSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_choiceButtonSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_heartSpr = new Dictionary<string, Sprite>();

    public Dictionary<string, Sprite> BackgroundSpr { get => m_backgroundSpr; }
    public Dictionary<string, Sprite> StandingSpr { get => m_standingSpr; }
    public Dictionary<string, Sprite> ChoiceButtonSpr { get => m_choiceButtonSpr; }
    public Dictionary<string, Sprite> HeartSpr { get => m_heartSpr; }
    #endregion

    public VisualNovelManager() : base()
    {
        m_stageLevel = STAGE.LEVEL_VISUALNOVEL;
        m_sceneName = "VisualNovel";
    }

    protected override void Load_Resource()
    {
        // 배경 이미지 할당
        m_backgroundSpr.Add("BackGround_SchoolGate",    GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_SchoolGate"));    // 학교 교문
        m_backgroundSpr.Add("BackGround_SchoolClass",   GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_SchoolClass"));   // 교실
        m_backgroundSpr.Add("BackGround_SchoolHallway", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_SchoolHallway")); // 교실 복도
        m_backgroundSpr.Add("BackGround_BandRoom",      GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_BandRoom"));      // 동아리실
        m_backgroundSpr.Add("BackGround_Roadside",      GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_Roadside"));      // 길가1
        m_backgroundSpr.Add("BackGround_Festival",      GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_Festival"));      // 길가2(축제)
        m_backgroundSpr.Add("BackGround_Living",        GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_Living"));        // 아야카의 집(거실)
        m_backgroundSpr.Add("BackGround_Bathroom",      GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_Bathroom"));      // 아야ㅏ의 집(화장실)
        m_backgroundSpr.Add("BackGround_Dark",          GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_Dark"));

        // 컷씬 이미지 할당
        m_backgroundSpr.Add("C01",  GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_C01"));
        m_backgroundSpr.Add("C02",  GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_C02"));
        m_backgroundSpr.Add("C03",  GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_C03"));
        m_backgroundSpr.Add("C04",  GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_C04"));
        m_backgroundSpr.Add("C05",  GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_C05"));
        m_backgroundSpr.Add("C06",  GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_C06"));
        m_backgroundSpr.Add("MI01", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_MI01"));
        m_backgroundSpr.Add("MI02", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_MI02"));
        m_backgroundSpr.Add("MI03", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_MI03"));
        m_backgroundSpr.Add("SA01", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_SA01"));
        m_backgroundSpr.Add("SA02", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_SA02"));
        m_backgroundSpr.Add("SA03", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_SA03"));
        m_backgroundSpr.Add("KI01", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_KI01"));
        m_backgroundSpr.Add("KI02", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_KI02"));
        m_backgroundSpr.Add("KI03", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_KI03"));
        m_backgroundSpr.Add("DA01", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_DA01"));
        m_backgroundSpr.Add("DA09", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/CUT_DA09"));

        m_backgroundSpr.Add("AyakaEye", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/STMInats01_STHINA01"));
        m_backgroundSpr.Add("TSAyaka01", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/TSAyaka01"));
        m_backgroundSpr.Add("BackGround_BandRoomAyaka", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/BackGround_BandRoom_Ayaka"));

        m_backgroundSpr.Add("DA7_1", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/DA7_1"));
        m_backgroundSpr.Add("DA7_2", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/DA7_2"));
        m_backgroundSpr.Add("DA7_3", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/DA7_3"));

        // 스탠딩 이미지 할당 
        m_standingSpr.Add("KI00", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/KI00"));
        m_standingSpr.Add("KI01", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/KI01"));
        m_standingSpr.Add("KI02", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/KI02"));
        m_standingSpr.Add("KI03", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/KI03"));
        m_standingSpr.Add("KI04", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/KI04"));
        m_standingSpr.Add("KI05", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/KI05"));
        m_standingSpr.Add("KI06", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/KI06"));
        m_standingSpr.Add("KI07", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/KI07"));
        m_standingSpr.Add("KI08", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/KI08"));
        m_standingSpr.Add("KI09", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/KI09"));
        m_standingSpr.Add("KI10", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/KI10"));
        m_standingSpr.Add("KI11", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/KI11"));
        m_standingSpr.Add("KI04_1", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/KI04_1"));

        m_standingSpr.Add("KIE601", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/Effect/Event6/SYAYAKA01"));
        m_standingSpr.Add("KIE602", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/Effect/Event6/SYAYAKA02"));
        m_standingSpr.Add("KIE603", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/Effect/Event6/SYAYAKA03"));
        m_standingSpr.Add("KIE604", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/Effect/Event6/SYAYAKA04"));
        m_standingSpr.Add("KIE605", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/KI/Effect/Event6/SYAYAKA05"));
        //
        m_standingSpr.Add("MI00", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/MI/MI00"));
        m_standingSpr.Add("MI01", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/MI/MI01"));
        m_standingSpr.Add("MI02", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/MI/MI02"));
        m_standingSpr.Add("MI03", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/MI/MI03"));
        m_standingSpr.Add("MI04", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/MI/MI04"));
        m_standingSpr.Add("MI05", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/MI/MI05"));
        m_standingSpr.Add("MI06", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/MI/MI06"));

        m_standingSpr.Add("MIE301", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/MI/Effect/Event3/SYMInats01"));
        m_standingSpr.Add("MIE302", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/MI/Effect/Event3/SYMInats02"));
        m_standingSpr.Add("MIE303", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/MI/Effect/Event3/SYMInats03"));
        m_standingSpr.Add("MIE304", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/MI/Effect/Event3/SYMInats04"));
        m_standingSpr.Add("MIE305", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/MI/Effect/Event3/SYMInats05"));
        //
        m_standingSpr.Add("SA00", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/SA/SA00"));
        m_standingSpr.Add("SA01", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/SA/SA01"));
        m_standingSpr.Add("SA02", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/SA/SA02"));
        m_standingSpr.Add("SA03", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/SA/SA03"));
        m_standingSpr.Add("SA04", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/SA/SA04"));
        m_standingSpr.Add("SA05", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/SA/SA05"));
        m_standingSpr.Add("SA06", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/SA/SA06"));
        m_standingSpr.Add("SA07", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Character/SA/SA07"));

        // 버튼 이미지 할당
        m_choiceButtonSpr.Add("UI_VisualNovel_White_ButtonOFF", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Button/UI_VisualNovel_ChooseButton_OFF"));
        m_choiceButtonSpr.Add("UI_VisualNovel_White_ButtonON",  GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Button/UI_VisualNovel_ChooseButton_ON"));

        // 하트 이미지 할당
        m_heartSpr.Add("UI_VisualNovel_Blue_FriendshipHeartOFF",   GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Friendship_Heart_Blue_OFF"));
        m_heartSpr.Add("UI_VisualNovel_Blue_FriendshipHeartON",    GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Friendship_Heart_Blue_ON"));
        m_heartSpr.Add("UI_VisualNovel_Pink_FriendshipHeartOFF",   GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Friendship_Heart_Pink_OFF"));
        m_heartSpr.Add("UI_VisualNovel_Pink_FriendshipHeartON",    GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Friendship_Heart_Pink_ON"));
        m_heartSpr.Add("UI_VisualNovel_Yellow_FriendshipHeartOFF", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Friendship_Heart_Yellow_OFF"));
        m_heartSpr.Add("UI_VisualNovel_Yellow_FriendshipHeartON",  GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Friendship_Heart_Yellow_ON"));
    }

    public override void Enter_Stage()
    {
        base.Enter_Stage();
    }

    protected override void Load_Scene()
    {
        // 변수 할당
        GameObject canvas = GameObject.Find("Canvas");
        m_dialog           = canvas.transform.GetChild(0).gameObject;
        m_likeabilityPanel = canvas.transform.GetChild(1).gameObject;
        m_likeabilityHeartPanel = new NpcLike[3];
        m_likeabilityHeartPanel[0] = m_likeabilityPanel.transform.GetChild(0).GetChild(0).GetComponent<NpcLike>();
        m_likeabilityHeartPanel[1] = m_likeabilityPanel.transform.GetChild(0).GetChild(1).GetComponent<NpcLike>();
        m_likeabilityHeartPanel[2] = m_likeabilityPanel.transform.GetChild(0).GetChild(2).GetComponent<NpcLike>();

        // 기본 값 초기화
        m_npcHeart = new int[(int)OWNERTYPE.OT_END];
        for (int i = 0; i < (int)OWNERTYPE.OT_END; i++)
            m_npcHeart[i] = 0;

        // 레벨 초기화
        m_levelController = new LevelController();
        List<Level> levels = new List<Level>
        {
            new Novel_Day1(),
            new Novel_Day2(),
            new Novel_Day3Before(),
            new Novel_Day3Shoot(),
            new Novel_Day3After(),
            new Novel_Day3Chase(),
        };
        for (int i = 0; i < levels.Count; ++i)
            levels[i].Initialize_Level(m_levelController);
        m_levelController.Initialize_Level(levels);

        // 게임 시작
        Cursor.lockState = CursorLockMode.None;
        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => In_Game());
    }

    public override void Update_Stage()
    {
        if (m_levelController == null)
            return;

        m_levelController.Update_Level();
    }

    public override void LateUpdate_Stage()
    {
        if (m_levelController == null)
            return;

        m_levelController.LateUpdate_Level();
    }

    public override void Exit_Stage()
    {
        base.Exit_Stage();
    }

    public override void Set_Pause(bool pause, bool Setcursur)
    {
        base.Set_Pause(pause, Setcursur);

        if (LevelController == null || LevelController.Curlevel == -1)
            return;
        switch(LevelController.Curlevel)
        {
            case (int)LEVELSTATE.LS_DAY3CHASEGAME:
                Novel_Day3Chase chase = LevelController.Get_CurrentLevel<Novel_Day3Chase>();
                if (chase == null) return;
                chase.Player.Stop_Player(pause);
                chase.Yandere.Stop_Yandere(pause);
                break;
        }
    }


    public void Active_Popup()
    {
        if (m_likeabilityPanel == null || m_levelController.Curlevel == (int)LEVELSTATE.LS_DAY3CHASEGAME)
            return;

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