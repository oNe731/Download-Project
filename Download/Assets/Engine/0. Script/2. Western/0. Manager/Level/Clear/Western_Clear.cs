using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Western
{
    public class Western_Clear : Level
    {
        private GameObject m_bloodObj = null;
        private GameObject m_clearObj = null;
        private Image m_clearImage = null;

        private float m_leveltime = 0f;
        private bool  m_shake   = false;
        private bool  m_fadeOut = false;

        private Vector3 m_startScale  = new Vector3(30f, 30f, 30f);
        private Vector3 m_targetScale = new Vector3(7f, 7f, 7f);
        private float m_duration = 0.5f;

        private Color m_startColor  = new Color(0.2f, 0.2f, 0.2f, 1f);
        private Color m_targetColor = new Color(1f, 1f, 1f, 1f);

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            WesternManager.Instance.MainPanel.SetActive(true);

            WesternManager.Instance.PlayButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(720f, -200f, 0f);
            m_bloodObj = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/UI_Blood"), Vector2.zero, Quaternion.identity, WesternManager.Instance.MainPanel.transform);
            m_bloodObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);

            GameManager.Instance.UI.Start_FadeIn(1f, Color.black);
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
            // 몇 초 뒤에 클리어 추가
            m_leveltime += Time.deltaTime;
            if (m_clearObj == null)
            {
                if (m_leveltime >= 1f)
                {
                    m_leveltime = 0f;

                    m_clearObj = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/UI_Clear"), Vector2.zero, Quaternion.identity, WesternManager.Instance.MainPanel.transform);
                    m_clearObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
                    m_clearObj.transform.localScale = m_startScale;
                    m_clearImage = m_clearObj.GetComponent<Image>();
                    m_clearImage.color = m_startColor;
                }
            }
            else
            {
                
                if(m_clearObj.transform.localScale != m_targetScale)
                {
                    m_clearObj.transform.localScale = Vector3.Lerp(m_startScale, m_targetScale, m_leveltime / m_duration);
                    m_clearImage.color = Color.Lerp(m_startColor, m_targetColor, m_leveltime / m_duration);

                    if(m_clearObj.transform.localScale.x <= m_targetScale.x)
                        m_leveltime = 0f;
                }
                else
                {
                    if(m_shake == false)
                    {
                        m_shake = true;
                        GameObject.Find("Canvas").transform.GetChild(1).GetComponent<Panel>().Start_Shake(1000f, 0.2f);
                    }
                    else
                    {
                        if (m_fadeOut == false && m_leveltime >= 2f)
                        {
                            m_fadeOut = true;
                            m_leveltime = 0f;

                            //UIManager.Instance.Start_FadeOut(1f, Color.black, () => WesternManager.Instance.LevelController.Change_NextLevel(), 0f, false);
                            GameManager.Instance.UI.Start_FadeOut(1f, Color.black, () => SceneManager.LoadScene("Window"), 0f, false);
                        }
                    }
                }
            }
        }

        public override void LateUpdate_Level()
        {
        }

        public override void Exit_Level()
        {
            if(WesternManager.Instance != null)
                WesternManager.Instance.MainPanel.SetActive(false);

            if (m_bloodObj != null)
                Destroy(m_bloodObj);

            if (m_clearObj != null)
                Destroy(m_clearObj);
        }
    }
}

