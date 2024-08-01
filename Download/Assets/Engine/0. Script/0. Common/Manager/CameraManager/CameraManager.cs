using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAMERATYPE { CT_BASIC_2D, CT_BASIC_3D, CT_CUTSCENE, CT_FOLLOW, CT_WALK, CT_END };

public class CameraManager : MonoBehaviour
{
    private CAMERATYPE m_currentCameraType = CAMERATYPE.CT_END;
    private CameraBase[] m_cameras;

    private void Start()
    {
        m_cameras = new CameraBase[CAMERATYPE.GetValues(typeof(CAMERATYPE)).Length];
        m_cameras[(int)CAMERATYPE.CT_BASIC_2D] = new CameraBasic_2D();
        m_cameras[(int)CAMERATYPE.CT_BASIC_3D] = new CameraBasic_3D();
        m_cameras[(int)CAMERATYPE.CT_CUTSCENE] = new CameraCutscene();
        m_cameras[(int)CAMERATYPE.CT_FOLLOW] = new CameraFollow();
        m_cameras[(int)CAMERATYPE.CT_WALK] = new CameraWalk();

        for (int i = 0; i < m_cameras.Length - 1; ++i)
            m_cameras[i].Initialize_Camera();
    }

    private void LateUpdate()
    {
        if (m_currentCameraType == CAMERATYPE.CT_END)
            return;

        m_cameras[(int)m_currentCameraType].Update_Camera();
    }


    public void Change_Camera(CAMERATYPE type)
    {
        if (type == m_currentCameraType)
            return;

        // 탈출
        if(m_currentCameraType != CAMERATYPE.CT_END)
            m_cameras[(int)m_currentCameraType].Exit_Camera();

        // 교체
        m_currentCameraType = type;

        // 진입
        if (m_currentCameraType != CAMERATYPE.CT_END)
            m_cameras[(int)m_currentCameraType].Enter_Camera();
    }

    public CameraBase Get_CurCamera()
    {
        return m_cameras[(int)m_currentCameraType];
    }

    public void Set_CursorLock(bool CursorLock)
    {
        if (CursorLock == true)
            Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 고정
        else
            Cursor.lockState = CursorLockMode.None;
    }
}
