using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Character
{
    public enum TYPE { TYPE_STRAITJACKER, TYPE_BUG, TYPE_END };

    protected float m_hp;
    protected float m_attack;

    protected StateMachine<Monster> m_stateMachine;
    protected Spawner m_spawner;

    protected Animator m_animator;

    public float Hp => m_hp;
    public float Attack => m_attack;
    public StateMachine<Monster> StateMachine => m_stateMachine;
    public Spawner Spawner => m_spawner;
    public Animator Animator => m_animator;

    public abstract void Damage_Monster(float damage);

    public void Initialize_Monster(Spawner spawner)
    {
        m_spawner = spawner;
        if (m_spawner == null)
            return;

        transform.position = m_spawner.Get_RandomPosition();
        m_animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
