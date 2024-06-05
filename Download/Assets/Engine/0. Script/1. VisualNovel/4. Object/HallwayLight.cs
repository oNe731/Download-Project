using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class HallwayLight : MonoBehaviour
    {
        private Light m_light;
        private float m_changeTime;
        private float m_time;

        [SerializeField] private bool m_blink = false;
        public bool Blink
        {
            get { return m_blink; }
            set { m_blink = value; }
        }

        private void Start()
        {
            m_light = GetComponent<Light>();
            m_changeTime = Random.Range(1.0f, 3.0f);

            VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().Light.Add(this);
        }

        private void Update()
        {
            if (false == m_blink)
                return;

            m_time += Time.deltaTime;
            if (m_time > m_changeTime)
            {
                m_time = 0.0f;
                m_changeTime = Random.Range(0.5f, 2.0f);
                m_light.enabled = !m_light.enabled;
            }
        }
    }
}

