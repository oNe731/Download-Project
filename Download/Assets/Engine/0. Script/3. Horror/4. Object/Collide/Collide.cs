using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collide : MonoBehaviour
{
    [SerializeField] private bool m_loop;
    private bool m_isTrigger = false;

    public bool IsTrigger => m_isTrigger;

    public virtual void Trigger_Event()
    {
    }

    public virtual void Collision_Event()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_isTrigger == true || other.gameObject.CompareTag("Player") == false)
            return;

        m_isTrigger = !m_loop;
        Trigger_Event();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_isTrigger == true || collision.gameObject.CompareTag("Player") == false)
            return;

        m_isTrigger = !m_loop;
        Collision_Event();
    }
}
