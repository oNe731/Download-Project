using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Fly : Bug_Base
{
    float m_time = 0f;
    float m_ataackTime = 0f;

    public Bug_Fly(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0;
        m_ataackTime = Random.Range(0.3f, 0.6f);

        m_animator.SetBool("IsFly", true);
        if(m_audioSource.loop == false)
            GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Horror_Bug_Fly", true, 1f);
    }

    public override void Update_State()
    {
        m_time += Time.deltaTime;
        if(m_time >= m_ataackTime)
            m_stateMachine.Change_State((int)Bug.State.ST_CHARGE); // 공격 상태로 전환
        else
            Look_Player();
    }

    public override void Exit_State()
    {
    }
}
