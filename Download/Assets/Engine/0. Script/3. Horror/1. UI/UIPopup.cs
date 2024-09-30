using Horror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour
{
    public enum TYPE { T_NOTE, T_WEAPON, T_QUESTITEM, T_EXPENITEM, T_CLUE, T_END };
    public enum EVENT { E_NONE, E_FIRSTBULLET, E_END };

    private TYPE      m_type = TYPE.T_END;
    private NoteItem  m_itemInfo;

    private bool      m_closeText = false;
    private float[]   m_activeTimes;
    private string[]  m_texts;

    private bool  m_closeEvent = false;
    private EVENT m_eventType  = EVENT.E_END;

    public void Initialize_UI(TYPE type, NoteItem itemInfo, bool closeText, float[] activeTimes, string[] texts, bool closeEvent, EVENT eventType)
    {
        m_type      = type;
        m_itemInfo  = itemInfo;

        m_closeText   = closeText;
        m_activeTimes = activeTimes;
        m_texts       = texts;

        m_closeEvent = closeEvent;
        m_eventType  = eventType;

        GameManager.Ins.Set_Pause(true); // 게임 일시정지
        if (m_type == TYPE.T_QUESTITEM) // 퀘스트 조합 아이템 (가져가기/ 두고가기)
        {
            transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200f, -200f);
            transform.GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(200f, -200f);
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true); // 두고가기 버튼
        }
        else // 노트, 장비, 소모품 아이템, 단서 (가져가기)
        {
            transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -200f);
            transform.GetChild(0).GetChild(1).gameObject.SetActive(false); // 두고가기 버튼
        }
        transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = GameManager.Ins.Horror.NoteElementIcon[m_itemInfo.m_imageName + "_1"];

        gameObject.SetActive(true);
    }

    public void Button_Acquire()
    {
        GameManager.Ins.Sound.Play_ManagerAudioSource("Horror_GetItem", false, 1f);

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

        GameManager.Ins.Set_Pause(false); // 일시정지 해제
        if(m_closeText == true)
        {
            m_closeText = false;
            GameManager.Ins.Horror.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_FADE, UIInstruction.ACTIVETYPE.TYPE_FADE, m_activeTimes, m_texts);
        }
        if(m_closeEvent == true)
        {
            m_closeEvent = false;
            switch(m_eventType)
            {
                case EVENT.E_FIRSTBULLET:
                    GameManager.Ins.StartCoroutine(Event_Bullet());
                    break;
            }
        }
        gameObject.SetActive(false);
    }

    public void Button_Leave()
    {
        GameManager.Ins.Set_Pause(false); // 일시정지 해제
        gameObject.SetActive(false);
    }


    public void Type_QuestItem()
    {
        // 퀘스트 조합용 아이템 추가
        Note playerNote = GameManager.Ins.Horror.Player.Note;
        if (playerNote == null)
            return;
        playerNote.Add_Item(m_itemInfo);
    }

    public void Type_Note()
    {
        GameManager.Ins.Horror.Player.Acquire_Note(); // 노트 추가

        float[] activeTimes = new float[2];
        string[] texts = new string[2];
        activeTimes[0] = 2f;
        texts[0] = "동료가 사용하던 수첩인 듯 하다...\n내용을 살펴보자.";
        activeTimes[1] = 3f;
        texts[1] = "[TAB]으로 수첩 사용 가능";

        GameManager.Ins.Horror.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_FADE, UIInstruction.ACTIVETYPE.TYPE_FADE, activeTimes, texts); // 문구 출력
    }

    public void Type_Weapon()
    {
        // 아이템 추가
        GameManager.Ins.Horror.Player.WeaponManagement.Add_Weapon(m_itemInfo);

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
        GameManager.Ins.Horror.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_FADE, UIInstruction.ACTIVETYPE.TYPE_FADE, activeTimes, texts);
    }

    public void Type_Expenitem()
    {
        // 소모품 아이템 추가
        Note playerNote = GameManager.Ins.Horror.Player.Note;
        if (playerNote == null)
            return;
        playerNote.Add_Item(m_itemInfo);
    }

    public void Type_Clue()
    {
        // 단서 추가
        Note playerNote = GameManager.Ins.Horror.Player.Note;
        if (playerNote == null)
            return;
        playerNote.Add_Clue(m_itemInfo);
    }


    public IEnumerator Event_Bullet()
    {
        Horror_Base level = GameManager.Ins.Horror.LevelController.Get_CurrentLevel<Horror_Base>();
        HorrorLight light = level.Light.transform.GetChild(0).GetComponent<HorrorLight>();

        float time = 0f;
        while (true)
        {
            if(GameManager.Ins.IsGame == true)
            {
                time += Time.deltaTime;
                if(time >= 0.5f) // 총알 획득창을 닫고 1초 뒤...
                {
                    // 거울이 깨지며 (유리조각 이펙트)
                    //

                    // 붉은 글씨가 나타난다.들어가는 문구: “DO YOU RECOGNIZE ME” (지금은 깨진거울없이 그냥 거울 위에)
                    light.Light.enabled = false;

                    GameObject doyou = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/Effect/BloodPont/DoYou/DoYou", level.Stage.transform);
                    doyou.transform.position = new Vector3(-13.29f, 2.045f, 15.52f);
                    doyou.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
                    doyou.transform.localScale = new Vector3(0.3377168f, 2.045f, 0.1591384f);
                    break;
                }
            }

            yield return null;
        }

        time = 0f;
        while (true)
        {
            if (GameManager.Ins.IsGame == true)
            {
                time += Time.deltaTime;
                if (time >= 0.2f)
                {
                    light.Light.enabled = true;
                    light.Start_Blink(true, 0.4f, 0.8f, true, 3f); // 화장실 불이 깜빡거린다.
                    break;
                }
            }

            yield return null;
        }

        yield break;
    }
}
