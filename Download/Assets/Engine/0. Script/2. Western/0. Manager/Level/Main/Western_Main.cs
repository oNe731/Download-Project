using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Western
{
    public abstract class Western_Main : Level
    {
        protected RectTransform m_rectTransform;

        protected bool m_dialogStart = false;
        protected bool m_moveGun = false;
        protected bool m_shootGun = false;

        protected Vector3 m_startPosition;
        protected Vector3 m_targetPosition;
        protected float m_time = 0f;
        protected float m_moveDuration = 1f;
        protected float m_shootDuration = 0.3f;
        protected float m_darkDuration = 1f;
        protected int m_shootCount = 0;

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
            m_rectTransform = GameManager.Ins.Western.PlayButton.GetComponent<RectTransform>();
            m_startPosition = new Vector3(0f, 0f, 0f);
            m_targetPosition = new Vector3(720f, -200f, 0f);
        }

        protected abstract void Start_Dialog();

        public override void Enter_Level()
        {
            WesternManager stage = GameManager.Ins.Western;
            stage.MainPanel.SetActive(true);

            m_rectTransform.anchoredPosition = m_startPosition;
            stage.PlayButton.GetComponent<Button>().interactable = false;

            GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Start_Dialog());
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
            if (m_moveGun)
                Move_Gun();
            else if (m_shootGun)
                Shoot_Gun();
        }

        public override void LateUpdate_Level()
        {
            // 버튼 활성화
            if (m_dialogStart && GameManager.Ins.Western.DialogPlay.Active == false)
            {
                m_dialogStart = false;
                GameManager.Ins.Western.PlayButton.GetComponent<Button>().interactable = true;
            }
        }

        public override void Exit_Level()
        {
            GameManager.Ins.Western.MainPanel.SetActive(false);
        }

        public bool Button_Play()
        {
            if (m_moveGun == false)
            {
                m_moveGun = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Move_Gun()
        {
            // 총 버튼이 대각선으로 화면 밖으로 나감.
            m_time += Time.deltaTime;
            if (m_time >= m_moveDuration)
            {
                m_time = 0f;
                m_rectTransform.anchoredPosition = m_targetPosition;
                m_moveGun = false;
                m_shootGun = true;
                return;
            }

            m_rectTransform.anchoredPosition = Vector3.Lerp(m_startPosition, m_targetPosition, m_time / m_moveDuration);
        }

        private void Shoot_Gun()
        {
            if (GameManager.Ins.IsGame == false)
                return;

            m_time += Time.deltaTime;
            if (m_shootCount < 2)
            {
                if (m_time >= m_shootDuration)
                {
                    m_time = 0f;

                    GameObject bulletMark = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/UI_BulletMark", Vector2.zero, Quaternion.identity, GameManager.Ins.Western.MainPanel.transform);
                    switch (m_shootCount)
                    {
                        case 0: // 총자국 생성
                            bulletMark.GetComponent<RectTransform>().anchoredPosition     = new Vector3(-438f, 125f, 0f);
                            bulletMark.GetComponent<RectTransform>().transform.localScale = new Vector3(1.3f, 1.3f, 1f);
                            bulletMark.GetComponent<Image>().sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/Main/BulletMark/Wanted_BulletMark_1");
                            GameManager.Ins.Sound.Play_ManagerAudioSource("Western_Gun_Shoot", false, 1f);
                            break;

                        case 1: // 총자국 생성
                            bulletMark.GetComponent<RectTransform>().anchoredPosition     = new Vector3(174.9f, -29.4f, 0f);
                            bulletMark.GetComponent<RectTransform>().transform.localScale = new Vector3(1.4f, 1.4f, 1f);
                            bulletMark.GetComponent<Image>().sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/Main/BulletMark/Wanted_BulletMark_2");
                            GameManager.Ins.Sound.Play_ManagerAudioSource("Western_Gun_Shoot", false, 1f);

                            break;
                    }
                    m_shootCount++;
                }
            }
            else if (m_shootCount == 2) // 암전 및 레벨 전환
            {
                if (m_time >= m_darkDuration)
                {
                    GameManager.Ins.UI.Start_FadeWaitAction(1f, Color.black, () => GameManager.Ins.Western.LevelController.Change_NextLevel(), 1f, false);
                    m_shootCount++;
                }
            }
            else
            {
                m_shootGun = false;
            }

        }
    }
}

