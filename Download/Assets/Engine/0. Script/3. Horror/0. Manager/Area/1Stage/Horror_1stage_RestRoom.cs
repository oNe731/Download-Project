using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horror_1stage_RestRoom : Area
{
    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);

        m_levelIndex = (int)Horror_1stage.LEVEL1.LV_RESTROOM;
    }


    public override bool Check_Clear(Interaction_Door interaction_Door, ref string text)
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
