using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket_Run : Straitjacket_Base
{
    public Straitjacket_Run(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_speed = 5f;
    }

    public override void Enter_State()
    {
        base.Enter_State();

        Debug.Log("플레이어 추격");
    }

    public override void Update_State()
    {
        if (Change_Attack() == false)
        {
            // 플레이어 공격
            Vector3 direction = (HorrorManager.Instance.Player.transform.position - m_owner.transform.position).normalized;
            Vector3 newPos = m_owner.transform.position + direction * m_speed * Time.deltaTime;

            m_owner.transform.position = newPos;
            m_owner.transform.LookAt(HorrorManager.Instance.Player.transform);

            //// 이동할 수 있는 위치인지 확인
            //if (m_owner.Spawner.Check_Position(newPos))
            //{
            //    m_owner.transform.position = newPos;
            //}
            //else
            //{
            //    // 이동할 수 없는 경우에는 방향을 전환할 수 있는 로직을 추가할 수 있습니다.
            //    Debug.Log("이동할 수 없는 위치");
            //}
        }
    }

    public override void Exit_State()
    {
        base.Exit_State();
    }
}
