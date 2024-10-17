using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRouletteText : MonoBehaviour
{
    private enum STATE { ST_SLIDE, ST_BACKGROUND, ST_END }

    [SerializeField] private GameRouletteCards m_gameRouletteCards;
    private STATE m_state = STATE.ST_END;

    private Vector2 m_targetPosition = new Vector2(0f, 3.2f);
    private float m_moveSpeed = 10f;

    private Color m_targetColor;
    private float m_fadeSpeed = 5f;

    private RectTransform m_rectTransform;
    private Image m_blackImage;

    public void Start_Text()
    {
        // 오른쪽에서 왼쪽으로 슬라이드 후 고정
        m_rectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        m_rectTransform.anchoredPosition = new Vector2(1275f, m_targetPosition.y); // 1275 -> 0
        m_state = STATE.ST_SLIDE;

        m_blackImage = GetComponent<Image>();
        m_blackImage.color = new Color(m_blackImage.color.r, m_blackImage.color.g, m_blackImage.color.b, 0f);
        m_targetColor = new Color(m_blackImage.color.r, m_blackImage.color.g, m_blackImage.color.b, 0.5f);
    }

    private void Update()
    {
        if (m_state == STATE.ST_SLIDE)
        {
            m_rectTransform.anchoredPosition = Vector2.Lerp(m_rectTransform.anchoredPosition, m_targetPosition, Time.deltaTime * m_moveSpeed);
            if (Vector2.Distance(m_rectTransform.anchoredPosition, m_targetPosition) < 0.1f)
            {
                m_state = STATE.ST_BACKGROUND;
            }
        }
        else if (m_state == STATE.ST_BACKGROUND)
        {
            // 검은색 50으로 서서히 깔림
            m_blackImage.color = Color.Lerp(m_blackImage.color, m_targetColor, Time.deltaTime * m_fadeSpeed);
            if (Mathf.Approximately(m_blackImage.color.a, 0.5f))
            {
                m_state = STATE.ST_END;
                m_blackImage.color = m_targetColor;
                m_gameRouletteCards.Start_GameRouletteCards();
            }
        }
    }
}
