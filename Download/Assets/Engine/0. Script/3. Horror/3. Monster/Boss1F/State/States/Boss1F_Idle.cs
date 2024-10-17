using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Idle : Boss1F_Base
{
    private float m_time = 0f;

    public Boss1F_Idle(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        Debug.Log("아이들상태");
    }

    public override void Update_State()
    {
        if (Change_Weakness() == false)
        {
            //Look_Player();

            m_time += Time.deltaTime;
            if(m_time >= 1f)
            {
                m_time = 0f;
                Change_Patterns();
            }
        }
    }

    public override void Exit_State()
    {
    }
}
