using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObstacleDialog : MonoBehaviour
{
    [SerializeField] private TMP_Text m_dialogTxt;

    private bool m_isUpdate = false;

    private bool m_isStart = false;
    private float m_startTime = 0f;

    private float m_time = 0;
    private float m_waitTime = 2f;

    private string[] m_texts;
    private string[] m_sounds;
    private Coroutine m_dialogTextCoroutine = null;
    private bool m_isTyping = false;
    private int m_dialogIndex = 0;
    private float m_typeSpeed = 0.05f;

    private AudioSource m_audioSource;

    public void Start_Dialog(string[] texts, string[] sounds, float startTime)
    {
        m_audioSource = transform.GetComponent<AudioSource>();

        m_texts = texts;
        m_sounds = sounds;

        m_startTime = startTime;
        m_isStart = false;

        m_time = 0f;
        m_isUpdate = true;
        m_isTyping = false;
        m_dialogIndex = 0;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (GameManager.Ins.IsGame == false)
            return;

        if (m_isUpdate == false || m_isTyping == true)
            return;

        m_time += Time.deltaTime;
        if (m_isStart == false)
        {
            if (m_time >= m_startTime)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                Update_DialogIndex();

                m_isStart = true;
            }
        }
        else
        {
            if (m_time >= m_waitTime)
            {
                if (!m_isTyping)
                    Update_DialogIndex();
            }
        }
    }

    private void Update_DialogIndex()
    {
        m_time = 0;

        // 다이얼로그 진행
        if (m_dialogIndex < m_texts.Length)
        {
            if(m_sounds[m_dialogIndex] != "")
                GameManager.Ins.Sound.Play_AudioSource(m_audioSource, m_sounds[m_dialogIndex], false, 1f);

            if (m_dialogTextCoroutine != null)
                StopCoroutine(m_dialogTextCoroutine);
            m_dialogTextCoroutine = StartCoroutine(Type_Text(m_dialogIndex));

            m_dialogIndex++;
        }
        else // 다이얼로그 종료
        {
            GameManager.Ins.Western.IsShoot = true;
            Destroy(gameObject);
        }
    }

    IEnumerator Type_Text(int dialogIndex)
    {
        m_texts[dialogIndex] = m_texts[dialogIndex].Replace("{{PLAYER_NAME}}", GameManager.Ins.PlayerName);

        m_isTyping = true;
        m_dialogTxt.text = "";
        foreach (char letter in m_texts[dialogIndex].ToCharArray())
        {
            m_dialogTxt.text += letter;
            yield return new WaitForSeconds(m_typeSpeed);
        }

        m_isTyping = false;
        yield break;
    }
}
