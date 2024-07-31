using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket : Monster
{
    public enum State { ST_IDLE, ST_WALK, ST_RUN, ST_ATTACK, ST_DIE, ST_END } // 4

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
        m_hp = 5f;
        m_attack = 1f;

        m_stateMachine = new StateMachine<Monster>(gameObject);

        List<State<Monster>> states = new List<State<Monster>>();
        states.Add(new Straitjacket_Idle(m_stateMachine));   // 0
        states.Add(new Straitjacket_Walk(m_stateMachine));   // 1
        states.Add(new Straitjacket_Run(m_stateMachine));    // 2
        states.Add(new Straitjacket_Attack(m_stateMachine)); // 3
        states.Add(new Straitjacket_Die(m_stateMachine));    // 4

        m_stateMachine.Initialize_State(states, (int)State.ST_IDLE);
    }

    private void Update()
    {
        m_stateMachine.Update_State();
    }

    private void OnDrawGizmos()
    {
        m_stateMachine.OnDrawGizmos();
    }
}
