using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Western;
public class Western_Round2 : Level
{
    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);
    }

    public override void Enter_Level()
    {
        WesternManager WM = GameManager.Ins.Western;
        if(WM.Stage == null)
        {
            WM.Stage = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/1Stage/2Stage");
            WM.Stage.name = "2Stage";
        }
        if(WM.Stage.name != "2Stage")
        {
            GameManager.Ins.Resource.Destroy(WM.Stage);
            WM.Stage = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/1Stage/2Stage");
            WM.Stage.name = "2Stage";
        }

        // 변수 할당
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
