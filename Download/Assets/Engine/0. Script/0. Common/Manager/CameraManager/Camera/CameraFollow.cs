using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : CameraBase
{
    private Transform m_cameraPositionTarget;
    private Transform m_cameraRotationTarget;
    private bool m_isPosition = false;
    private bool m_isRotation = false;

    private Vector3 m_offset   = new Vector3(0.0f, 1.3f, 0.0f);
    private float m_mouseSpeed = 250.0f;
    private float m_lerpSpeed  = 100.0f;

    private Vector2 m_rotationLimit = new Vector2(-45f, 45f);
    private bool m_isXRotate = false;
    private bool m_isYRotate = false;

    private float xRotate = 0.0f;

    private bool isRock = false;
    public bool IsRock { get => isRock; set => isRock = value; }

    public override void Initialize_Camera()
    {
    }

    public override void Enter_Camera()
    {
        base.Enter_Camera();
        GameManager.Ins.Camera.Set_CursorLock(true);
    }

    public override void Update_Camera()
    {
        if (m_mainCamera == null || m_cameraPositionTarget == null || isRock == true)
            return;

        // 이동
        if(m_isPosition == false)
        {
            m_mainCamera.transform.position = m_cameraPositionTarget.position;
        }
        else
        {
            Vector3 TargetPos = new Vector3(m_cameraPositionTarget.position.x + m_offset.x, m_cameraPositionTarget.position.y + m_offset.y, m_cameraPositionTarget.position.z);
            Vector3 NewPosition = TargetPos - m_mainCamera.forward * m_offset.z;
            m_mainCamera.position = Vector3.Lerp(m_mainCamera.position, NewPosition, Time.deltaTime * m_lerpSpeed);
        }

        // 회전
        if (m_isPosition == false)
        {
            //m_mainCamera.transform.rotation = m_cameraRotationTarget.rotation;
            m_mainCamera.transform.rotation = Quaternion.Lerp(m_mainCamera.transform.rotation, m_cameraRotationTarget.rotation, Time.deltaTime * m_lerpSpeed);
        }
        else
        {
            float xRotateSize = 0f;
            float yRotateSize = 0f;
            if (m_isXRotate == true)
                xRotateSize = -Input.GetAxis("Mouse Y") * m_mouseSpeed * Time.deltaTime;
            if (m_isYRotate == true)
                yRotateSize = Input.GetAxis("Mouse X") * m_mouseSpeed * Time.deltaTime;

            float yRotate = m_mainCamera.transform.eulerAngles.y + yRotateSize;
            xRotate = Mathf.Clamp(xRotate + xRotateSize, m_rotationLimit.x, m_rotationLimit.y); // 각도 제한

            m_mainCamera.transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
        }

        base.Update_Camera();
    }

    public override void Exit_Camera()
    {
        GameManager.Ins.Camera.Set_CursorLock(false);
    }

    public void Set_FollowInfo(Transform positiontarget, Transform rotationtarget, bool isPosition, bool isRotation, Vector3 offset, float moveSpeed, float lerpSpeed, Vector2 rotationLimit, bool isXRotate, bool isYRotate, bool update = false)
    {
        m_cameraPositionTarget = positiontarget;
        m_cameraRotationTarget = rotationtarget;
        m_isPosition = isPosition;
        m_isRotation = isRotation;

        m_offset     = offset;
        m_mouseSpeed = moveSpeed;
        m_lerpSpeed  = lerpSpeed;

        m_rotationLimit = rotationLimit;
        m_isXRotate = isXRotate;
        m_isYRotate = isYRotate;

        if (update == true)
            Update_Camera();
    }
}
