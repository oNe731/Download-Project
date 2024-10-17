using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRouletteCards : MonoBehaviour
{
    private int m_count = 10;
    private List<RectTransform> m_rectTransforms = new List<RectTransform>();
    private float m_moveSpeed = 300f;

    public void Start_GameRouletteCards()
    {
        // 10개의 슬롯 : 게임 3, 열쇠 7개 랜덤 배치
        for (int i = 0; i < transform.childCount; ++i)
            m_rectTransforms.Add(transform.GetChild(i).GetComponent<RectTransform>());
        m_rectTransforms.Shuffle();

        for (int i = 0; i < m_rectTransforms.Count; ++i)
        {
            m_rectTransforms[i].anchoredPosition = new Vector2(809f + (365f * i), m_rectTransforms[i].anchoredPosition.y);
            m_rectTransforms[i].gameObject.SetActive(true);
        }

    }

    private void Update()
    {
        for (int i = 0; i < m_rectTransforms.Count; ++i)
        {
            Vector2 currentPosition = m_rectTransforms[i].anchoredPosition;
            if (currentPosition.x <= -809f) // 2476 + 365f 반복 이동
                currentPosition.x = 2841f;

            currentPosition.x -= m_moveSpeed * Time.deltaTime;
            m_rectTransforms[i].anchoredPosition = currentPosition;
        }
    }
}
