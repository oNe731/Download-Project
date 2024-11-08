using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private ParticleSystem m_particleSystem;

    private AudioSource m_audioSource;

    private void Start()
    {
        m_particleSystem = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
        m_audioSource = GetComponent<AudioSource>();
    }

    public void Click_Gun()
    {
        GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Western_Gun_Attacked", false, 1f);
    }

    public void Shoot_Gun()
    {
        GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Western_Gun_Shoot", false, 1f);

        // ¿Ã∆Â∆Æ
        m_particleSystem.gameObject.SetActive(true);
        m_particleSystem.Play();
    }
}
