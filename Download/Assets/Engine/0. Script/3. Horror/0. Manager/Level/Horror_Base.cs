using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Horror_Base : Level
{
    protected LevelController m_levels = null;
    protected GameObject m_stage = null;
    protected GameObject m_light = null;
    protected float m_playerSpeedAdd = 0f;

    public LevelController Levels => m_levels;
    public GameObject Stage => m_stage;
    public GameObject Light => m_light;
    public float PlayerSpeedAdd => m_playerSpeedAdd;

    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);
    }

    public abstract bool Check_Clear(Interaction_Door interaction_Door, ref float[] activeTimes, ref string[] texts);

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
