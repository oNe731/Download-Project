using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Appear : Boss1F_Base
{
    private bool m_isUsed = false;
    private float m_speed = 1.5f;
    private Vector3 m_targetPosition;

    public Boss1F_Appear(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        if(m_isUsed == false)
        {
            m_isUsed = true;
            m_owner.transform.position = new Vector3(12.18f, 5.173f, 7.75f);
            m_targetPosition = new Vector3(m_owner.transform.position.x, 3.59f, m_owner.transform.position.z);
        }

        Debug.Log("등장상태");
    }

    public override void Update_State()
    {
        m_owner.transform.position = Vector3.Lerp(m_owner.transform.position, m_targetPosition, Time.deltaTime * m_speed);

        if (Vector3.Distance(m_owner.transform.position, m_targetPosition) < 0.01f)
        {
            m_owner.transform.position = m_targetPosition;
            m_owner.StateMachine.Change_State((int)Boss1F.State.ST_WAIT);
        }
    }

    public override void Exit_State()
    {
    }
}
