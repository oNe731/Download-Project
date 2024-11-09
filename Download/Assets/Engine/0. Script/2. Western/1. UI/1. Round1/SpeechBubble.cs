using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Western
{
    public class SpeechBubble : MonoBehaviour
    {
        private Person.PERSONTYPE m_personType = Person.PERSONTYPE.PT_END;

        private float m_time = 0f;
        private float m_deleteTime = 1.5f;

        private RectTransform m_txtTrasform;
        private float m_shakeTime   = 0.3f;
        private float m_shakeAmount = 2.0f; // 세기

        public Person.PERSONTYPE PersonType { set => m_personType = value; }

        private void Start()
        {
            m_txtTrasform = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
            StartCoroutine(Shake_Text(m_txtTrasform.anchoredPosition));
        }

        private void Update()
        {
            m_time += Time.deltaTime;
            if (m_time >= m_deleteTime)
            {
                if (m_personType == Person.PERSONTYPE.PT_CRIMINAL)
                    GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>().LayDown_Group(true);
                else // 시민일 때
                    GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>().Fail_Group();
                Destroy(gameObject);
            }
        }

        private IEnumerator Shake_Text(Vector2 m_startPosition)
        {
            float timer = 0;
            Vector2 startPosition = m_startPosition;

            while (timer < m_shakeTime)
            {
                Vector2 randomPoint = Random.insideUnitCircle * m_shakeAmount;
                m_txtTrasform.anchoredPosition = startPosition + randomPoint;
                timer += Time.deltaTime;
                yield return null;
            }

            m_txtTrasform.anchoredPosition = startPosition;
            yield break;
        }
    }
}

