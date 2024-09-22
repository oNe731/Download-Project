using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket : Monster
{
    public enum State { ST_IDLE, ST_WALK, ST_RUN, ST_RUNWAIT, ST_ATTACK, ST_ATTACKWAIT, ST_DIE, ST_END } // 4

    public override void Damage_Monster(float damage)
    {
        base.Damage_Monster(damage);
    }

    private void Start()
    {
        m_hp = 5f;
        m_attack = 1f;
        m_DieStateIndex = (int)State.ST_DIE;

        m_animator = transform.GetChild(0).GetComponent<Animator>();

        m_stateMachine = new StateMachine<Monster>(gameObject);

        List<State<Monster>> states = new List<State<Monster>>();
        states.Add(new Straitjacket_Idle(m_stateMachine));        // 0
        states.Add(new Straitjacket_Walk(m_stateMachine));        // 1
        states.Add(new Straitjacket_Run(m_stateMachine));         // 2
        states.Add(new Straitjacket_RunWait(m_stateMachine));     // 3
        states.Add(new Straitjacket_Attack(m_stateMachine));      // 4
        states.Add(new Straitjacket_AttackWait(m_stateMachine));  // 5
        states.Add(new Straitjacket_Die(m_stateMachine));         // 6

        m_stateMachine.Initialize_State(states, (int)State.ST_IDLE);
    }

    private void Update()
    {
        if (HorrorManager.Instance.IsGame == false)
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

    private void OnDestroy()
    {
        if (m_spawner != null)
        {
            if(m_spawner.transform.childCount <= 1)
                Destroy(m_spawner.gameObject);
        }
    }
}
