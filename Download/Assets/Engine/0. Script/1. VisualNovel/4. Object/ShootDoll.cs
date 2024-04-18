using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DOLLTYPE { DT_TRASH, DT_BIRD, BT_SHEEP, BT_CAT, BT_END };

public class ShootDoll : MonoBehaviour
{
    [SerializeField] private GameObject m_Particle;

    [SerializeField] private DOLLTYPE m_dollType = DOLLTYPE.BT_END;
    [SerializeField] private int m_line = 0;
    public int Line
    {
        get { return m_line; }
        set { m_line = value; }
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Ball(Clone)")
        {
            m_hp--;
            if(m_hp <= 0)
            {
                // 게임종료
                
                // 본인 빼고 나머지 인형 다 펑 터지면서 사라짐

                // 몇 초 뒤 페이드 아웃으로 미연시 화면 전환
                //if(!m_Fail)
                //{
                //    // 꽝 이벤트 처리
                //}
                //else
                //{
                //    // 당첨 이벤트 처리
                //}
            }
            else
            {
                StartCoroutine(Blink());
            }

            Destroy(other.gameObject);
            GameObject clone = Instantiate(m_Particle);
            clone.transform.position = transform.position;
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
