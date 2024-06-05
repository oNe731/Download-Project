using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemText : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    private float m_time = 0f;

    public void Start_ItemText(string str)
    {
        m_time = 0;
        m_text.text = str;
        gameObject.SetActive(true);
    }

    public void Update()
    {
        m_time += Time.deltaTime;
        if(m_time > 5f)
        {
            gameObject.SetActive(false);
        }
    }
}
