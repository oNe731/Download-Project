using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Western;
public class Western_Round1 : Level
{
    protected Dialog_PlayWT m_dialogPlay;
    protected GameObject    m_MainPanel;
    protected GameObject    m_playButton;
    protected HeartUI       m_heartUI;
    protected StatusBarUI   m_statusBarUI;
    protected Gun           m_gun;
    protected GameObject    m_operation;

    public Dialog_PlayWT DialogPlay => m_dialogPlay;
    public GameObject MainPanel => m_MainPanel;
    public GameObject PlayButton => m_playButton;
    public HeartUI HeartUI => m_heartUI;
    public StatusBarUI StatusBarUI => m_statusBarUI;
    public Gun Gun => m_gun;
    public GameObject Operation => m_operation;

    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);
    }

    public override void Enter_Level()
    {
        WesternManager WM = GameManager.Ins.Western;
        if(WM.Stage == null)
        {
            WM.Stage = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/1Stage/1Stage");
            WM.Stage.name = "1Stage";
        }
        if(WM.Stage.name != "1Stage")
        {
            GameManager.Ins.Resource.Destroy(WM.Stage);
            WM.Stage = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/1Stage/1Stage");
            WM.Stage.name = "1Stage";
        }

        m_dialogPlay  = WM.Stage.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Dialog_PlayWT>();
        m_MainPanel   = WM.Stage.transform.GetChild(0).GetChild(1).gameObject;
        m_playButton  = WM.Stage.transform.GetChild(0).GetChild(1).GetChild(6).gameObject;
        m_heartUI     = WM.Stage.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<HeartUI>();
        m_statusBarUI = WM.Stage.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<StatusBarUI>();
        m_gun         = WM.Stage.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Gun>();
        m_operation   = WM.Stage.transform.GetChild(0).GetChild(0).GetChild(4).gameObject;
    }

    public override void Play_Level()
    {
    }

    public override void Update_Level()
    {
    }

    public override void LateUpdate_Level()
    {
    }

    public override void Exit_Level()
    {
    }
}
