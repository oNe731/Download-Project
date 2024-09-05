using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void Click_Gun()
    {
        GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Western_Gun_Attacked", false, 1f);
    }

    public void Shoot_Gun()
    {
        GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Western_Gun_Shoot", false, 1f);
    }
}
