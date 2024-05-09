using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Western_IntroLv2 : Level
{
    public Western_IntroLv2(LevelController levelController) : base(levelController)
    {
    }

    public override void Enter_Level()
    {
        // WesternManager.Instance.Dialog.GetComponent<Dialog_WT>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_WT>("Assets/Resources/4. Data/2. Western/Dialog/Intro/Dialog1_Intro1.json"));
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
