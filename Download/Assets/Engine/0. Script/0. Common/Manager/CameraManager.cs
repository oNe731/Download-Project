using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraTypes { Follow, Cutscene };

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform m_mainCamera;
    [SerializeField] private Transform m_cameraTarget;

    [SerializeField] private Vector3 m_offset = new Vector3(0.0f, 1.3f, 0.0f);
    [SerializeField] private float m_mouseSpeed = 200.0f;
    [SerializeField] private float m_lerpSpeed = 100.0f;

    private CameraTypes CameraType;

    private void Start()
    {
        CameraType = CameraTypes.Follow;

        // 마우스 커서 고정
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        switch (CameraType)
        {
            case CameraTypes.Follow:
                Follow_Camera();
                break;

            case CameraTypes.Cutscene:
                break;
        }
    }

    private void Follow_Camera()
    {
        // 회전
        Vector3 TargetPos = new Vector3(m_cameraTarget.position.x + m_offset.x, m_cameraTarget.position.y + m_offset.y, m_cameraTarget.position.z);
        float MouseX = Input.GetAxis("Mouse X") * m_mouseSpeed * Time.deltaTime;
        m_mainCamera.RotateAround(TargetPos, Vector3.up, MouseX); // 수평 회전

        // 이동
        Vector3 NewPosition = TargetPos - m_mainCamera.forward * m_offset.z;
        m_mainCamera.position = Vector3.Lerp(m_mainCamera.position, NewPosition, Time.deltaTime * m_lerpSpeed);
    }

    private void Change_Camera(CameraTypes type)
    {
        // 카메라 타입마다 클래스로 만들고 관리하기
        // 진입/ 진행/ 탈출
        // Cursor.lockState = CursorLockMode.Locked; 진입에 추후 추가하기
    }
}
