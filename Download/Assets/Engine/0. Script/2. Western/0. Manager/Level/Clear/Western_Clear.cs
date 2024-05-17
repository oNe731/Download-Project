using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Western_Clear : Level
{
    public Western_Clear(LevelController levelController) : base(levelController)
    {
    }

    public override void Enter_Level()
    {
        WesternManager.Instance.MainPanel.SetActive(true);
        UIManager.Instance.Start_FadeIn(1f, Color.black);
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
        WesternManager.Instance.MainPanel.SetActive(false);
    }
}
