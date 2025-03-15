using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBackground : MonoBehaviour
{
    [SerializeField] private Sprite[] m_sprites;

    private int m_currentIndex = 0;
    private float m_time = 0;

    private Image m_image;

    private void Start()
    {
        m_image = GetComponent<Image>();
        m_image.sprite = m_sprites[m_currentIndex];
    }

    private void Update()
    {
        m_time += Time.deltaTime;
        if(m_time >= 3f)
        {
            m_time = 0f;
            Change_Sprite();
        }
    }

    private void Change_Sprite()
    {
        int newIndex = 0;
        do
        {
            newIndex = Random.Range(0, m_sprites.Length);
        } while (newIndex == m_currentIndex);

        m_currentIndex = newIndex;
        m_image.sprite = m_sprites[m_currentIndex];
    }
}
