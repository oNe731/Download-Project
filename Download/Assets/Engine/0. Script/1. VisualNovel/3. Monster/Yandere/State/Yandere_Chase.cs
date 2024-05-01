using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Yandere_Chase : State<HallwayYandere>
{
    private float m_attackDist = 2.0f;

    private NavMeshAgent m_agent;
    private Transform m_ownerTr;
    private Transform m_playerTr;

    public Yandere_Chase(StateMachine<HallwayYandere> stateMachine) : base(stateMachine)
    {
        m_agent = m_stateMachine.Owner.GetComponent<NavMeshAgent>();
        m_ownerTr = m_stateMachine.Owner.GetComponent<Transform>();
        m_playerTr = VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().PlayerTr;
    }

    public override void Enter_State()
    {
    }

    public override void Update_State()
    {
        m_agent.destination = m_playerTr.position;

        if (Vector3.Distance(m_playerTr.position, m_ownerTr.position) <= m_attackDist)
            m_stateMachine.Change_State((int)HallwayYandere.YandereState.ST_ATTCK);
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_ownerTr.position, m_attackDist);
    }
}
