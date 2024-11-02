using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBar : MonoBehaviour
{
    private int m_barCount    = 0;
    private int m_changeCount = 7;

    private float m_moveSpeed = 500f;
    private float m_position  = 391.6f;

    private RectTransform m_gage;

    private void Start()
    {
        m_gage = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        m_gage.anchoredPosition = new Vector2(-m_position, -0.2999878f);
    }

    private void Update()
    {
        if (GameManager.Ins.IsGame == false)
            return;

        m_gage.anchoredPosition += new Vector2(m_moveSpeed * Time.deltaTime, 0f);

        if (m_gage.anchoredPosition.x >= m_position)
        {
            m_barCount++;
            if(m_barCount >= m_changeCount)
                GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_LOGIN);
            else
                m_gage.anchoredPosition = new Vector2(-m_position, -0.2999878f);
        }
    }
}
