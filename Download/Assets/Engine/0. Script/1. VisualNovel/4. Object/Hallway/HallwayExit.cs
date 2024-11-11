using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualNovel;

public class HallwayExit : MonoBehaviour
{
    private bool m_exit = false;
    private HallwayPlayer m_player;

    private Vector3 m_targetPosition;

    public void Set_HallwayExit(HallwayPlayer player)
    {
        m_player = player;

        // 목표 위치 계산
        m_targetPosition = transform.position + new Vector3(0f, 1.30005f, -0.5f);
    }

    private void Update()
    {
        if (m_exit == false)
        {
            // 플레이어가 일정 거리 이내로 접근했을 시
            if (Vector3.Distance(transform.position, m_player.gameObject.transform.position) < 6f)
            {
                True_Exit();
            }
        }
        else
        {
            // 비상구로 이동

            // 카메라 위치 이동
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, m_targetPosition, 3f * Time.deltaTime);

            // 목표 방향 계산 (카메라가 목표 위치를 바라보도록 함)
            Vector3 direction = (m_targetPosition - Camera.main.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    private void True_Exit()
    {
        m_exit = true;

        GameManager.Ins.Novel.IsClear = true;

        // 플레이어 조작 비활성화
        GameManager.Ins.Novel.Set_Pause(true, false);

        // 페이드 아웃(하얀색) 재생
        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeOut(3f, Color.white, () => GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WINDOW), 0.5f, false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 6f);
    }
}
