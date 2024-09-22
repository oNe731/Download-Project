using UnityEngine;

public class Collide_Sound : Collide
{
    [SerializeField] private string m_soundName;
    [SerializeField] private bool   m_soundLoop;
    [SerializeField] private float  m_soundSpeed;

    private AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public override void Trigger_Event()
    {
        GameManager.Ins.Sound.Play_AudioSource(m_audioSource, m_soundName, m_soundLoop, m_soundSpeed);
    }
}
