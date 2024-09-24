using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Attack : Bug_Base
{
    private float m_time = 0f;

    public Bug_Attack(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0f;

        Look_Player();
        GameManager.Ins.Horror.Player.Damage_Player(m_owner.Attack);
        GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Horror_Bug_Attack", false, 1f);
    }

    public override void Update_State()
    {
        m_time += Time.deltaTime;
        if(m_time > 1f)
            m_stateMachine.Change_State((int)Bug.State.ST_RETREAT); // 후퇴 상태로 전환
    }

    public override void Exit_State()
    {
    }
}
