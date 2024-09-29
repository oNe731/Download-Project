using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private HorrorManager.LEVEL m_changelevel;

    private bool m_active = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (m_active == false || collision.gameObject.CompareTag("Player") == false)// || m_player == null)
            return;

        GameManager.Ins.Set_Pause(true); // 일시정지

        // 페이드 아웃 -> 이동 -> 인
        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Horror.LevelController.Change_Level((int)m_changelevel), 1f, false);
    }
}
