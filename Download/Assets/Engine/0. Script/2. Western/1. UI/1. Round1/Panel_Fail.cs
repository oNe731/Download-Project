using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Western
{
    public class Panel_Fail : MonoBehaviour
    {
        [SerializeField] private RectTransform m_rt;
        [SerializeField] private Animator m_animator;
        [SerializeField] private TMP_Text m_dialogText;

        private float m_typingSpeed = 0.05f;
        private Vector2 m_centerPosition = new Vector2(0, 0);
        private Vector2 m_exitPosition = new Vector2(-1120, 0);

        private string[] m_texts;
        private int m_currentTextIndex;

        private void Start()
        {
            // 대사 초기화
            m_texts = new string[7];
            m_texts[0] = "어이, 너. 지금 뭐하는거야.";
            m_texts[1] = "내가 누구냐고?";
            m_texts[2] = "니가 움직이던 서부 최고의 총잡이, 그게 나다.";
            m_texts[3] = "아, 이런 말이나 하러 온건 아니고.";
            m_texts[4] = "너 우릴 다 죽일 셈이냐?";
            m_texts[5] = "영웅이 지는 책임의 무게는 가볍지 않다고.";
            m_texts[6] = "정신 똑바로 차려.";

            m_currentTextIndex = 0;
            m_dialogText.text = "";

            Western_PlayLv1 level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>();
            if (level != null)
                level.Destroy_Element();

            GameManager.Ins.UI.EventUpdate = true;
            GameManager.Ins.UI.Start_FadeIn(0f, Color.black);

            GameManager.Ins.Sound.Stop_AudioSourceBGM();
            StartCoroutine(Start_Sequence());
        }

        private IEnumerator Start_Sequence()
        {
            // 가운데로 이동
            yield return new WaitForSeconds(1f);
            yield return Move_Position(m_centerPosition, 1.5f);

            // 도착 후 아이들 애니메이션 재생
            m_animator.SetTrigger("IsIdle");
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(Display_Texts());

            // 왼쪽으로 이동
            m_dialogText.text = "";
            m_animator.SetTrigger("IsWalk");
            yield return new WaitForSeconds(0.5f);
            yield return Move_Position(m_exitPosition, 1.5f);

            // 게임 재시작
            yield return new WaitForSeconds(1f);
            Restart_Game();
        }

        private IEnumerator Display_Texts()
        {
            while (m_currentTextIndex < m_texts.Length)
            {
                yield return StartCoroutine(Type_Text(m_texts[m_currentTextIndex]));
                m_currentTextIndex++;

                // 텍스트 표시 후 2초 대기
                yield return new WaitForSeconds(2f);
            }
        }

        private IEnumerator Type_Text(string text)
        {
            m_dialogText.text = "";
            foreach (char letter in text)
            {
                m_dialogText.text += letter;
                yield return new WaitForSeconds(m_typingSpeed);
            }
        }

        private IEnumerator Move_Position(Vector2 targetPosition, float duration)
        {
            float time = 0;
            Vector2 startPosition = m_rt.anchoredPosition;

            while (time < duration)
            {
                time += Time.deltaTime;
                m_rt.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, time / duration);
                yield return null;
            }

            m_rt.anchoredPosition = targetPosition;
        }

        private void Restart_Game()
        {
            // 1라운드 재시작
            Western_PlayLv1 level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>();
            if(level != null)
            {
                Destroy(gameObject);
                level.Restart_Game();
            }
        }
    }
}