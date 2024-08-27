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
    protected SkinnedMeshRenderer[] m_skinnedMeshRenderers;

    protected Coroutine m_colorCorutine = null;
    protected Coroutine m_fadeCorutine = null;
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

        if (m_skinnedMeshRenderers == null)
            m_skinnedMeshRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();

        if (m_colorCorutine != null)
            StopCoroutine(m_colorCorutine);
        m_colorCorutine = StartCoroutine(Change_Color(new Color(1f, 1f, 1f, 1f), new Color(1f, 0f, 0f, 1f), 0.2f));
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

    protected IEnumerator Change_Color(Color startColor, Color changeColor, float fadespeed)
    {
        Start_Fade(startColor, changeColor, fadespeed);

        while(true)
        {
            if (m_fadeCorutine == null)
                break;
            yield return null;
        }

        Start_Fade(changeColor, startColor, fadespeed);
        yield break;
    }

    public void Start_Fade(Color startColor, Color changeColor, float duration)
    {
        if (m_fadeCorutine != null)
            StopCoroutine(m_fadeCorutine);
        m_fadeCorutine = StartCoroutine(FadeCoroutine(startColor, changeColor, duration));
    }

    private IEnumerator FadeCoroutine(Color startColor, Color changeColor, float duration)
    {
        float currentTime = 0f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float fadeProgress = currentTime / duration;
            Set_Color(new Color(
                Mathf.Lerp(startColor.r, changeColor.r, fadeProgress), 
                Mathf.Lerp(startColor.g, changeColor.g, fadeProgress), 
                Mathf.Lerp(startColor.b, changeColor.b, fadeProgress), 
                Mathf.Lerp(startColor.a, changeColor.a, fadeProgress)));

            yield return null;
        }

        if (m_fadeCorutine != null)
        {
            StopCoroutine(m_fadeCorutine);
            m_fadeCorutine = null;
        }

        yield break;
    }

    protected void Set_Color(Color color)
    {
        for (int i = 0; i < m_skinnedMeshRenderers.Length; ++i)
        {
            if (m_skinnedMeshRenderers[i].material == null)
                continue;
            m_skinnedMeshRenderers[i].material.color = color;
        }
    }
}
