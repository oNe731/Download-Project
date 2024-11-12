using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Western;

public class ObjPc : MonoBehaviour
{
    public enum State { ST_STOP, ST_WAIT, ST_CLEAR, ST_END }

    private float m_dist = 2f;

    private State m_state = State.ST_STOP;
    private float m_time = 0f;

    private Western_PlayLv2 m_level;
    private WalkPlayer m_player;
    private CameraCutscene m_camera;

    private void Start()
    {
        m_level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv2>();
        if (m_level != null)
            m_player = m_level.Player;
    }

    private void Update()
    {
        switch(m_state)
        {
            case State.ST_STOP:
                float distanceToPlayer = Vector3.Distance(transform.position, m_player.transform.position);
                if (distanceToPlayer <= m_dist)
                {
                    m_player.Set_Lock(true);

                    // UI 비활성화
                    GameManager.Ins.Western.Stage.transform.GetChild(0).gameObject.SetActive(false);

                    GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_CUTSCENE);
                    m_camera = (CameraCutscene)GameManager.Ins.Camera.Get_CurCamera();
                    m_camera.Start_Position(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.3f), 0.5f);
                    m_camera.Start_FOV(30f, 10f);

                    m_state = State.ST_WAIT;
                }
                break;

            case State.ST_WAIT:
                if(m_camera.IsPosition == false)
                {
                    m_time += Time.deltaTime;
                    if (m_time > 1f)
                    {
                        m_time = 0f;

                        // 화면 메테리얼 주인공 얼굴로 교체
                        transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = GameManager.Ins.Resource.Load<Material>("1. Graphic/3D/2. Western/Map/Stage2/Meterial/3D/2_Computer2");

                        // 라이트 삭제
                        GameManager.Ins.Resource.Destroy(transform.GetChild(2).gameObject);

                        // 수배지 조건 마주하기 완료
                        m_level.PlayerMemo.Check_List();

                        m_state = State.ST_CLEAR;
                    }
                }
                break;

            case State.ST_CLEAR:
                m_time += Time.deltaTime;
                if(m_time > 3.5f)
                {
                    // 서부 게임 클리어
                    GameManager.Ins.Western.IsClear = true;

                    // 페이드 아웃/ 윈도우로 씬 이동
                    GameManager.Ins.UI.Start_FadeOut(0f, Color.black, () => GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WINDOW), 1f, false);

                    m_state = State.ST_END;
                }
                break;
        }
    }

    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_dist);
#endif
    }
}
