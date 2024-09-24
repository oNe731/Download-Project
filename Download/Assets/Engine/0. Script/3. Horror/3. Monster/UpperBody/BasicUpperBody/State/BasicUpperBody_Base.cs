using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUpperBody_Base : State<Monster>
{
    protected BasicUpperBody m_owner = null;

    protected Vector3 m_targetPosition;
    protected float m_speed = 5f;

    protected float m_chaseDist = 4f;
    protected float m_attackDist = 4.5f;

    public BasicUpperBody_Base(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_owner = m_stateMachine.Owner.GetComponent<BasicUpperBody>();
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

    public override void OnDrawGizmos()
    {
#if UNITY_EDITOR
        // 추격 범위 표시
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_stateMachine.Owner.transform.position, m_chaseDist);

        // 공격 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_stateMachine.Owner.transform.position, m_attackDist);
#endif
    }

    protected bool Change_Chase()
    {
        // 플레이어가 일정 범위 내로 접근하면 추격 상태 전환
        float distanceToPlayer = Vector3.Distance(m_stateMachine.Owner.transform.position, GameManager.Ins.Horror.Player.transform.position);
        if (distanceToPlayer <= m_chaseDist)
        {
            m_stateMachine.Change_State((int)BasicUpperBody.State.ST_CHASE); // 추격 상태로 전환
            return true;
        }

        return false;
    }

    protected bool Change_Attack()
    {
        // 플레이어가 일정 범위 내로 접근하면 공격 상태 전환
        float distanceToPlayer = Vector3.Distance(m_stateMachine.Owner.transform.position, GameManager.Ins.Horror.Player.transform.position);
        if (distanceToPlayer <= m_attackDist)
        {
            m_stateMachine.Change_State((int)BasicUpperBody.State.ST_ATTACK); // 공격 상태로 전환
            return true;
        }

        return false;
    }

    protected void Set_RandomTargetPosition()
    {
        if (m_owner.Spawner == null)
            return;

        m_targetPosition = m_owner.Spawner.Get_RandomPosition();
    }

    protected Vector3 Get_Direction(Vector3 lookRotation)
    {
        Vector3 direction = lookRotation - m_owner.transform.position;
        direction = direction.normalized;

        return direction;
    }

    protected Quaternion Get_LookRotation(Vector3 lookRotation)
    {
        return Quaternion.LookRotation(Get_Direction(lookRotation), m_owner.Spawner.transform.up);
    }

    protected bool Check_Collider(Vector3 dir, int layerIndex) // ~0
    {
        Vector3 startPosition = m_owner.transform.position + m_owner.transform.up * 0.3f;

        RaycastHit hit = GameManager.Ins.Start_Raycast(startPosition, dir, 1f, layerIndex);
        if (hit.collider != null)
            return true;

        return false;
    }
}
