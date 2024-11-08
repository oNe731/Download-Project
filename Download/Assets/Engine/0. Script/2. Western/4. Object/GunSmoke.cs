using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class GunSmoke : MonoBehaviour
    {
        private Animator m_animator;
        private float m_time = 0f;

        private void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("AM_GunSmoke") == true)
            {
                if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) // 애니메이션 종료일 시
                {
                    m_time += Time.deltaTime;
                    if (m_time > 0.2f) { Destroy(gameObject); }
                }
            }
        }
    }
}

