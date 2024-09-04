using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // [SerializeField] private Vector3 m_targetPosition;
    [SerializeField] private HorrorManager.LEVEL m_changelevel;

    private bool m_active = true;

    // private GameObject m_player;

    private void Start()
    {
        //m_player = GameObject.FindWithTag("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_active == false || collision.gameObject.CompareTag("Player") == false)// || m_player == null)
            return;

        HorrorManager.Instance.Set_Pause(true); // 일시정지

        // 페이드 아웃 -> 이동 -> 인
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Teleport_Player(), 1f, false);
    }

    private void Teleport_Player()
    {
        HorrorManager.Instance.LevelController.Change_Level((int)m_changelevel);

        //m_player.transform.position = m_targetPosition;

        HorrorManager.Instance.Set_Pause(false); // 일시정지 해제

        GameManager.Ins.UI.Start_FadeIn(1f, Color.black);
    }
}
