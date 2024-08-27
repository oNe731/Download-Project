using Horror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour
{
    public enum TYPE { T_NOTE, T_WEAPON, T_QUESTITEM, T_EXPENITEM, T_CLUE, T_END };

    private TYPE      m_type = TYPE.T_END;
    private NoteItem  m_itemInfo;

    public void Initialize_UI(TYPE type, NoteItem itemInfo)
    {
        m_type      = type;
        m_itemInfo  = itemInfo;

        HorrorManager.Instance.Set_Pause(true); // 게임 일시정지
        if (m_type == TYPE.T_QUESTITEM) // 퀘스트 조합 아이템 (가져가기/ 두고가기)
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true); // 두고가기 버튼
        else // 노트, 장비, 소모품 아이템, 단서 (가져가기)
            transform.GetChild(0).GetChild(1).gameObject.SetActive(false); // 두고가기 버튼
        transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = HorrorManager.Instance.NoteElementIcon[m_itemInfo.m_imageName];

        gameObject.SetActive(true);
    }

    public void Button_Acquire()
    {
        switch (m_type)
        {
            case TYPE.T_QUESTITEM: // 퀘스트 조합 아이템
                Type_QuestItem();
                break;
            case TYPE.T_NOTE: // 노트
                Type_Note(); 
                break;
            case TYPE.T_WEAPON: // 장비
                Type_Weapon(); 
                break;
            case TYPE.T_EXPENITEM: // 소모품 아이템
                Type_Expenitem(); 
                break;
            case TYPE.T_CLUE: // 단서
                Type_Clue(); 
                break;
        }

        HorrorManager.Instance.Set_Pause(false); // 일시정지 해제
        gameObject.SetActive(false);
    }

    public void Button_Leave()
    {
        HorrorManager.Instance.Set_Pause(false); // 일시정지 해제
        gameObject.SetActive(false);
    }


    public void Type_QuestItem()
    {
        // 퀘스트 조합용 아이템 추가
        Note playerNote = HorrorManager.Instance.Player.Note;
        if (playerNote == null)
            return;
        playerNote.Add_Item(m_itemInfo);
    }

    public void Type_Note()
    {
        HorrorManager.Instance.Player.Acquire_Note(); // 노트 추가

        float[] activeTimes = new float[2];
        string[] texts = new string[2];
        activeTimes[0] = 2f;
        texts[0] = "동료가 사용하던 수첩인 듯 하다...\n내용을 살펴보자.";
        activeTimes[1] = 3f;
        texts[1] = "[TAB]으로 수첩 사용 가능";

        HorrorManager.Instance.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_FADE, UIInstruction.ACTIVETYPE.TYPE_FADE, activeTimes, texts); // 문구 출력
    }

    public void Type_Weapon()
    {
        // 아이템 추가
        HorrorManager.Instance.Player.WeaponManagement.Add_Weapon(m_itemInfo);

        // 문구 출력
        float[] activeTimes = new float[1];
        string[] texts = new string[1];
        switch (m_itemInfo.m_itemType)
        {
            case NoteItem.ITEMTYPE.TYPE_PIPE:
                activeTimes[0] = 1f;
                texts[0] = "아이템 ‘파이프’를 얻었다.\n[CTRL]으로 무기 사용 가능";
                break;
            case NoteItem.ITEMTYPE.TYPE_GUN:
                activeTimes[0] = 1f;
                texts[0] = "[Ctrl]로 장비교체 가능";
                break;
            case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                activeTimes[0] = 1f;
                texts[0] = "[Ctrl]로 장비교체 가능";
                break;
        }
        HorrorManager.Instance.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_FADE, UIInstruction.ACTIVETYPE.TYPE_FADE, activeTimes, texts);
    }

    public void Type_Expenitem()
    {
        // 소모품 아이템 추가
        Note playerNote = HorrorManager.Instance.Player.Note;
        if (playerNote == null)
            return;
        playerNote.Add_Item(m_itemInfo);
    }

    public void Type_Clue()
    {
        // 단서 추가
        Note playerNote = HorrorManager.Instance.Player.Note;
        if (playerNote == null)
            return;
        playerNote.Add_Clue(m_itemInfo);
    }
}
