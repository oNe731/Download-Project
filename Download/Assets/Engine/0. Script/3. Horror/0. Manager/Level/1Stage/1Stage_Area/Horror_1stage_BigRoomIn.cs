using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horror_1stage_BigRoomIn : Area
{
    private UIKeypad m_keypadUI = null;

    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);

        m_levelIndex = (int)Horror_1stage.LEVEL1.LV_BIGROOMIN;

        // 키패드 UI 생성
        GameObject keypadObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/UI_Keypad", GameObject.Find("Canvas").transform.Find("Panel_Front"));
        m_keypadUI = keypadObject.GetComponent<UIKeypad>();
    }


    public override bool Check_Clear(Interaction_Door interaction_Door, ref float[] activeTimes, ref string[] texts)
    {
        // 비밀번호 단서를 획득한 상태인가?
        Horror.Note note = HorrorManager.Instance.Player.Note;
        if(note != null)
        {
            if (note.Check_Clue(NoteItem.ITEMTYPE.TYPE_KEYPADNUMBER))
            {
                if (m_keypadUI == null)
                    return false;

                m_keypadUI.OnEnable_Keypad(interaction_Door); // 키패드UI 활성화
                return false; // 잠금 해제 성공 시 오픈 가능
            }
        }

        activeTimes = new float[1];
        texts = new string[1];
        activeTimes[0] = 1f;
        texts[0] = "비밀번호가 필요하다.";
        return false;
    }


    public override void Enter_Level()
    {
    }

    public override void Play_Level()
    {
    }

    public override void Update_Level()
    {
    }

    public override void LateUpdate_Level()
    {
    }

    public override void Exit_Level()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
