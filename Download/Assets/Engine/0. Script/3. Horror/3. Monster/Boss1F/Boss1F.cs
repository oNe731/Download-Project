using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F : Monster
{
    public enum State { ST_APPEAR, ST_IDLE, ST_DIE, ST_END }

    public override void Damage_Monster(float damage)
    {
        base.Damage_Monster(damage);
    }

    private void Start()
    {
        m_hp = 10f;
        m_attack = 1f;
        m_DieStateIndex = (int)State.ST_DIE;
        m_effectOffset = new Vector3(0f, -1f, 0f);

        m_animator = transform.GetChild(0).GetComponent<Animator>();

        m_stateMachine = new StateMachine<Monster>(gameObject);

        List<State<Monster>> states = new List<State<Monster>>();
        states.Add(new Boss1F_Appear(m_stateMachine)); // 0
        states.Add(new Boss1F_Idle(m_stateMachine));   // 1
        states.Add(new Boss1F_Die(m_stateMachine));    // 2

        m_stateMachine.Initialize_State(states, (int)State.ST_APPEAR);
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
