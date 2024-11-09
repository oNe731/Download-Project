using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;

public class SystemTime : MonoBehaviour
{
    private TMP_Text m_timeTxt;

    private void Start()
    {
        m_timeTxt = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        DateTime currentTime = DateTime.Now;

        string amPm = currentTime.Hour < 12 ? "����" : "����";
        int hour = currentTime.Hour % 12; // 12�ð� ����
        hour = hour == 0 ? 12 : hour;     // 0�� -> 12�÷� ǥ��

        m_timeTxt.text = amPm + " " + hour + ":" + currentTime.Minute.ToString("00"); // �� 2�ڸ��� ���
    }
}
