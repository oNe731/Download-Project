using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Charge : Bug_Base
{
    private Vector3[] m_controlPoints;
    private float m_curveDuration = 1.0f;
    private float m_t = 0f;

    public Bug_Charge(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_t = 0f;
        Generate_RandomCurve();
    }

    public override void Update_State()
    {
        m_t += Time.deltaTime / m_curveDuration;
        if (m_t >= 1f)
        {
            if (Get_PlayerDistance() <= m_attackDist)
                m_stateMachine.Change_State((int)Bug.State.ST_ATTACK); // 공격 상태로 전환
            else
                m_stateMachine.Change_State((int)Bug.State.ST_RETREAT); // 후퇴 상태로 변경
            return;
        }

        Vector3 targetPosition = Calculate_BezierPoint(m_t, m_controlPoints[0], m_controlPoints[1], m_controlPoints[2], m_controlPoints[3]);
        Vector3 direction = (targetPosition - m_owner.transform.position).normalized;
        if (Check_Collider(direction, LayerMask.GetMask("Ground", "Wall", "Ceiling", "Interaction")) == false) // 장애물이 없으면 이동
            m_owner.transform.position = targetPosition;
        else
        {
            if (Get_PlayerDistance() <= m_chaseDist)
                m_stateMachine.Change_State((int)Bug.State.ST_FLY);
            else
                m_stateMachine.Change_State((int)Bug.State.ST_CHASE); // 추격 상태로 변경
        }

        //m_rigidbody.MovePosition(targetPosition);
        //m_rigidbody.velocity = direction * 5f;

        Look_Player();
    }

    public override void Exit_State()
    {
    }

    private void Generate_RandomCurve() // 돌격
    {
        Vector3 playerDirection = HorrorManager.Instance.Player.transform.forward;

        Vector3 startPoint = m_owner.transform.position;
        Vector3 endPoint = HorrorManager.Instance.Player.transform.position + (playerDirection * 0.8f);
        endPoint.y = Camera.main.transform.position.y;

        float distance = Random.Range(1.5f, 3f);
        float height = Random.Range(0f, 1.5f);
        m_controlPoints = new Vector3[4];
        m_controlPoints[0] = startPoint;
        m_controlPoints[1] = startPoint + new Vector3(Random.Range(-distance, distance), Random.Range(0f, height), Random.Range(-distance, distance)); // Random mid-point
        m_controlPoints[2] = endPoint + new Vector3(Random.Range(-distance, distance), Random.Range(0f, height), Random.Range(-distance, distance)); // Another random mid-point
        m_controlPoints[3] = endPoint;
    }
}
