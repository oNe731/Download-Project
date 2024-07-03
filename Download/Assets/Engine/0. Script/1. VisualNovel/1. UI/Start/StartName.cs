using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class StartName : MonoBehaviour
    {
        private float m_minScale = 0.975f;
        private float m_maxScale = 1.025f;
        private float m_speed = 0.1f;
        private Vector3 m_initialScale;

        void Start()
        {
            m_initialScale = transform.localScale;
        }

        void Update()
        {
            float scale = Mathf.PingPong(Time.time * m_speed, m_maxScale - m_minScale) + m_minScale;
            transform.localScale = m_initialScale * scale;
        }
    }

}