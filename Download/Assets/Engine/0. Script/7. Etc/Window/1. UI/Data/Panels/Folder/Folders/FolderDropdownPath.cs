using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FolderDropdownPath : MonoBehaviour, IPointerClickHandler
{
    public enum STATE { ST_BEFORE, ST_NEXT, ST_END }

    private FolderPathInput m_owner;

    private STATE m_state = STATE.ST_END;
    private int m_count = 0;


    private TMP_Text m_pathTxt;

    public void Initialize_Dropdown(FolderPathInput owner)
    {
        m_owner = owner;
        m_pathTxt = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    public void Set_Dropdown(STATE state, int count, string path)
    {
        m_state = state;
        m_count = count;
        m_pathTxt.text = path;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_state == STATE.ST_END)
        {
            m_owner.DropDownPanel.SetActive(false);
            return;
        }

        Panel_Folder folder = GameManager.Ins.Window.Folder;
        switch (m_state)
        {
            case STATE.ST_BEFORE:
                for(int i = 0; i < m_count; ++i)
                    folder.Folder_Back();
                break;

            case STATE.ST_NEXT:
                for (int i = 0; i < m_count; ++i)
                    folder.Folder_Again();
                break;
        }

        m_owner.DropDownPanel.SetActive(false);
    }
}
