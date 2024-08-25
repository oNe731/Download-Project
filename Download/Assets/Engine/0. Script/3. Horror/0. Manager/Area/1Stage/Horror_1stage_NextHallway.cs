using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horror_1stage_NextHallway : Area
{
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
            Horror.Note note = HorrorManager.Instance.Player.Note;
            if (note != null)
            {
                if (note.Check_Item(NoteItem.ITEMTYPE.TYPE_1KEY))
                    return true;
            }

            activeTimes = new float[1];
            texts = new string[1];
            activeTimes[0] = 1f;
            texts[0] = "잠겨있다. 열쇠가 필요해 보인다.";
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
}
