using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRedLine : MonoBehaviour
{
    private List<RectTransform> m_rectTransforms = new List<RectTransform>();
    private float m_moveSpeed = 150f;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
            m_rectTransforms.Add(transform.GetChild(i).GetComponent<RectTransform>());
    }

    private void Update()
    {
        // 왼쪽 -> 오른쪽 반복
        for (int i = 0; i < m_rectTransforms.Count; ++i)
        {
            Vector2 currentPosition = m_rectTransforms[i].anchoredPosition;
            if (currentPosition.x >= 800f) // 반복 이동
                currentPosition.x = -850f;

            currentPosition.x += m_moveSpeed * Time.deltaTime;
            m_rectTransforms[i].anchoredPosition = currentPosition;
        }
    }
}
