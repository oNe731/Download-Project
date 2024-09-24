using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Base : State<Monster>
{
    protected Boss1F m_owner = null;

    //protected Animator m_animator = null;
    protected AudioSource m_audioSource = null;

    public Boss1F_Base(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_owner = m_stateMachine.Owner.GetComponent<Boss1F>();

        //m_animator = m_owner.Animator;
        m_audioSource = m_stateMachine.Owner.GetComponent<AudioSource>();
    }

    public override void Enter_State()
    {
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }
}
