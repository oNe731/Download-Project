using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Horror_Base : Level
{
    private LevelController m_levels = null;
    public LevelController Levels => m_levels;

    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);
    }

    public abstract bool Check_Clear();

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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false)
            return;

        m_levelController.Change_Level(m_levelIndex);
    }

    public override void OnDrawGizmos()
    {
    }
}
