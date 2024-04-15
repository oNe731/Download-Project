using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAMERATYPE { CT_FOLLOW, CT_END };

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CAMERATYPE m_currentCameraType;
    private CameraBase[] m_cameras;

    private void Awake()
    {
        m_cameras = new CameraBase[CAMERATYPE.GetValues(typeof(CAMERATYPE)).Length]; // 초기화
        m_cameras[(int)CAMERATYPE.CT_FOLLOW] = new CameraFollow();
        m_cameras[(int)CAMERATYPE.CT_FOLLOW].Initialize_Camera();
    }

    private void Start()
    {
        // Change_Camera(CAMERATYPE.CT_FOLLOW);
    }

    private void LateUpdate()
    {
        if (m_currentCameraType == CAMERATYPE.CT_END)
            return;

        m_cameras[(int)m_currentCameraType].Update_Camera();
    }

    public void Change_Camera(CAMERATYPE type)
    {
        // 탈출
        m_cameras[(int)m_currentCameraType].Exit_Camera();

        // 교체
        m_currentCameraType = type;

        // 진입
        m_cameras[(int)m_currentCameraType].Enter_Camera();
    }
}
