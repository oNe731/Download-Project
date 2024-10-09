using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Western;
public class WesternManager : StageManager
{
    public enum LEVELSTATE { LS_IntroLv1, LS_MainLv1, LS_PlayLv1, LS_ClearLv1, LS_IntroLv2, LS_MainLv2, LS_PlayLv2, LS_ClearLv2, LS_IntroLv3, LS_MainLv3, LS_PlayLv3, LS_ClearLv3, LS_END };


    private bool m_isShoot = false;
    private Dictionary<string, Sprite> m_backgroundSpr = new Dictionary<string, Sprite>();

    private Dialog_IntroWT m_dialogIntro;
    private Dialog_PlayWT  m_dialogPlay;
    private GameObject m_MainPanel;
    private GameObject m_playButton;
    private HeartUI     m_heartUI;
    private StatusBarUI m_statusBarUI;
    private Gun         m_gun;

    public bool IsShoot { get => m_isShoot; set => m_isShoot = value; }
    public Dictionary<string, Sprite> BackgroundSpr { get { return m_backgroundSpr; } }
    public Dialog_IntroWT DialogIntro => m_dialogIntro;
    public Dialog_PlayWT DialogPlay => m_dialogPlay;
    public GameObject MainPanel => m_MainPanel;
    public GameObject PlayButton => m_playButton;
    public HeartUI HeartUI => m_heartUI;
    public StatusBarUI StatusBarUI => m_statusBarUI;
    public Gun Gun => m_gun;


    public WesternManager() : base()
    {
        m_stageLevel = STAGE.LEVEL_WESTERN;
        m_sceneName = "Western";
    }

    protected override void Load_Resource()
    {
        // 배경 이미지 할당
        m_backgroundSpr.Add("Background_01", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/ChatScript/IntroChatScript/Background/Background_01"));
        m_backgroundSpr.Add("Background_02", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/ChatScript/IntroChatScript/Background/Background_02"));
    }

    public override void Enter_Stage()
    {
        base.Enter_Stage();
    }

    protected override void Load_Scene()
    {
        // 변수 할당
        GameObject canvas = GameObject.Find("Canvas");
        m_dialogIntro = canvas.transform.GetChild(3).gameObject.GetComponent<Dialog_IntroWT>();
        m_dialogPlay  = canvas.transform.GetChild(2).gameObject.GetComponent<Dialog_PlayWT>();
        m_MainPanel   = canvas.transform.GetChild(1).gameObject;
        m_playButton  = canvas.transform.GetChild(1).GetChild(6).gameObject;
        m_heartUI     = canvas.transform.GetChild(0).GetChild(1).GetComponent<HeartUI>();
        m_statusBarUI = canvas.transform.GetChild(0).GetChild(0).GetComponent<StatusBarUI>();
        m_gun         = canvas.transform.GetChild(0).GetChild(3).GetComponent<Gun>();

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
            new Western_MainLv2(),
            new Western_PlayLv2(),
            new Western_ClearLv2(),

            new Western_IntroLv3(),
            new Western_MainLv3(),
            new Western_PlayLv3(),
            new Western_ClearLv3(),
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

    protected override void In_Game()
    {
        base.In_Game();
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
        GameManager.Ins.StartCoroutine(Clear_Game());
    }

    private IEnumerator Clear_Game()
    {
        ////* 임시
        GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/Panel_Fail", GameObject.Find("Canvas").transform);
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_END);
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WINDOW), 1f, false);
        yield break;
    }
}
