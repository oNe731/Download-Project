using UnityEngine;

public class WindowButton
{
    public WindowButton()
    {
    }

    public void Button_VisualNovel()
    {
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_VISUALNOVEL), 0f, false);
    }

    public void Button_Western()
    {
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WESTERN), 0f, false);
    }

    public void Button_Horror()
    {
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_HORROR), 0f, false);
    }

    public void Button_Exit()
    {
        GameManager.Ins.End_Game();
    }
}
