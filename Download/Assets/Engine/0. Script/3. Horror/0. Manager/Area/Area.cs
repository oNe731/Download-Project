using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : Horror_Base
{
    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false)
            return;

        m_levelController.Change_Level(m_levelIndex);
    }
}