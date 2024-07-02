using UnityEngine;

public class CameraBase
{
    protected Transform m_mainCamera;

    private bool m_shake = false;
    private float m_time;
    private float m_shakeTime;
    private float m_shakeAmount;
    private Vector3 m_startPosition;

    public virtual void Initialize_Camera()
    {
    }

    public virtual void Enter_Camera()
    {
        m_mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
    }

    public virtual void Update_Camera()
    {
        if(m_shake)
        {
            m_time += Time.deltaTime;
            Vector3 randomPoint = m_startPosition + Random.insideUnitSphere * m_shakeAmount;
            m_mainCamera.transform.localPosition = Vector3.Lerp(m_startPosition, randomPoint, Time.deltaTime);

            if (m_time >= m_shakeTime)
            {
                m_shake = false;
                m_mainCamera.transform.localPosition = m_startPosition;
            }
        }
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

    public void Start_Shake(float ShakeAmount, float ShakeTime)
    {
        m_shake = true;
        m_time = 0f;
        m_shakeTime = ShakeTime;
        m_shakeAmount = ShakeAmount;
        m_startPosition = m_mainCamera.transform.localPosition;
    }
}
