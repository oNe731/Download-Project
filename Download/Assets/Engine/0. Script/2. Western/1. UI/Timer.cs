using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Western
{
    public class Timer : MonoBehaviour
    {
        private Image m_currentImage;
        private Image m_nextImage;

        private Sprite[] m_numberSprite;

        private Coroutine m_countingCoroutine;
        private int m_maxCount;
        private int m_currentCount;
        private float m_count = 0f;

        private bool isCount = false;
        public bool IsCount => isCount;

        private void Awake()
        {
            // 1, 2, 3, 4, 5 .. ������ �ε�
            m_numberSprite = GameManager.Ins.Resource.LoadAll<Sprite>("1. Graphic/2D/2. Western/UI/Play/Round1/Timer/");
            m_maxCount = m_numberSprite.Length;
                
            m_currentImage = GetComponent<Image>();
            m_nextImage = transform.GetChild(0).GetComponent<Image>();
        }

        private void Update()
        {
        }

        public void Start_Timer(float timerSpeed)
        {
            m_currentCount = m_maxCount - 1;
            m_currentImage.sprite = m_numberSprite[m_currentCount];
            m_nextImage.sprite = m_numberSprite[m_currentCount - 1];

            if (m_countingCoroutine != null)
                StopCoroutine(m_countingCoroutine);
            m_countingCoroutine = StartCoroutine(Count(timerSpeed));
        }

        private IEnumerator Count(float timerSpeed)
        {
            isCount = true;
            GameManager.Ins.Western.IsShoot = true;

            while (m_currentCount > 0)
            {
                if(GameManager.Ins.IsGame == true)
                {
                    m_count += Time.deltaTime * timerSpeed;
                    m_nextImage.fillAmount = m_count;
                    if (m_count >= 1f)
                    {
                        m_count = 0f;

                        m_currentCount--;
                        m_currentImage.sprite = m_numberSprite[m_currentCount];
                        if (0 <= m_currentCount - 1)
                        {
                            m_nextImage.fillAmount = 0f;
                            m_nextImage.sprite = m_numberSprite[m_currentCount - 1];
                        }
                    }
                }

                yield return null;
            }

            // ����
            isCount = false;
            GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>().Fail_Group();

            Destroy(gameObject);
            yield break;
        }
    }
}

