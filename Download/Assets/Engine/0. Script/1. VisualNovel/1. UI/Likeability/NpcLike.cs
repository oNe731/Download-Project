using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VisualNovel
{
    public class NpcLike : MonoBehaviour
    {
        [SerializeField] private VisualNovelManager.NPCTYPE m_npcIndex = VisualNovelManager.NPCTYPE.OT_END;
        [SerializeField] private GameObject[] m_heart;

        private bool m_update = false;
        private Image[] m_images = null;

        public bool HeartClear
        {
            get
            {
                int count = 0;
                for(int i = 0; i < m_heart.Length; ++i) { if (m_heart[i] == null) count++; }
                if (count == m_heart.Length) return true;

                return false;
            }
        }

        public void Set_Owner(VisualNovelManager.NPCTYPE index)
        {
            if (index == VisualNovelManager.NPCTYPE.OT_END)
                return;
            else if (index == VisualNovelManager.NPCTYPE.OT_WHITE)
            {
                Active_Heart(false);
                return;
            }

            Active_Heart(true);
            m_update = true;
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
            if (VisualNovelManager.NPCTYPE.OT_END == m_npcIndex)
                return;

            if (true == m_update)
            {
                m_update = false;
                Update_Heart();
            }
        }

        public void Update_Heart()
        {
            VisualNovelManager manager = GameManager.Ins.Novel;
            if (manager.NpcHeart == null)
                return;

            Sprite spriteOn = null;
            Sprite spriteOff = null;
            switch (m_npcIndex)
            {
                case VisualNovelManager.NPCTYPE.OT_BLUE:
                    spriteOn  = manager.HeartSpr["UI_VisualNovel_Blue_FriendshipHeartON"];
                    spriteOff = manager.HeartSpr["UI_VisualNovel_Blue_FriendshipHeartOFF"];
                    break;

                case VisualNovelManager.NPCTYPE.OT_YELLOW:
                    spriteOn  = manager.HeartSpr["UI_VisualNovel_Yellow_FriendshipHeartON"];
                    spriteOff = manager.HeartSpr["UI_VisualNovel_Yellow_FriendshipHeartOFF"];
                    break;

                case VisualNovelManager.NPCTYPE.OT_PINK:
                    spriteOn  = manager.HeartSpr["UI_VisualNovel_Pink_FriendshipHeartON"];
                    spriteOff = manager.HeartSpr["UI_VisualNovel_Pink_FriendshipHeartOFF"];
                    break;
            }

            int currentCount = manager.NpcHeart[(int)m_npcIndex];
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

        public void Shake_Heart(ref List<int> effectCreate)
        {
            int Count = 2;
            for (int i = 0; i < m_heart.Length; ++i) 
            {
                if (m_heart[i] == null) return;

                Like like = m_heart[i].GetComponent<Like>();
                if (Count > 0 && effectCreate[0] == i) 
                {
                    like.EffectCreate = true;
                    effectCreate.RemoveAt(0);
                    Count--;
                }
                else { like.EffectCreate = false; }

                like.NpcIndex = m_npcIndex;
                like.Shake_Heart();
            }
        }
    }
}

