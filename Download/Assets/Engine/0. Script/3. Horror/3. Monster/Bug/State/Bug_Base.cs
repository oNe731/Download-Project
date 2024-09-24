using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Base : State<Monster>
{
    protected Bug m_owner = null;

    protected float m_flyDist = 3f;
    protected float m_attackDist = 2f;
    protected float m_chaseDist = 3f;

    protected Animator m_animator = null;
    protected AudioSource m_audioSource = null;

    public Bug_Base(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_owner    = m_stateMachine.Owner.GetComponent<Bug>();

        m_animator = m_owner.Animator;
        m_audioSource = m_stateMachine.Owner.GetComponent<AudioSource>();
    }

    public override void Enter_State()
    {
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }

    protected float Get_PlayerDistance()
    {
        return Vector3.Distance(m_stateMachine.Owner.transform.position, GameManager.Ins.Horror.Player.transform.position);
    }

    protected bool Change_FLY()
    {
        // 플레이어가 일정 범위 내로 접근하면 비행 상태 전환
        if (Get_PlayerDistance() <= m_flyDist)
        {
            m_stateMachine.Change_State((int)Bug.State.ST_FLY); // 비행 상태로 전환
            return true;
        }

        return false;
    }

    protected void Look_Player()
    {
        m_owner.transform.rotation = Quaternion.LookRotation(GameManager.Ins.Horror.Player.transform.position - m_owner.transform.position);
    }

    protected bool Check_Collider(Vector3 dir, int layerIndex) // ~0
    {
        Vector3 startPosition = m_owner.transform.position;

        RaycastHit hit = GameManager.Ins.Start_Raycast(startPosition, dir, 1f, layerIndex);
        if (hit.collider != null)
            return true;

        return false;
    }

    protected Vector3 Calculate_BezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) // Bezier curve calculation method
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;

        /*Vector3 M0 = Vector3.Lerp(p0, p1, t);
        Vector3 M1 = Vector3.Lerp(p1, p2, t);
        Vector3 M2 = Vector3.Lerp(p2, p3, t);

        Vector3 B0 = Vector3.Lerp(M0, M1, t);
        Vector3 B1 = Vector3.Lerp(M1, M2, t);

        return Vector3.Lerp(B0, B1, t);*/
    }

    public override void OnDrawGizmos()
    {
#if UNITY_EDITOR
        // 변신 범위 표시
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_stateMachine.Owner.transform.position, m_flyDist);

        // 추격 범위 표시
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_stateMachine.Owner.transform.position, m_chaseDist);

        // 공격 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_stateMachine.Owner.transform.position, m_attackDist);
#endif
    }
}
