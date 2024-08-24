using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInstruction : MonoBehaviour
{
    public interface InstructionInfo
    {
    }

    public struct Expendables : InstructionInfo
    {
        public string text { get; set; }
    }

    private InstructionInfo m_instructionInfo;
    private float m_time = 0f;

    public void Initialize_UI(InstructionInfo InstructionInfo)
    {
        m_instructionInfo = InstructionInfo;
        m_time = 0f;

        Expendables info = (Expendables)m_instructionInfo;
        transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = info.text;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        m_time += Time.deltaTime;
        if (m_time > 1f)
            gameObject.SetActive(false);
    }
}
