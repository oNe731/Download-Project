using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAMERATYPE { CT_FOLLOW, CT_END };

public class CameraManager : MonoBehaviour
{
    private static CameraManager m_instance = null;
    public static CameraManager Instance
    {
        get //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 호출 가능
        {
            if (null == m_instance)
                return null;
            return m_instance;
        }
    }


    private CAMERATYPE m_currentCameraType = CAMERATYPE.CT_END;
    private CameraBase[] m_cameras;


    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject); //씬 전환이 되더라도 파괴되지 않음

            m_cameras = new CameraBase[CAMERATYPE.GetValues(typeof(CAMERATYPE)).Length]; // 초기화
            m_cameras[(int)CAMERATYPE.CT_FOLLOW] = new CameraFollow();
            m_cameras[(int)CAMERATYPE.CT_FOLLOW].Initialize_Camera();
        }
        else
        {
            Destroy(this.gameObject); //이미 전역변수인 instance에 인스턴스가 존재한다면 자신을 삭제
        }
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
        if(m_currentCameraType != CAMERATYPE.CT_END)
            m_cameras[(int)m_currentCameraType].Exit_Camera();

        // 교체
        m_currentCameraType = type;

        // 진입
        m_cameras[(int)m_currentCameraType].Enter_Camera();
    }
}
