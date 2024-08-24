using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Horror_Base : Level
{
    protected LevelController m_levels = null;
    protected GameObject m_stage = null;

    public LevelController Levels => m_levels;
    public GameObject Stage => m_stage;

    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);
    }

    public abstract bool Check_Clear(Interaction_Door interaction_Door, ref string text);

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
