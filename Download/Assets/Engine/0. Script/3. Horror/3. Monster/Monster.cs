using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Character
{
    public enum TYPE { TYPE_STRAITJACKER, TYPE_BUG, TYPE_END };

    protected float m_hp;
    protected float m_attack;
    protected int m_DieStateIndex;

    protected StateMachine<Monster> m_stateMachine;
    protected Spawner m_spawner;

    protected Animator m_animator;

    public float Hp => m_hp;
    public float Attack => m_attack;
    public StateMachine<Monster> StateMachine => m_stateMachine;
    public Spawner Spawner => m_spawner;
    public Animator Animator => m_animator;

    public virtual void Damage_Monster(float damage)
    {
        if (m_stateMachine.CurState == m_DieStateIndex)
            return;

        m_hp -= damage;
        if (m_hp <= 0)
        {
            m_hp = 0;
            m_stateMachine.Change_State(m_DieStateIndex);
        }

        // 피 이펙트 생성
        GameObject gameObject = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Effect/Blood/BloodParticle");
        gameObject.transform.position   = transform.position;
        gameObject.transform.localScale = transform.localScale;

    }

    public void Initialize_Monster(Spawner spawner)
    {
        m_spawner = spawner;
        if (m_spawner == null)
            return;

        transform.position = m_spawner.Get_RandomPosition();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
