using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class StartHeart : MonoBehaviour
    {
        public float moveSpeed = 1.0f; // 이동 속도
        private RectTransform m_rectTransform;

        private void Start()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            Vector2 moveDirection = new Vector2(-1, -1).normalized;
            m_rectTransform.anchoredPosition += moveDirection * moveSpeed * Time.deltaTime;

            if(m_rectTransform.anchoredPosition.x <= -4810f)
                m_rectTransform.anchoredPosition = new Vector2(2880f, m_rectTransform.anchoredPosition.y);

            if (m_rectTransform.anchoredPosition.y <= -3750f)
                m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, 5720f);
        }
    }

}