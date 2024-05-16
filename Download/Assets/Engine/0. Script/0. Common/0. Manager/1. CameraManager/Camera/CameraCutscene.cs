using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraCutscene : CameraBase
{
    private bool m_isCutscene = false;

    public Vector3 m_targetPosition = Vector3.zero; // 카메라가 이동할 목표 위치
    public Vector3 m_targetRotation = Vector3.zero; // 카메라가 회전할 목표 각도
    public float m_moveSpeed     = 0f; // 카메라 이동 속도
    public float m_rotationSpeed = 0f; // 카메라 회전 속도

    public override void Initialize_Camera()
    {
    }

    public override void Enter_Camera()
    {
        m_mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
    }

    public override void Update_Camera()
    {
        if (m_isCutscene)
        {
            m_mainCamera.position = Vector3.MoveTowards(m_mainCamera.position, m_targetPosition, m_moveSpeed * Time.deltaTime);
            m_mainCamera.rotation = Quaternion.Slerp(m_mainCamera.rotation, Quaternion.Euler(m_targetRotation), m_rotationSpeed * Time.deltaTime);

            if (m_mainCamera.position == m_targetPosition)
                m_isCutscene = false;
        }
    }

    public override void Exit_Camera()
    {
        // 초기화
        m_mainCamera.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }

    public void Start_Cutscene(Vector3 targetPosition, Vector3 targetRotation, float moveSpeed, float rotationSpeed)
    {
        m_isCutscene = true;
        m_targetPosition = targetPosition;
        m_targetRotation = targetRotation;
        m_moveSpeed      = moveSpeed;
        m_rotationSpeed  = rotationSpeed;
    }
}
