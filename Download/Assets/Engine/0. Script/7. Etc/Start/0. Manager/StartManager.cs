using UnityEngine;

public class StartManager : MonoBehaviour
{
    public void Button_Start()
    {
        GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WINDOW); // Intro
    }

    public void Button_Exit()
    {
        GameManager.Ins.End_Game();
    }
}