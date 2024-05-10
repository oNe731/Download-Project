using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Western_IntroLv1 : Level
{
    public Western_IntroLv1(LevelController levelController) : base(levelController)
    {
    }

    public override void Enter_Level()
    {
        WesternManager.Instance.IntroPanel.SetActive(true);
        WesternManager.Instance.DialogIntro.GetComponent<Dialog_IntroWT>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_IntroWT>("Assets/Resources/4. Data/2. Western/Dialog/Intro/Dialog1_Intro1.json"));
    }

    public override void Play_Level()
    {
    }

    public override void Update_Level()
    {
    }

    public override void Exit_Level()
    {
        WesternManager.Instance.IntroPanel.SetActive(false);
    }
}
