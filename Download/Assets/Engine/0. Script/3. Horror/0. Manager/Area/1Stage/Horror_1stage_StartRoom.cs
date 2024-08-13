using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horror_1stage_StartRoom : Area
{
    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);

        m_levelIndex = (int)Horror_1stage.LEVEL1.LV_STARTROOM;
    }


    public override bool Check_Clear(ref string text)
    {
        // 파이프를 획득했는가?
        if (HorrorManager.Instance.Player.WeaponManagement.Get_WeaponIndex(NoteItem.ITEMTYPE.TYPE_PIPE) != -1)
            return true;

        text = "문 너머를 확인할 준비가 되지 않은 것 같다.";
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
