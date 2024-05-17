using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class SpeechBubble : MonoBehaviour
    {
        private float m_time = 0f;
        private float m_delteTime = 1f;

        private void Start()
        {

        }

        private void Update()
        {
            m_time += Time.deltaTime;
            if (m_time >= m_delteTime)
            {
                WesternManager.Instance.LevelController.Get_CurrentLevel<Western_Play>().LayDown_Group(true);
                Destroy(gameObject);
            }
        }
    }
}

