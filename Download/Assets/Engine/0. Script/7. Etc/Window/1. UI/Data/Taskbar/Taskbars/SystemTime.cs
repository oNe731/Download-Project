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

        string amPm = currentTime.Hour < 12 ? "오전" : "오후";
        int hour = currentTime.Hour % 12; // 12시간 형식
        hour = hour == 0 ? 12 : hour;     // 0시 -> 12시로 표시

        m_timeTxt.text = amPm + " " + hour + ":" + currentTime.Minute.ToString("00"); // 분 2자리로 출력
    }
}
