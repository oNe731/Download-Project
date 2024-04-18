using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Base : MonoBehaviour
{
    private ParticleSystem m_system; 

    private void Start()
    {
        m_system = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(!m_system.isPlaying)
            Destroy(gameObject);
    }
}
