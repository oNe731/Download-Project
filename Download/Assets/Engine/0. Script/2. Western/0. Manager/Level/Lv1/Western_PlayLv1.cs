using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Western_PlayLv1 : Level
{
    public Western_PlayLv1(LevelController levelController) : base(levelController)
    {
    }

    public override void Enter_Level()
    {
        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_WALK);
    }

    public override void Play_Level()
    {
    }

    public override void Update_Level()
    {
    }

    public override void Exit_Level()
    {
    }
}
