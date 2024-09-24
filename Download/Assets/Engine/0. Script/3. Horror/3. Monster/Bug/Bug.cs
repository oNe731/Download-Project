using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : Monster
{
    public enum State { ST_IDLE, ST_FLY, ST_CHARGE, ST_ATTACK, ST_RETREAT, ST_CHASE, ST_DIE, ST_END } // 4

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
        states.Add(new Bug_Idle(m_stateMachine));    // 0
        states.Add(new Bug_Fly(m_stateMachine));     // 1
        states.Add(new Bug_Charge(m_stateMachine));  // 2
        states.Add(new Bug_Attack(m_stateMachine));  // 3
        states.Add(new Bug_Retreat(m_stateMachine)); // 4
        states.Add(new Bug_Chase(m_stateMachine));   // 5
        states.Add(new Bug_Die(m_stateMachine));     // 6

        m_stateMachine.Initialize_State(states, (int)State.ST_IDLE);
    }

    private void Update()
    {
        if (GameManager.Ins.IsGame == false)
            return;

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
