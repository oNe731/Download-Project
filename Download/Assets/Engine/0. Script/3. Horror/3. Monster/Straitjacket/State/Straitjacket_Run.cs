using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Straitjacket_Run : Straitjacket_Base
{
    private NavMeshAgent m_agent;

    public Straitjacket_Run(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_speed = 5f;
        m_agent = m_owner.GetComponent<NavMeshAgent>();
        m_agent.speed = m_speed;
    }

    public override void Enter_State()
    {
        base.Enter_State();
        m_agent.enabled = true;

        m_animator.SetBool("IsRun", true);
    }

    public override void Update_State()
    {
        Play_Sound(1f, 3f);

        if (Change_Attack() == false)
        {
            m_agent.destination = HorrorManager.Instance.Player.transform.position;
        }
    }

    public override void Exit_State()
    {
        base.Exit_State();
        m_agent.enabled = false;

        m_animator.SetBool("IsRun", false);
    }
}
