using UnityEngine;

public class CameraBase
{
    protected Transform m_mainCamera;

    public virtual void Initialize_Camera()
    {
    }

    public virtual void Enter_Camera()
    {
    }

    public virtual void Update_Camera()
    {
    }

    public virtual void Exit_Camera()
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
