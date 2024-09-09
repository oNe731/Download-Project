using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bug_Chase : Bug_Base
{
    public float m_stopDistance = 1f;
    private NavMeshAgent m_agent;

    public Bug_Chase(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_agent = m_owner.GetComponent<NavMeshAgent>();
        m_agent.speed = 5f;
    }

    public override void Enter_State()
    {
        //Debug.Log("추격상태로변신");
        m_agent.enabled = true;
        m_agent.stoppingDistance = m_stopDistance;

        m_agent.updatePosition = false;
        m_agent.updateRotation = false;
    }

    public override void Update_State()
    {
        if (Get_PlayerDistance() <= m_chaseDist)
            m_stateMachine.Change_State((int)Bug.State.ST_FLY);
        else
        {
            Vector3 targetPosition = HorrorManager.Instance.Player.transform.position + (HorrorManager.Instance.Player.transform.forward * 0.8f);
            //m_agent.destination = position;
            m_agent.destination = new Vector3(targetPosition.x, m_owner.transform.position.y, targetPosition.z);

            Vector3 nextPosition = new Vector3(m_agent.nextPosition.x, m_owner.transform.position.y, m_agent.nextPosition.z);
            nextPosition.y = Mathf.Lerp(nextPosition.y, 1f, Time.deltaTime * 2f); // 높이 고정
            m_owner.transform.position = nextPosition;

            Look_Player();
        }

    }

    public override void Exit_State()
    {
        m_agent.enabled = false;
    }
}
