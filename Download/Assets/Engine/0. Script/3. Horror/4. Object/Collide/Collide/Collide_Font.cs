using System.Collections;
using UnityEngine;

public class Collide_Font : Collide
{
    [SerializeField] private string[] m_text;
    [SerializeField] private float[]  m_activeTimes;

    private void Start()
    {
        for (int i = 0; i < m_text.Length; ++i)
            m_text[i] = m_text[i].Replace("\\n", "\n"); // 인스펙터에서 입력된 문자열은 기본적으로 이스케이프 문자로 인식되지 않고 그냥 일반 문자열로 처리
    }

    public override void Trigger_Event()
    {
        Active_Font();
    }

    public override void Collision_Event()
    {
        Active_Font();
    }

    private void Active_Font()
    {
        GameManager.Ins.Horror.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_FADE, UIInstruction.ACTIVETYPE.TYPE_FADE, m_activeTimes, m_text);
    }
}
