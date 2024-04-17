using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootDoll : MonoBehaviour
{
    private int m_hp = 5;
    public int Hp
    {
        get { return m_hp; }
        set { m_hp = value; }
    }

    private int m_blinkCount = 2;
    private SpriteRenderer m_spriteRenderer;

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // 커튼 속으로 들어가면 다른 라인 시작 위치로 이동해서 해당 라인 이동 방향으로 이동
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Ball(Clone)")
        {
            m_hp--;
            StartCoroutine(Blink());
            Destroy(other.gameObject);
            Debug.Log("충돌");
        }
    }

    private IEnumerator Blink()
    {
        Color startColor = m_spriteRenderer.color;

        int Count = 0;
        while (Count < m_blinkCount)
        {
            float alpha = startColor.a;
            while (alpha > 0f)
            {
                alpha -= 0.1f;
                m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, alpha);
                yield return new WaitForSeconds(0.005f);
            }
            yield return new WaitForSeconds(0.01f);

            while (alpha < 1f)
            {
                alpha += 0.1f;
                m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, alpha);
                yield return new WaitForSeconds(0.005f);
            }
            yield return new WaitForSeconds(0.01f);

            Count++;
        }
        m_spriteRenderer.color = startColor;
    }
}
