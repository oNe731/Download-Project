using UnityEngine;

public class WindowManager : StageManager
{
    public WindowManager() : base()
    {
        m_stageLevel = STAGE.LEVEL_WINDOW;
        m_sceneName  = "Window";
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
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => In_Game());
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
}
