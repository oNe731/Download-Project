using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Western
{
    public class KeyUI : MonoBehaviour
    {
        private GameObject m_Owner;
        private KeyCode m_keyType = KeyCode.None;

        private Image m_Image;

        public GameObject Owner { set { m_Owner = value; } }
        public KeyCode KeyType { set { m_keyType = value; } }

        private void Start()
        {
            m_Image = GetComponent<Image>();
            switch (m_keyType)
            {
                case KeyCode.Q:
                    m_Image.sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/Play/Bomb/Q");
                    break;
                case KeyCode.W:
                    m_Image.sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/Play/Bomb/W");
                    break;
                case KeyCode.E:
                    m_Image.sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/Play/Bomb/E");
                    break;
                case KeyCode.R:
                    m_Image.sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/2. Western/UI/Play/Bomb/R");
                    break;
            }
        }

        private void LateUpdate()
        {
            if (m_Owner == null)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = Camera.main.WorldToScreenPoint(m_Owner.transform.position + new Vector3(-0.08f, 0.08f, 0));
        }
    }
}

