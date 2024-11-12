using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour
{
    [SerializeField] private Sprite[] m_sprites;

    private int m_index = 0;
    private Image m_image;

    private void Start()
    {
        m_image = GetComponent<Image>();
    }

    public void Change_Portrait()
    {
        m_image.sprite = m_sprites[m_index];
        m_index++;
    }
}
