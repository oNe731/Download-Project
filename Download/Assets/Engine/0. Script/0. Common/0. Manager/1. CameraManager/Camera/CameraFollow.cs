using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : CameraBase
{
    private Transform m_cameraTarget;

    private Vector3 m_offset   = new Vector3(0.0f, 1.3f, 0.0f);
    private float m_mouseSpeed = 250.0f;
    private float m_lerpSpeed  = 100.0f;

    public override void Initialize_Camera()
    {
    }

    public override void Enter_Camera()
    {
        base.Enter_Camera();
        m_cameraTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 고정
    }

    public override void Update_Camera()
    {
        base.Update_Camera();

        // 회전
        Vector3 TargetPos = new Vector3(m_cameraTarget.position.x + m_offset.x, m_cameraTarget.position.y + m_offset.y, m_cameraTarget.position.z);
        float MouseX = Input.GetAxis("Mouse X") * m_mouseSpeed * Time.deltaTime;
        m_mainCamera.RotateAround(TargetPos, Vector3.up, MouseX); // 수평 회전

        // 이동
        Vector3 NewPosition = TargetPos - m_mainCamera.forward * m_offset.z;
        m_mainCamera.position = Vector3.Lerp(m_mainCamera.position, NewPosition, Time.deltaTime * m_lerpSpeed);
    }

    public override void Exit_Camera()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
