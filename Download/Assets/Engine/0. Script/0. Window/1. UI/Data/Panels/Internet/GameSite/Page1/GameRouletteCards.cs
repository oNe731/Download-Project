using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRouletteCards : MonoBehaviour
{
    private List<RectTransform> m_rectTransforms = new List<RectTransform>();
    private float m_moveSpeed = 300f;

    public void Start_GameRouletteCards()
    {
        // 10���� ���� : ���� 3, ���� 7�� ���� ��ġ
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
            if (currentPosition.x <= -809f) // 2476 + 365f �ݺ� �̵�
                currentPosition.x = 2841f;

            currentPosition.x -= m_moveSpeed * Time.deltaTime;
            m_rectTransforms[i].anchoredPosition = currentPosition;
        }
    }
}
