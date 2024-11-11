using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Western;
public class WesternManager : StageManager
{
    public enum LEVELSTATE { 
        LS_IntroLv1, LS_MainLv1, LS_PlayLv1, LS_ClearLv1,
        LS_IntroLv2, LS_PlayLv2,
        LS_END };


    private bool m_isShoot = false;
    private Dictionary<string, Sprite> m_backgroundSpr = new Dictionary<string, Sprite>();
    private Dialog_IntroWT m_dialogIntro;
    private GameObject m_stage;

    public bool IsShoot { get => m_isShoot; set => m_isShoot = value; }
    public Dictionary<string, Sprite> BackgroundSpr { get { return m_backgroundSpr; } }
    public Dialog_IntroWT DialogIntro => m_dialogIntro;
    public GameObject Stage { get => m_stage; set => m_stage = value; }

    public WesternManager() : base()
    {
        m_stageLevel = STAGE.LEVEL_WESTERN;
        m_sceneName = "Western";
    }

    protected override void Load_Resource()
    {
        // 배경 이미지 할당
        m_backgroundSpr.Add("CUT_1A", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/ChatScript/IntroChatScript/Background/CUT_1A"));
        m_backgroundSpr.Add("CUT_1B", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/ChatScript/IntroChatScript/Background/CUT_1B"));
        m_backgroundSpr.Add("CUT_1C", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/ChatScript/IntroChatScript/Background/CUT_1C"));
        m_backgroundSpr.Add("CUT_1D", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/ChatScript/IntroChatScript/Background/CUT_1D"));
        m_backgroundSpr.Add("CUT_2A", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/ChatScript/IntroChatScript/Background/CUT_2A"));
        m_backgroundSpr.Add("CUT_2B", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/ChatScript/IntroChatScript/Background/CUT_2B"));
        m_backgroundSpr.Add("CUT_2C", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/ChatScript/IntroChatScript/Background/CUT_2C"));
        m_backgroundSpr.Add("CUT_2D", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/ChatScript/IntroChatScript/Background/CUT_2D"));
        m_backgroundSpr.Add("CUT_NONE", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/ChatScript/IntroChatScript/Background/CUT_NONE"));
    }

    public override void Enter_Stage()
    {
        base.Enter_Stage();
    }

    protected override void Load_Scene()
    {
        // 변수 할당
        GameObject canvas = GameObject.Find("Canvas");
        m_dialogIntro = canvas.transform.GetChild(0).gameObject.GetComponent<Dialog_IntroWT>();

        // 기본 값 초기화

        // 레벨 초기화
        m_levelController = new LevelController();
        List<Level> levels = new List<Level>
        {
            new Western_IntroLv1(),
            new Western_MainLv1(),
            new Western_PlayLv1(),
            new Western_ClearLv1(),

            new Western_IntroLv2(),
            new Western_PlayLv2(),
        };
        for(int i = 0; i < levels.Count; ++i)
            levels[i].Initialize_Level(m_levelController);
        m_levelController.Initialize_Level(levels);

        // 게임 시작
        Cursor.lockState = CursorLockMode.None;
        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => In_Game());

        GameManager.Ins.Sound.Play_AudioSourceBGM("Western_MainBGM", true, 1f);
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


    public void Over_Game()
    {
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/Panel_Fail", GameObject.Find("Canvas").transform), 0f, true);
    }
}
