using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : Monster
{
    public enum State { ST_IDLE, ST_FLY, ST_ATTACK, ST_DIE, ST_END } // 4

    public override void Damage_Monster(float damage)
    {
        if (m_stateMachine.CurState == (int)State.ST_DIE)
            return;

        m_hp -= damage;
        if (m_hp <= 0)
        {
            m_hp = 0;
            m_stateMachine.Change_State((int)State.ST_DIE);
        }
    }

    private void Start()
    {
        m_hp = 4f;

        m_stateMachine = new StateMachine<Monster>(gameObject);

        List<State<Monster>> states = new List<State<Monster>>();
        states.Add(new Bug_Idle(m_stateMachine));   // 0
        states.Add(new Bug_Fly(m_stateMachine));    // 1
        states.Add(new Bug_Attack(m_stateMachine)); // 2
        states.Add(new Bug_Die(m_stateMachine));    // 3

        m_stateMachine.Initialize_State(states, (int)State.ST_IDLE);
    }

    private void Update()
    {
        m_stateMachine.Update_State();
    }
}
