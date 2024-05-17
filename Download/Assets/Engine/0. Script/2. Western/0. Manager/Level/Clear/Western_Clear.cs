using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Western_Clear : Level
    {
        private GameObject m_bloodObj = null;
        private GameObject m_clearObj = null;

        private float m_leveltime = 0f;
        private bool m_fadeOut = false;

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

            UIManager.Instance.Start_FadeIn(1f, Color.black);
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
                if (m_leveltime >= 2f)
                {
                    m_leveltime = 0f;

                    m_clearObj = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/UI_Clear"), Vector2.zero, Quaternion.identity, WesternManager.Instance.MainPanel.transform);
                    m_clearObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
                }
            }
            else
            {
                if (m_fadeOut == false && m_leveltime >= 2f)
                {
                    m_fadeOut = true;
                    m_leveltime = 0f;

                    UIManager.Instance.Start_FadeOut(1f, Color.black, () => Change_Level(), 0f, false);
                }
            }
        }

        public override void LateUpdate_Level()
        {
        }

        public override void Exit_Level()
        {
            WesternManager.Instance.MainPanel.SetActive(false);

            if (m_bloodObj != null)
                Destroy(m_bloodObj);
        }

        private void Change_Level()
        {
            int nextIndex = WesternManager.Instance.LevelController.Curlevel + 1;
            WesternManager.Instance.LevelController.Change_Level(nextIndex);
        }
    }
}

