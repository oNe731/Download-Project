using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Base : MonoBehaviour
{
    private ParticleSystem m_system; 

    private void Start()
    {
        m_system = GetComponent<ParticleSystem>();
        if(m_system == null) 
        { 
            m_system = transform.GetChild(0).GetComponent<ParticleSystem>(); 
        }

        if (m_system != null && !m_system.isPlaying) 
        { 
            m_system.Play();
        }
    }

    private void Update()
    {
        if (m_system != null && !m_system.IsAlive()) 
        {
            Destroy(gameObject);
        }
    }
}
