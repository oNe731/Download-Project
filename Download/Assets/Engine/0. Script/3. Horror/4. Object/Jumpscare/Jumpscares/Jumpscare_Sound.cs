using UnityEngine;

public class Jumpscare_Sound : Jumpscare
{
    [SerializeField] private string m_soundName;
    [SerializeField] private bool m_loop;
    [SerializeField] private float m_speed;
    private AudioSource m_audioSource;

    public override void Active_Jumpscare()
    {
        m_isTrigger = true;
        GameManager.Ins.Sound.Play_AudioSource(ref m_audioSource, m_soundName, m_loop, m_speed);
    }

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }
}
