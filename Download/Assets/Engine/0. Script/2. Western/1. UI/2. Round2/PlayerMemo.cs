using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMemo : MonoBehaviour
{
    [SerializeField] private GameObject[] m_lists;
    private int m_index = 0;

    private float m_time = 0f;
    private float m_waitTime = 1f;
    private bool m_isCheck = false;

    public void Check_List()
    {
        m_time = 0;
        m_isCheck = true;
    }

    public void Update()
    {
        if (m_isCheck == false)
            return;

        m_time += Time.deltaTime;
        if(m_time >= m_waitTime)
        {
            m_isCheck = false;
            m_lists[m_index].transform.GetChild(0).gameObject.SetActive(true);
            m_index++;
        }
    }
}
