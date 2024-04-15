using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogHeart : MonoBehaviour
{
    [SerializeField] private OWNER_TYPE m_npcIndex;
    [SerializeField] private GameObject[] m_heart;
    [SerializeField] private TMP_Text m_nameTxt;

    [SerializeField] private Sprite m_heart_Off;
    [SerializeField] private Sprite m_heart_On;

    private bool    m_update = false;
    private Image[] m_images = null;

    public void Set_Owner(OWNER_TYPE index)
    {
        m_update = true; // 시작하자마자 싱글톤 매니저에게 접근하기 어려워 다른 차선책
        m_npcIndex = index;
    }

    private void Awake()
    {
        if (null == m_images)
        {
            m_images = new Image[m_heart.Length];
            for (int i = 0; i < m_heart.Length; i++)
                m_images[i] = m_heart[i].GetComponent<Image>();

            if (null == m_nameTxt)
                return;

            switch (m_npcIndex)
            {
                case OWNER_TYPE.OT_SIA:
                    m_nameTxt.text = "시아";
                    break;

                case OWNER_TYPE.OT_SOYUL:
                    m_nameTxt.text = "소율";
                    break;

                case OWNER_TYPE.OT_JIU:
                    m_nameTxt.text = "지우";
                    break;
            }
        }
    }

    private void Update()
    {
        if (OWNER_TYPE.OT_END == m_npcIndex)
            return;

        if (true == m_update)
        {
            m_update = false;
            Update_Heart();
        }
    }

    public void Update_Heart()
    {
        int currentCount = VisualNovelManager.Instance.NpcHeart[(int)m_npcIndex];
        for (int i = 0; i < currentCount; i++)
        {
            if (i >= m_heart.Length)
                return;

            m_images[i].sprite = m_heart_On;
        }

        for (int i = currentCount; i < m_heart.Length; i++)
            m_images[i].sprite = m_heart_Off;
    }
}
