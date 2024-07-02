using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraBasic_2D : CameraBase
{
    public override void Initialize_Camera()
    {
    }

    public override void Enter_Camera()
    {
        base.Enter_Camera();
        Set_Renderer(0); // 2D
    }

    public override void Update_Camera()
    {
        base.Update_Camera();
    }

    public override void Exit_Camera()
    {

    }

    private void Set_Renderer(int rendererindex)
    {
        UniversalAdditionalCameraData CameraData_URP = Camera.main.GetUniversalAdditionalCameraData();
        CameraData_URP.SetRenderer(rendererindex);
    }
}
