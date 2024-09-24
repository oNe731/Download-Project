using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicUpperBody_Chase : BasicUpperBody_Base
{
    public float stopDistance = 1f;
    private NavMeshAgent m_agent;

    public BasicUpperBody_Chase(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_speed = 5f;
        m_agent = m_owner.GetComponent<NavMeshAgent>();
        m_agent.speed = m_speed;
    }

    public override void Enter_State()
    {
        m_agent.enabled = true;
        m_agent.stoppingDistance = stopDistance;

        // m_animator.SetBool("IsRun", true);
    }

    public override void Update_State()
    {
        // 주인공 쪽으로 돌아본다.

        if (Change_Attack() == false)
        {
            m_targetPosition = GameManager.Ins.Horror.Player.transform.position;

            Vector3 direction = m_targetPosition - m_owner.transform.position;
            direction.y = 0;
            direction = direction.normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                if (Quaternion.Angle(m_owner.transform.rotation, targetRotation) > 10f) // 현재 회전과 목표 회전이 일치하는지 확인/ 회전이 완료되지 않았으면 회전 먼저
                {
                    m_owner.transform.rotation = Quaternion.Slerp(m_owner.transform.rotation, targetRotation, Time.deltaTime * 5f);
                    return;
                }
            }

            if (Check_Collider(direction, LayerMask.GetMask("Monster")) == false) // 회전이 완료된 후 이동
                m_agent.destination = m_targetPosition;
            //else
            //    m_stateMachine.Change_State((int)BasicUpperBody.State.ST_WAIT);
        }
    }

    public override void Exit_State()
    {
        m_agent.enabled = false;

        // m_animator.SetBool("IsRun", false);
    }
}
