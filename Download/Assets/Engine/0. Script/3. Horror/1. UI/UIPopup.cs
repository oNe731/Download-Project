using Horror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopup : MonoBehaviour
{
    public enum TYPE { T_GETITEM, T_EXPENDABLES, T_END };
    public interface PopupInfo
    {
    }

    public struct ItemInfo : PopupInfo
    {
        public NoteItem.ITEMTYPE type { get; set; }
    }

    public struct Expendables : PopupInfo
    {
        public string text { get; set; }
    }

    private TYPE      m_type = TYPE.T_END;
    private PopupInfo m_popupInfo;

    private float m_time = 0f;

    public void Initialize_UI(TYPE type, PopupInfo popupInfo)
    {
        m_type      = type;
        m_popupInfo = popupInfo;

        switch (m_type)
        {
            case TYPE.T_GETITEM:
                GameObject gameObject = transform.GetChild(0).gameObject;
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                break;

            case TYPE.T_EXPENDABLES: // 문구 출력
                transform.GetChild(1).gameObject.SetActive(true);
                Expendables info = (Expendables)m_popupInfo;
                transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = info.text;

                GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
                break;
        }

        HorrorManager.Instance.Set_Pause(true);
    }

    public void Update()
    {
        if(m_type == TYPE.T_EXPENDABLES)
        {
            m_time += Time.deltaTime;
            if(m_time > 1f)
            {
                HorrorManager.Instance.Set_Pause(false);
                Destroy(gameObject);
            }
        }
    }

    public void Button_Acquire()
    {
        switch (m_type)
        {
            case TYPE.T_GETITEM:
                GameObject ui = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_GetItem", GameObject.Find("Canvas").transform.GetChild(2));
                if (ui == null)
                    return;

                ItemInfo item = (ItemInfo)m_popupInfo;
                ui.GetComponent<UIGetItem>().Initialize_UI(item.type);
                break;
        }

        Destroy(gameObject);
    }

    public void Button_Leave()
    {

    }
}
