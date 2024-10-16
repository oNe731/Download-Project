using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseUpperBody : Monster
{
    public enum State { ST_IDLE, ST_CHASE, ST_ATTECK, ST_DIE }

    public override bool Damage_Monster(float damage)
    {
        return base.Damage_Monster(damage);
    }

    private void Start()
    {
        m_hp = 1f; // 어떤 무기로든 한대만 맞아도 죽음
        m_attack = 1f;
        m_DieStateIndex = (int)State.ST_DIE;

        m_animator = transform.GetChild(0).GetComponent<Animator>();

        m_stateMachine = new StateMachine<Monster>(gameObject);

        List<State<Monster>> states = new List<State<Monster>>();
        states.Add(new FaseUpperBody_Idle(m_stateMachine));   // 0
        states.Add(new FaseUpperBody_Chase(m_stateMachine));  // 1
        states.Add(new FaseUpperBody_Attack(m_stateMachine)); // 2
        states.Add(new FaseUpperBody_Die(m_stateMachine));    // 3

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
