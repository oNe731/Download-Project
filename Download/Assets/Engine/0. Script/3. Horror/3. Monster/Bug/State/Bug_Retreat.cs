using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Retreat : Bug_Base
{
    private Vector3[] m_controlPoints;
    private float m_curveDuration = 2.0f;
    private float m_t = 0f;

    private Rigidbody m_rigidbody;

    public Bug_Retreat(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_rigidbody = m_owner.GetComponent<Rigidbody>();
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
            m_stateMachine.Change_State((int)Bug.State.ST_FLY); // 대기 나는 상태로 전환
            return;
        }

        m_owner.transform.position = Calculate_BezierPoint(m_t, m_controlPoints[0], m_controlPoints[1], m_controlPoints[2], m_controlPoints[3]);
        //Vector3 targetPosition = Calculate_BezierPoint(m_t, m_controlPoints[0], m_controlPoints[1], m_controlPoints[2], m_controlPoints[3]);
        //m_rigidbody.MovePosition(targetPosition);

        //Vector3 targetPosition = Calculate_BezierPoint(m_t, m_controlPoints[0], m_controlPoints[1], m_controlPoints[2], m_controlPoints[3]);
        //Vector3 direction = (targetPosition - m_owner.transform.position).normalized;
        //m_rigidbody.velocity = direction * 5f;

        m_owner.transform.LookAt(HorrorManager.Instance.Player.transform);
        m_owner.transform.Rotate(-90, 0, 0); // X축으로 -90도 회전 추가
    }

    public override void Exit_State()
    {
    }

    private void Generate_RandomCurve() // 후퇴
    {
        Vector3 startPoint = m_owner.transform.position;
        Vector3 endPoint = m_owner.transform.position + (HorrorManager.Instance.Player.transform.forward * 2f);
        endPoint.y = Camera.main.transform.position.y;
        endPoint.x += Random.Range(-0.5f, 0.5f);
        endPoint.z += Random.Range(-0.5f, 0.5f);

        float distance = Random.Range(1.5f, 3f);
        m_controlPoints = new Vector3[4];
        m_controlPoints[0] = startPoint;
        m_controlPoints[1] = startPoint + new Vector3(Random.Range(-distance, distance), Random.Range(0f, distance), Random.Range(-distance, distance)); // Random mid-point
        m_controlPoints[2] = endPoint + new Vector3(Random.Range(-distance, distance), Random.Range(0f, distance), Random.Range(-distance, distance)); // Another random mid-point
        m_controlPoints[3] = endPoint;
    }
}
