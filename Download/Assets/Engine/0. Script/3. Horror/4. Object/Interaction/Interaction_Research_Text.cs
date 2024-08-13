using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Research_Text : Interaction
{
    [SerializeField] UIWorldHint.HINTTYPE m_hintType;
    [SerializeField] private string m_text;

    private void Start()
    {
        GameObject gameObject = HorrorManager.Instance.Create_WorldHintUI(m_hintType, transform, m_uiOffset);
        m_interactionUI = gameObject.GetComponent<UIWorldHint>();
    }

    private void Update()
    {
        Update_InteractionUI();
    }

    public override void Click_Interaction()
    {
        if (m_interactionUI.gameObject.activeSelf == false || m_interact == true)
            return;

        GameObject ui = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Popup", GameObject.Find("Canvas").transform.GetChild(2));
        if (ui == null)
            return;
        UIPopup.Expendables info = new UIPopup.Expendables();
        info.text = m_text;
        ui.GetComponent<UIPopup>().Initialize_UI(UIPopup.TYPE.T_EXPENDABLES, info);

        Desttoy_Interaction();
    }
}