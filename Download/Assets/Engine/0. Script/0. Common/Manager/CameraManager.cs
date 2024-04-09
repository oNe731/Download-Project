using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraTypes { Follow, Cutscene };

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject MainCamera;   // 메인 카메라
    [SerializeField] private GameObject CameraTarget; // 카메라 타겟

    [SerializeField] private Vector3 Offset = new Vector3(0.0f, 1.5f, 3.0f);
    [SerializeField] private float MouseSpeed = 100.0f;
    [SerializeField] private float LerpSpeed = 5.0f;

    private CameraTypes CameraType;

    void Start()
    {
        CameraType = CameraTypes.Follow;

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
        float MouseX = Input.GetAxis("Mouse X") * MouseSpeed * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * MouseSpeed * Time.deltaTime;

        Vector3 TargetPos = CameraTarget.transform.position;
        TargetPos.x += Offset.x;
        TargetPos.y += Offset.y;

        // 카메라 회전
        MainCamera.transform.RotateAround(TargetPos, Vector3.up, MouseX);                  // 수평 회전
        //MainCamera.transform.RotateAround(TargetPos, MainCamera.transform.right, -MouseY); // 수직 회전

        // 타겟 따라가기
        Vector3 Position = TargetPos - MainCamera.transform.forward * Offset.z; // 타겟 주위로 카메라 이동
        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, Position, Time.deltaTime * LerpSpeed); // 이동 보간
    }

    private void Change_Camera(CameraTypes type)
    {
        // 카메라 타입마다 클래스로 만들고 관리하기
        // 진입/ 진행/ 탈출
        // Cursor.lockState = CursorLockMode.Locked; 진입에 추후 추가하기
    }
}
