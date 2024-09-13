using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Idle : Boss1F_Base
{
    public Boss1F_Idle(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
    }

    public override void Update_State()
    {
        Vector3 direction = HorrorManager.Instance.Player.transform.position - m_owner.transform.position;
        direction.y = 0f;
        direction = direction.normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            if (Quaternion.Angle(m_owner.transform.rotation, targetRotation) > 10f)
            {
                m_owner.transform.rotation = Quaternion.Slerp(m_owner.transform.rotation, targetRotation, Time.deltaTime * 5f);
                return;
            }
        }
    }

    public override void Exit_State()
    {
    }
}
