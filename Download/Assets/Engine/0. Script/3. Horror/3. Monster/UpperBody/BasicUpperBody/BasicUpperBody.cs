using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUpperBody : Monster
{
    public enum State { ST_IDLE, ST_WALK, ST_CHASE, ST_ATTACK, ST_DIE, ST_END }

    public override void Damage_Monster(float damage)
    {
        base.Damage_Monster(damage);
    }

    private void Start()
    {
        m_hp = 4f;
        m_attack = 1f;
        m_DieStateIndex = (int)State.ST_DIE;

        m_animator = transform.GetChild(0).GetComponent<Animator>();

        m_stateMachine = new StateMachine<Monster>(gameObject);

        List<State<Monster>> states = new List<State<Monster>>();
        states.Add(new BasicUpperBody_Idle(m_stateMachine));  // 0
        states.Add(new BasicUpperBody_Walk(m_stateMachine));  // 1
        states.Add(new BasicUpperBody_Chase(m_stateMachine)); // 2
        states.Add(new BasicUpperBody_Attack(m_stateMachine));// 3
        states.Add(new BasicUpperBody_Die(m_stateMachine));   // 4

        m_stateMachine.Initialize_State(states, (int)State.ST_WALK);
    }

    private void Update()
    {
        if (m_stateMachine == null)
            return;

        m_stateMachine.Update_State();
    }

    private void OnDrawGizmos()
    {
        if (m_stateMachine == null)
            return;

        m_stateMachine.OnDrawGizmos();
    }
}
