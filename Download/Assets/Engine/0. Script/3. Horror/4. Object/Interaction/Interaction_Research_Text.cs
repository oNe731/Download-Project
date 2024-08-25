using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Research_Text : Interaction
{
    [SerializeField] UIWorldHint.HINTTYPE m_hintType;
    [SerializeField] private float[]  m_activeTimes;
    [SerializeField] private string[] m_texts;

    private void Start()
    {
        GameObject gameObject = HorrorManager.Instance.Create_WorldHintUI(m_hintType, transform.GetChild(0), m_uiOffset);
        m_interactionUI = gameObject.GetComponent<UIWorldHint>();
    }

    private void Update()
    {
        //Update_InteractionUI();
    }

    public override void Click_Interaction()
    {
        if (m_interactionUI.gameObject.activeSelf == false || m_interact == true)
            return;

        HorrorManager.Instance.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_BASIC, UIInstruction.ACTIVETYPE.TYPE_BASIC, m_activeTimes, m_texts);
        Destroy_Interaction();
    }
}