using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horror_2stage : Horror_Base
{
    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);

        m_levelIndex = (int)HorrorManager.LEVEL.LV_2STAGE;
    }

    public override bool Check_Clear(ref string text)
    {
        return true;
    }

    public override void Enter_Level()
    {
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

    public override void OnDrawGizmos()
    {
    }
}