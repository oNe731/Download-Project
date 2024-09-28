using TMPro;
using UnityEngine;

public class LoadingManager : StageManager
{
    public LoadingManager() : base()
    {
        m_stageLevel = STAGE.LEVEL_LOADING;
        m_sceneName  = "Loading";
    }

    protected override void Load_Resource()
    {
    }

    public override void Enter_Stage()
    {
        base.Enter_Stage();
    }

    protected override void Load_Scene()
    {
        Cursor.lockState = CursorLockMode.None;
        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black);
    }

    public override void Update_Stage()
    {
    }

    public override void LateUpdate_Stage()
    {
    }

    public override void Exit_Stage()
    {
        base.Exit_Stage();
    }

    public override void Set_Pause(bool pause, bool Setcursur)
    {
        base.Set_Pause(pause, Setcursur);
    }
}