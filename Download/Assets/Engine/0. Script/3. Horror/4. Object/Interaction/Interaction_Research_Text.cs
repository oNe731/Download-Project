using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Research_Text : Interaction
{
    [SerializeField] private string m_text;

    private void Start()
    {
        m_interactionUI = HorrorManager.Instance.Create_WorldHintUI(UIWorldHint.HINTTYPE.HT_RESEARCH, transform, m_uiOffset);
        m_interactionUI.SetActive(false);
    }

    private void Update()
    {
        Update_InteractionUI();
    }

    public override void Click_Interaction()
    {
        if (m_interactionUI.activeSelf == false || m_interact == true)
            return;

        GameObject ui = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Popup", GameObject.Find("Canvas").transform.GetChild(2));
        if (ui == null)
            return;
        UIPopup.Expendables info = new UIPopup.Expendables();
        info.text = m_text;
        ui.GetComponent<UIPopup>().Initialize_UI(UIPopup.TYPE.T_EXPENDABLES, info);

        Destroy(m_interactionUI);
        Destroy(gameObject);
    }
}