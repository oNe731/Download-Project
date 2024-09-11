using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUpperBody_Walk : BasicUpperBody_Base
{
    private int m_targetCount = 0;
    private int m_changeCount = 0;

    public BasicUpperBody_Walk(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_speed = 2f;
    }

    public override void Enter_State()
    {
        m_targetCount = 0;
        m_changeCount = Random.Range(1, 4);
        Set_RandomTargetPosition();

        // m_animator.SetBool("IsWalk", true);
    }

    public override void Update_State()
    {
        if (Change_Attack() == false)
        {
            if (Change_Chase() == false)
            {
                Move_Monster();
            }
        }
    }

    public override void Exit_State()
    {
        // m_animator.SetBool("IsWalk", false);
    }

    private void Move_Monster() // 한면에 붙어있다.(바닥, 벽, 천장) -> 다른 벽으로의 이동은 없다.
    {
        if (m_owner.transform.position == m_targetPosition)
        {
            Count_TargetPosition();
        }
        else
        {
            Vector3 direction = Get_Direction(m_targetPosition);
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, m_owner.Spawner.transform.up);
                if (Quaternion.Angle(m_owner.transform.rotation, targetRotation) > 10f) // 현재 회전과 목표 회전이 일치하는지 확인/ 회전이 완료되지 않았으면 회전 먼저
                {
                    m_owner.transform.rotation = Quaternion.Slerp(m_owner.transform.rotation, targetRotation, Time.deltaTime * 3f);
                    return;
                }
            }

            if (Check_Collider(direction, ~0) == false) // 회전이 완료된 후 이동
                m_owner.transform.position = Vector3.MoveTowards(m_owner.transform.position, m_targetPosition, m_speed * Time.deltaTime);
            else
                Count_TargetPosition();
        }
    }

    private void Count_TargetPosition()
    {
        m_targetCount++;

        if (m_targetCount >= m_changeCount)
            m_owner.StateMachine.Change_State((int)Straitjacket.State.ST_IDLE);
        else
            Set_RandomTargetPosition();
    }
}
