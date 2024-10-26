using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusMessage : MonoBehaviour
{
    private TMP_InputField m_statusText;
    private Image m_boxImage;

    private bool m_isInput = false;
    private string m_inputText;

    private void Start()
    {
        m_statusText = GetComponent<TMP_InputField>();
        m_boxImage = GetComponent<Image>();

        m_statusText.onSelect.AddListener(Input_Start);
        m_statusText.onValueChanged.AddListener(Input_Changed);
        m_statusText.onEndEdit.AddListener(Input_End);
    }

    private void Input_Start(string text)
    {
        m_isInput = true;
        m_boxImage.color = new Color(0f, 0f, 0f, 0.2f);
        //m_statusText.ActivateInputField(); // 입력 시작 시 포커스 설정
    }

    private void Input_Changed(string text)
    {
        m_inputText = m_statusText.text;
    }

    private void Input_End(string text)
    {
        m_isInput = false;
        m_boxImage.color = new Color(0f, 0f, 0f, 0f);
        GameManager.Ins.Window.StatusText = m_inputText;
    }

    public void Update()
    {
        if (m_statusText == null || m_isInput == true)
            return;

        m_statusText.text = GameManager.Ins.Window.StatusText;
    }
}
