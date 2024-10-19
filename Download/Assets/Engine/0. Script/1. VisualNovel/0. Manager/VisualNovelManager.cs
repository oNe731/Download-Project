using System.Collections.Generic;
using UnityEngine;

using VisualNovel;
public class VisualNovelManager : StageManager
{
    public enum LEVELSTATE { LS_NOVELBEGIN, LS_SHOOTGAME, LS_NOVELEND, LS_CHASEGAME, LS_END }; // 다이얼로그 - 새총 - 다이얼로그 - 추격 => 윈도우 레벨
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
    private Dictionary<string, Sprite> m_cutScene = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_standingSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_choiceButtonSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_heartSpr = new Dictionary<string, Sprite>();

    public Dictionary<string, Sprite> BackgroundSpr { get => m_backgroundSpr; }
    public Dictionary<string, Sprite> CutScene { get => m_cutScene; }
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
        m_backgroundSpr.Add("BackGround_SchoolWay",   GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_SchoolWay"));
        m_backgroundSpr.Add("BackGround_School",      GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_School"));
        m_backgroundSpr.Add("BackGround_NightMarket", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_NightMarket"));
        m_backgroundSpr.Add("BackGround_Festival",    GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_Festival"));
        m_backgroundSpr.Add("BackGround_PlayerHome",  GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_PlayerHome"));
        m_backgroundSpr.Add("BackGround_PinkHome",    GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_PinkHome"));
        m_backgroundSpr.Add("BackGround_Cellar",      GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_Cellar"));

        // 컷씬 이미지 할당
        m_cutScene.Add("UI_VisualNovel_Cut2", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/Cut2"));
        m_cutScene.Add("UI_VisualNovel_Cut5", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/CutScene/Cut5"));

        // 스탠딩 이미지 할당 
        m_standingSpr.Add("Blue",   GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/Character/Blue/Sprite3"));
        m_standingSpr.Add("Yellow", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/Character/Yellow/Sprite1"));
        m_standingSpr.Add("Pink",   GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/Character/Pink/Sprite2"));

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
            new Novel_Begin(),
            new Novel_Shoot(),
            new Novel_End(),
            new Novel_Chase()
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
            case (int)LEVELSTATE.LS_CHASEGAME:
                Novel_Chase chase = LevelController.Get_CurrentLevel<Novel_Chase>();
                if (chase == null) return;
                chase.Player.Stop_Player(pause);
                chase.Yandere.Stop_Yandere(pause);
                break;
        }
    }


    public void Active_Popup()
    {
        if (m_likeabilityPanel == null || m_levelController.Curlevel == (int)LEVELSTATE.LS_CHASEGAME)
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