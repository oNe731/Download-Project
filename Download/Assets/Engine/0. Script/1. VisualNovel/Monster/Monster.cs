using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public enum State { ST_CHASE, ST_ATTCK, ST_END }

    [SerializeField] private State m_state = State.ST_CHASE;
    [SerializeField] private float m_attackDist = 2.0f;
    [SerializeField] private bool m_attacked = false;
    [SerializeField] private bool m_stop = false;

    [SerializeField] private GameObject m_minimapIcon;
    [SerializeField] private GameObject m_stopLight;
    private float m_retryTime = 5f;
    private float m_time;

    private Transform m_playerTr;
    private Transform m_monsterTr;
    private NavMeshAgent m_Agent;

    private void Start()
    {
        m_playerTr  = GameObject.FindWithTag("Player").GetComponent<Transform>();
        m_monsterTr = GetComponent<Transform>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (m_stop)
        {
            m_time += Time.deltaTime;
            if (m_time > m_retryTime)
            {
                m_time = 0.0f;
                m_stop = false;

                m_minimapIcon.SetActive(false);
                m_stopLight.SetActive(false);
            }
        }
        else
        {
            Check_State();
            Update_State();
        }
    }

    private void Check_State()
    {
        if (m_attacked)
            return;

        float distance = Vector3.Distance(m_playerTr.position, m_monsterTr.position);
        if (distance <= m_attackDist)
            m_state = State.ST_ATTCK;
        else
            m_state = State.ST_CHASE;
    }

    private void Update_State()
    {
        switch(m_state)
        {
            case State.ST_CHASE:
                m_Agent.destination = m_playerTr.position;
                break;

            case State.ST_ATTCK:
                m_attacked = true;
                break;
        }
    }

    public void Use_Lever()
    {
        m_stop = true;
        m_Agent.destination = m_Agent.transform.position;
        m_minimapIcon.SetActive(true);
        m_stopLight.SetActive(true);

        // 파티클 생성
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackDist);
    }
}
