using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FaseUpperBody_Chase : FaseUpperBody_Base // 단 한번의 돌격
{
    private float m_attackDist = 1f;
    protected Vector3 m_targetPosition;

    private NavMeshAgent m_agent;

    public FaseUpperBody_Chase(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_agent = m_owner.GetComponent<NavMeshAgent>();
        m_agent.speed = 6f;
    }

    public override void Enter_State()
    {
        m_agent.enabled = true;
        m_agent.speed = 50f;
        m_agent.stoppingDistance = m_attackDist;

        m_targetPosition = HorrorManager.Instance.Player.transform.position;

        // m_animator.SetBool("IsRun", true);
    }

    public override void Update_State()
    {
        if (Change_Attack() == false)
        {
            // 추후 지그재그 움직임 추가
            //

            Vector3 direction = m_targetPosition - m_owner.transform.position;
            direction.y = 0;
            direction = direction.normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                if (Quaternion.Angle(m_owner.transform.rotation, targetRotation) > 10f) // 현재 회전과 목표 회전이 일치하는지 확인/ 회전이 완료되지 않았으면 회전 먼저
                {
                    m_owner.transform.rotation = Quaternion.Slerp(m_owner.transform.rotation, targetRotation, Time.deltaTime * 10f);
                    return;
                }
            }

            m_agent.destination = m_targetPosition;
        }
    }

    public override void Exit_State()
    {
        m_agent.enabled = false;

        // m_animator.SetBool("IsRun", false);
    }

    private bool Change_Attack()
    {
        // 플레이어가 일정 범위 내로 접근하면 공격 상태 전환
        float distanceToPlayer = Vector3.Distance(m_stateMachine.Owner.transform.position, m_targetPosition);
        if (distanceToPlayer <= m_attackDist)
        {
            m_stateMachine.Change_State((int)FaseUpperBody.State.ST_ATTECK); // 공격 상태로 전환
            return true;
        }

        return false;
    }
}
