using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class TimeUI : MonoBehaviour
    {
        private GameObject m_Owner;
        public GameObject Owner { set { m_Owner = value; } }

        private Vector3 m_initialScale;
        private Vector3 m_minScale;

        private void Start()
        {
            m_initialScale = transform.localScale;
            m_minScale = new Vector3(0.55f, 0.55f, 0.55f);
        }

        public void Update_Time(float StartTime,float currentTime)
        {
            if (currentTime > 0f)
                transform.localScale = Vector3.Lerp(m_minScale, m_initialScale, Mathf.Clamp01(currentTime / StartTime));
            else
                transform.localScale = m_minScale;
        }

        private void LateUpdate()
        {
            if (m_Owner == null)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = Camera.main.WorldToScreenPoint(m_Owner.transform.position);
        }
    }
}

