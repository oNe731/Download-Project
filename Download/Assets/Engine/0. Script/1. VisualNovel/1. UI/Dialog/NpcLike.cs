using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcLike : MonoBehaviour
{
    [SerializeField] private NPCTYPE m_npcIndex = NPCTYPE.OT_END;
    [SerializeField] private GameObject[] m_heart;

    private bool    m_update = false;
    private Image[] m_images = null;

    public void Set_Owner(NPCTYPE index)
    {
        if (index == NPCTYPE.OT_END)
            return;
        else if (index == NPCTYPE.OT_WHITE)
        {
            Active_Heart(false);
            return;
        }

        Active_Heart(true);
        m_update   = true;
        m_npcIndex = index;
    }

    private void Awake()
    {
        if (null == m_images)
        {
            m_images = new Image[m_heart.Length];
            for (int i = 0; i < m_heart.Length; i++)
                m_images[i] = m_heart[i].GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (NPCTYPE.OT_END == m_npcIndex)
            return;

        if (true == m_update)
        {
            m_update = false;
            Update_Heart();
        }
    }

    public void Update_Heart()
    {
        if (VisualNovelManager.Instance.NpcHeart == null)
            return;

        Sprite spriteOn = null;
        Sprite spriteOff = null;
        switch (m_npcIndex)
        {
            case NPCTYPE.OT_BLUE:
                spriteOn  = VisualNovelManager.Instance.HeartSpr["UI_VisualNovel_Blue_FriendshipHeartON"];
                spriteOff = VisualNovelManager.Instance.HeartSpr["UI_VisualNovel_Blue_FriendshipHeartOFF"];
                break;

            case NPCTYPE.OT_YELLOW:
                spriteOn  = VisualNovelManager.Instance.HeartSpr["UI_VisualNovel_Yellow_FriendshipHeartON"];
                spriteOff = VisualNovelManager.Instance.HeartSpr["UI_VisualNovel_Yellow_FriendshipHeartOFF"];
                break;

            case NPCTYPE.OT_PINK:
                spriteOn  = VisualNovelManager.Instance.HeartSpr["UI_VisualNovel_Pink_FriendshipHeartON"];
                spriteOff = VisualNovelManager.Instance.HeartSpr["UI_VisualNovel_Pink_FriendshipHeartOFF"];
                break;
        }
        
        int currentCount = VisualNovelManager.Instance.NpcHeart[(int)m_npcIndex];
        for (int i = 0; i < currentCount; i++)
        {
            if (i >= m_heart.Length)
                return;

            m_images[i].sprite = spriteOn;
        }

        for (int i = currentCount; i < m_heart.Length; i++)
            m_images[i].sprite = spriteOff;
    }

    private void Active_Heart(bool active)
    {
        for (int i = 0; i < m_heart.Length; i++)
            m_heart[i].SetActive(active);
    }
}
