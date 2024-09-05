using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    private enum EVENT { EVENT_APPEAR, EVENT_LIGHT, EVENT_DANCE, EVENT_FINISH, EVENT_NEXT, EVENT_END };

    [SerializeField] private BarPeoples m_peoples;
    [SerializeField] private GameObject m_light;
    private EVENT m_event = EVENT.EVENT_END;
    private float m_time = 0f;
    private AudioSource m_audioSource;

    private void Start()
    {
        m_light.SetActive(false);
    }

    public void Start_Event()
    {
        // 재생중이던 BGM 중지
        m_audioSource = Camera.main.GetComponent<AudioSource>();
        m_audioSource.Stop();

        m_event = EVENT.EVENT_APPEAR;
    }

    private void Update()
    {
        if(m_event == EVENT.EVENT_APPEAR)
        {
            // 3초 뒤 바닥에서 판넬들이 위로 올라온다.
            m_time += Time.deltaTime;
            if(m_time > 2f)
            {
                m_time = 0f;
                StartCoroutine(m_peoples.Start_Up());
                m_event = EVENT.EVENT_LIGHT;
            }
        }
        else if (m_event == EVENT.EVENT_LIGHT)
        {
            // 판넬들이 다 올라오고 1초 뒤 불이 한번에 켜진다.
            if (m_peoples.IsUp == true)
            {
                m_time += Time.deltaTime;
                if (m_time > 1f)
                {
                    m_time = 0f;
                    m_light.SetActive(true);
                    m_event = EVENT.EVENT_DANCE;
                }
            }
        }
        else if (m_event == EVENT.EVENT_DANCE)
        {
            // 불이 켜진 뒤 2초 뒤 노래 + 댄스 애니메이션 재생된다.
            m_time += Time.deltaTime;
            if (m_time > 2f)
            {
                m_time = 0f;
                // 노래 재생
                GameManager.Ins.Sound.Play_AudioSourceBGM("Western_BarBGM", false, 1f);

                // 댄스 애니메이션 재생
                m_peoples.Dance_Peoples();

                // 하늘에서 꽃가루 파티클이 떨어진다.
                GameObject particle = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/1Stage/Effect/Particle_Flower");
                particle.transform.position = new Vector3(0f, 1.471f, 19.72f);
                particle.transform.rotation = Quaternion.Euler(new Vector3(68.88f, - 90f, -90f));

                m_event = EVENT.EVENT_FINISH;
            }
        }
        else if (m_event == EVENT.EVENT_FINISH)
        {
            // 노래가 막마지쯤에 애니메이션이 바뀌고 빙그르르 짠하고 댄스가 끝난다.
            if(m_audioSource.clip.length - m_audioSource.time <= 1f) // 재생시간이 특정 값만큼 남았다면
            {
                // 애니메이션 전환
                m_peoples.Finish_Peoples();
                m_event = EVENT.EVENT_NEXT;
            }
        }
        else if (m_event == EVENT.EVENT_NEXT)
        {
            // 화면 페이드 아웃으로 암전 후 클리어 수배지로 레벨 전환
            if(Camera.main.GetComponent<AudioSource>().isPlaying == false)
            {
                m_time += Time.deltaTime;
                if (m_time > 2f)
                {
                    m_time = 0f;
                    GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => WesternManager.Instance.LevelController.Change_NextLevel(), 0f, false);
                    m_event = EVENT.EVENT_END;
                }
            }
        }
    }
}
