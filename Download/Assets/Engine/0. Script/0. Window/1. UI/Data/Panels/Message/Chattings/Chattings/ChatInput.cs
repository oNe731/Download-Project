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
        //m_inputField.ActivateInputField(); // �Է� ���� �� ��Ŀ�� ����
    }

    public void Button_Call() // ��ȭ ��ư
    {
        if (m_callCoroutine == null)
            m_callCoroutine = StartCoroutine(CallCoroutine());
    }

    private IEnumerator CallCoroutine()
    {
        // ��ǲ�ʵ� �Է� ��Ȱ��ȭ
        m_inputField.enabled = false;

        string message = "��ȭ�ϴ� ���Դϴ�."; // �⺻ �޽���
        int dotCount = 0;

        float time = 0;
        while (time < 0.2f)
        {
            time += Time.deltaTime;

            string dots = new string('.', dotCount); // �� ����
            m_inputField.text = message + dots;
            yield return new WaitForSeconds(0.5f);

            // ���� ���� 0���� 5���� �ݺ�
            dotCount++;
            if (dotCount > 3)
                dotCount = 0;
        }

        // ���� ���� �޽��� ���
        m_inputField.text = "������ �����߽��ϴ�.";

        // ���
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

    public void Button_Send() // �޼��� ���� ��ư
    {
        if (m_callCoroutine != null || m_inputField.text == "")
            return;

        #region �޽��� �߰�
        Chatting chatting = new Chatting();
        chatting.type = ChattingData.COMMUNICANTSTYPE.CT_RECEIVER;
        chatting.text = m_inputField.text;

        DateTime currentTime = DateTime.Now;
        string amPm = currentTime.Hour < 12 ? "AM" : "PM";
        int hour = currentTime.Hour % 12; // 12�ð� ����
        hour = hour == 0 ? 12 : hour; // 0�� -> 12�÷� ǥ��
        chatting.time = $"{amPm} {hour}:{currentTime.Minute.ToString("00")}"; // �� 2�ڸ��� ���

        chatting.fontColor = new List<float> { 0f, 0f, 0f, 1f };
        GameManager.Ins.Window.Chatting.Add_ChattingData("", chatting);
        #endregion

        // �ʱ�ȭ
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
