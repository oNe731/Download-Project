using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraWalk : CameraBase
{
    private bool m_isMove = false;
    private float m_moveSpeed = 2f;
    private float m_heightSpeed = 0.2f;

    private Vector3 m_targetPosition;

    private bool m_isHeightUp = true;
    private float m_height = 0f;
    private float m_heightBase  = 0f;
    private float m_heightRange = 0.03f;


    public bool IsMove => m_isMove;

    public override void Initialize_Camera()
    {
    }

    public override void Enter_Camera()
    {
        base.Enter_Camera();
    }

    public override void Update_Camera()
    {
        base.Update_Camera();

        if (m_isMove)
        {
            if (m_mainCamera.position.x == m_targetPosition.x && m_mainCamera.position.z == m_targetPosition.z)
            {
                if (Mathf.Abs(m_mainCamera.position.y - m_heightBase) < 0.05f)
                {
                    m_isMove = false;
                    return;
                }

                m_mainCamera.position = Vector3.Lerp(m_mainCamera.position, m_targetPosition, Time.deltaTime);
            }
            else
            {
                if (m_isHeightUp)
                {
                    m_height += m_heightSpeed * Time.deltaTime;
                    if (m_height >= m_heightBase + m_heightRange)
                        m_isHeightUp = false;
                }
                else
                {
                    m_height -= m_heightSpeed * Time.deltaTime;
                    if (m_height <= m_heightBase - m_heightRange)
                        m_isHeightUp = true;
                }

                m_mainCamera.position = Vector3.MoveTowards(m_mainCamera.position, m_targetPosition, m_moveSpeed * Time.deltaTime);
                m_mainCamera.position = new Vector3(m_mainCamera.position.x, m_height, m_mainCamera.position.z);
            }
        }
    }

    public override void Exit_Camera()
    {
    }

    public void Set_Height(float height)
    {
        m_heightBase = height;
    }

    public void Start_Move(Vector3 position)
    {
        m_isMove = true;
        m_isHeightUp = true;
        m_height = m_heightBase;
        m_targetPosition = new Vector3(position.x, m_height, position.z);
    }
}
