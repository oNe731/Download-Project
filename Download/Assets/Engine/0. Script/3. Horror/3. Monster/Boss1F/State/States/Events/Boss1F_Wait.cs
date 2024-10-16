using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Wait : Boss1F_Base
{
    private float m_time = 0f;

    public Boss1F_Wait(StateMachine<Monster> stateMachine) : base(stateMachine)
    {

    }

    public override void Enter_State()
    {
        m_owner.HpPanel.SetActive(true);
    }

    public override void Update_State()
    {
        if(Change_Weakness() == false) // 약점 상태 변경
        {
            Look_Player();

            m_time += Time.deltaTime;
            if(m_time >= 2f)
            {
                m_owner.StateMachine.Change_State((int)Boss1F.State.ST_IDLE);
            }
        }
    }

    public override void Exit_State()
    {

    }
}