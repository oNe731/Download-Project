using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horror_1stage_NextHallway : Area
{
    private bool m_event = false;

    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);

        m_levelIndex = (int)Horror_1stage.LEVEL1.LV_NEXTHALLWAY;
    }

    public override bool Check_Clear(Interaction_Door interaction_Door, ref float[] activeTimes, ref string[] texts)
    {
        // D 
        if(interaction_Door.DoorIndex == 4)
        {
            // 열쇠가 있는가?
            Horror.Note note = GameManager.Ins.Horror.Player.Note;
            if (note != null)
            {
                if (note.Check_Item(NoteItem.ITEMTYPE.TYPE_1FKEY))
                    return true;
            }

            activeTimes = new float[1];
            texts = new string[1];
            activeTimes[0] = 1f;
            texts[0] = "잠겨있다. 열쇠가 필요해 보인다.";

            // 이벤트 발생
            if(m_event == false)
            {
                m_event = true;
                GameManager.Ins.StartCoroutine(Event_Door(texts[0]));
            }
        }
        // H
        else if(interaction_Door.DoorIndex == 8)
        {
            activeTimes = new float[1];
            texts = new string[1];
            activeTimes[0] = 1f;
            texts[0] = "열리지 않는다. 반대쪽에서 열 수 있을 것 같은 구조다.";
        }

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

    private IEnumerator Event_Door(string text)
    {
        GameManager.Ins.Set_Pause(true, false); // 게임 일시정지

#region 안내 문구가 끝났다면 이벤트 발생
        bool active = false;
        UIInstruction instruction = GameManager.Ins.Horror.InstructionUI;

        while (true)
        {
            if(active == false)
            {
                if(instruction.Texts != null)
                {
                    if (instruction.Texts.Length != 0)
                        active = true;
                }
            }
            else
            {
                if (instruction.Texts == null)
                    break;
            }

             yield return null;
        }
        #endregion

        Horror_Base level = GameManager.Ins.Horror.LevelController.Get_CurrentLevel<Horror_Base>();
        Dummy dummy = level.Stage.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetChild(2).GetComponent<Dummy>();
        dummy.Fall_Dummy(); // 더미 오브젝트 이벤트 발생.

#region 카메라 쉐이킹        
        CameraFollow camera = (CameraFollow)GameManager.Ins.Camera.Get_CurCamera();
        camera.Start_Shake(3f, 1.5f);
        GameManager.Ins.Camera.ShakeUpdate = true;

        while (true)
        {
            if (camera == null || camera.IsShake == false)
                break; 
            yield return null;
        }
        GameManager.Ins.Camera.ShakeUpdate = false;
        #endregion

        GameObject tigerDoor = level.Stage.transform.GetChild(1).GetChild(3).GetChild(0).gameObject;
        tigerDoor.SetActive(true);

        GameManager.Ins.Set_Pause(false, false); // 게임 일시정지 해제

        yield break;
    }
}
