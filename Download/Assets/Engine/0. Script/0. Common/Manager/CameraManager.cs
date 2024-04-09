using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraTypes { Follow, Cutscene };

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform cameraTr; // 메인 카메라
    [SerializeField] private Transform targetTr; // 카메라 타겟

    [SerializeField] private float positionDistance;
    [SerializeField] private float positionHeight;
    [SerializeField] private float lookAtHeight;

    [SerializeField] private float damping = 10.0f;

    private Vector3 velocity = Vector3.zero;
    private CameraTypes CameraType;

    void Start()
    {
        //CameraType = CameraTypes.Follow;

        // 마우스 커서 고정
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
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
        Vector3 pos = targetTr.position
            + (-targetTr.forward * positionDistance)
            + (Vector3.up * positionHeight);
        cameraTr.position = Vector3.SmoothDamp(cameraTr.position, pos, ref velocity, damping);

        cameraTr.LookAt(targetTr.position + (targetTr.up * lookAtHeight));
    }

    private void Change_Camera(CameraTypes type)
    {
        // 카메라 타입마다 클래스로 만들고 관리하기
        // 진입/ 진행/ 탈출
        // Cursor.lockState = CursorLockMode.Locked; 진입에 추후 추가하기
    }
}
