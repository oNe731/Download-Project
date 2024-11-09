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
        // ������̴� BGM ����
        m_audioSource = Camera.main.GetComponent<AudioSource>();
        m_audioSource.Stop();

        m_event = EVENT.EVENT_APPEAR;
    }

    private void Update()
    {
        if(m_event == EVENT.EVENT_APPEAR)
        {
            // 3�� �� �ٴڿ��� �ǳڵ��� ���� �ö�´�.
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
            // �ǳڵ��� �� �ö���� 1�� �� ���� �ѹ��� ������.
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
            // ���� ���� �� 2�� �� �뷡 + �� �ִϸ��̼� ����ȴ�.
            m_time += Time.deltaTime;
            if (m_time > 2f)
            {
                m_time = 0f;
                // �뷡 ���
                GameManager.Ins.Sound.Play_AudioSourceBGM("Western_BarBGM", false, 1f);

                // �� �ִϸ��̼� ���
                m_peoples.Dance_Peoples();

                // �ϴÿ��� �ɰ��� ��ƼŬ�� ��������.
                GameObject particle = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/1Stage/Effect/Particle_Flower");
                particle.transform.position = new Vector3(0f, 1.471f, 19.72f);
                particle.transform.rotation = Quaternion.Euler(new Vector3(68.88f, - 90f, -90f));

                m_event = EVENT.EVENT_FINISH;
            }
        }
        else if (m_event == EVENT.EVENT_FINISH)
        {
            // �뷡�� �������뿡 �ִϸ��̼��� �ٲ�� ���׸��� §�ϰ� ���� ������.
            if(m_audioSource.clip.length - m_audioSource.time <= 1f) // ����ð��� Ư�� ����ŭ ���Ҵٸ�
            {
                // �ִϸ��̼� ��ȯ
                m_peoples.Finish_Peoples();
                m_event = EVENT.EVENT_NEXT;
            }
        }
        else if (m_event == EVENT.EVENT_NEXT)
        {
            // ȭ�� ���̵� �ƿ����� ���� �� Ŭ���� �������� ���� ��ȯ
            if(Camera.main.GetComponent<AudioSource>().isPlaying == false)
            {
                m_time += Time.deltaTime;
                if (m_time > 2f)
                {
                    m_time = 0f;
                    GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Western.LevelController.Change_NextLevel(), 0f, false);
                    m_event = EVENT.EVENT_END;
                }
            }
        }
    }
}
