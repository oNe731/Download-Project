using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager : StageManager
{
    public LoginManager() : base()
    {
        m_stageLevel = STAGE.LEVEL_LOGIN;
        m_sceneName = "Login";
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

    public override void Set_Pause(bool pause, bool Setcursur)
    {
        base.Set_Pause(pause, Setcursur);
    }
}
