using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public enum STATE { ST_UP, ST_IDLE, ST_DOWN, ST_END };

    private float m_damage = 2f;

    private STATE m_state = STATE.ST_END;
    private float m_time = 0f;
    private bool m_attack = false;
    private float m_idleTime = 0;

    private Animator m_animator;
    private BoxCollider m_collider;

    public void Start_Tentacle(float symptomTime, float idleTime)
    {
        m_animator = transform.GetChild(0).GetComponent<Animator>();
        m_collider = GetComponent<BoxCollider>();
        m_idleTime = idleTime;

        // 전조 증상 이펙트 생성 : 촉수가 나올 위치의 바닥에, 바닥이 갈라지는 듯한 이펙트
        GameObject gameObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/Effect/Monster/Tentacle_Symptom");
        if(gameObject != null)
        {
            gameObject.transform.position = transform.position;

            Tentacle_Symptom tentacle_Symptom = gameObject.GetComponent<Tentacle_Symptom>();
            if(tentacle_Symptom != null)
                tentacle_Symptom.Start_Symptom(this, symptomTime);
        }

        transform.gameObject.SetActive(false);
    }

    public void Up_Tentacle()
    {
        transform.gameObject.SetActive(true);
        m_animator.SetBool("IsUp", true);
        m_state = STATE.ST_UP;
    }

    private void Update()
    {
        switch(m_state)
        {
            case STATE.ST_UP:
                if (m_animator.IsInTransition(0) == true)
                    return;
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("IsUp") == true)
                {
                    m_animator.SetBool("IsUp", false);

                    float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    if (animTime >= 1f)
                    {
                        m_collider.enabled = true;
                        m_state = STATE.ST_IDLE;
                    }
                }
                break;

            case STATE.ST_IDLE:
                m_time += Time.deltaTime;
                if(m_time >= m_idleTime)
                {
                    Down_Tentacle();
                    m_collider.enabled = false;
                    m_state = STATE.ST_DOWN;
                }
                break;

            case STATE.ST_DOWN:
                if (m_animator.IsInTransition(0) == true)
                    return;
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("IsDown") == true)
                {
                    m_animator.SetBool("IsDown", false);

                    float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    if (animTime >= 1f)
                    {
                        Destroy(gameObject);
                    }
                }
                break;
        }
    }

    public void Down_Tentacle()
    {
        m_animator.SetBool("IsDown", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_attack == true)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            m_attack = true;
            GameManager.Ins.Horror.Player.Damage_Player(m_damage);
        }
    }
}
