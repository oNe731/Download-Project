using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatInput : MonoBehaviour
{
    private TMP_InputField m_inputField;

    private Coroutine m_scrollCoroutine;
    private Coroutine m_callCoroutine;

    private void Start()
    {
        m_inputField = transform.GetChild(2).GetComponent<TMP_InputField>();
        m_inputField.onSelect.AddListener(Input_Start);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Button_Send();
        }
    }

    private void Input_Start(string text)
    {
        //m_inputField.ActivateInputField(); // 입력 시작 시 포커스 설정
    }

    public void Button_Call() // 전화 버튼
    {
        if (m_callCoroutine == null)
            m_callCoroutine = StartCoroutine(CallCoroutine());
    }

    private IEnumerator CallCoroutine()
    {
        // 인풋필드 입력 비활성화
        m_inputField.enabled = false;

        string message = "전화하는 중입니다."; // 기본 메시지
        int dotCount = 0;

        float time = 0;
        while (time < 0.2f)
        {
            time += Time.deltaTime;

            string dots = new string('.', dotCount); // 점 생성
            m_inputField.text = message + dots;
            yield return new WaitForSeconds(0.5f);

            // 점의 개수 0부터 5까지 반복
            dotCount++;
            if (dotCount > 3)
                dotCount = 0;
        }

        // 연결 실패 메시지 출력
        m_inputField.text = "연결을 실패했습니다.";

        // 대기
        time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        m_callCoroutine = null;
        m_inputField.enabled = true;
        m_inputField.text = "";
    }

    public void Button_Send() // 메세지 전송 버튼
    {
        if (m_callCoroutine != null || m_inputField.text == "")
            return;

        #region 메시지 추가
        Chatting chatting = new Chatting();
        chatting.type = ChattingData.COMMUNICANTSTYPE.CT_RECEIVER;
        chatting.text = m_inputField.text;

        DateTime currentTime = DateTime.Now;
        string amPm = currentTime.Hour < 12 ? "AM" : "PM";
        int hour = currentTime.Hour % 12; // 12시간 형식
        hour = hour == 0 ? 12 : hour; // 0시 -> 12시로 표시
        chatting.time = $"{amPm} {hour}:{currentTime.Minute.ToString("00")}"; // 분 2자리로 출력

        chatting.fontColor = new List<float> { 0f, 0f, 0f, 1f };
        GameManager.Ins.Window.Chatting.Add_ChattingData("", chatting);
        #endregion

        // 초기화
        m_inputField.text = "";
        if (m_scrollCoroutine != null)
            StopCoroutine(m_scrollCoroutine);
        m_scrollCoroutine = StartCoroutine(Move_ScrollBottom());
    }

    private IEnumerator Move_ScrollBottom()
    {
        yield return new WaitForSeconds(0.1f);
        GameManager.Ins.Window.Chatting.ScrollRect.verticalNormalizedPosition = 0f;
    }
}
