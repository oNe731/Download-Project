using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraCutscene : CameraBase
{
    private Transform m_mainCamera;

    public override void Initialize_Camera()
    {
    }

    public override void Enter_Camera()
    {
        m_mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
    }

    public override void Update_Camera()
    {
    }

    public override void Exit_Camera()
    {
        
    }

    public void Change_Position(Vector3 position)
    {
        m_mainCamera.transform.position = position;
    }

    public void Change_Rotation(Vector3 rotation)
    {
        m_mainCamera.transform.rotation = Quaternion.Euler(rotation);
    }
}
