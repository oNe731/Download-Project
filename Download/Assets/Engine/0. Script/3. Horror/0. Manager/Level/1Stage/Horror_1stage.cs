using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horror_1stage : Horror_Base
{
    public enum LEVEL1 { LV_STARTROOM, LV_STARTHALLWAY, LV_NEXTROOM, LV_RESTROOM, LV_NEXTHALLWAY, LV_FINISHROOM, LV_BIGROOMOUT, LV_BIGROOMIN, LV_END };

    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);

        m_levelIndex = (int)HorrorManager.LEVEL.LV_1STAGE;
        m_playerSpeedAdd = 0;
    }

    public override bool Check_Clear(Interaction_Door interaction_Door, ref float[] activeTimes, ref string[] texts)
    {
        return true;
    }

    public override void Enter_Level()
    {
        if(m_IsVisit == false)
        {
            m_IsVisit = true;
            m_stage = GameObject.Find("World").transform.GetChild(1).gameObject;
            m_light = m_stage.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;

            // 레벨 초기화
            m_levels = new LevelController();
            List<Level> levels = new List<Level>
            {
                new Horror_1stage_StartRoom(),
                new Horror_1stage_StartHallway(),
                new Horror_1stage_NextRoom(),
                new Horror_1stage_RestRoom(),
                new Horror_1stage_NextHallway(),
                new Horror_1stage_FinishRoom(),
                new Horror_1stage_BigRoomOut(),
                new Horror_1stage_BigRoomIn(),
            };
            for (int i = 0; i < levels.Count; ++i)
                levels[i].Initialize_Level(m_levels);
            m_levels.Initialize_Level(levels, (int)LEVEL1.LV_STARTROOM);
        }
        else
        {
            // 플레이어 위치 및 회전 변경
            Transform playerTransform = GameManager.Ins.Horror.Player.gameObject.transform;
            playerTransform.position = new Vector3(26.38f, 0f, 24.99f);
            playerTransform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }

        m_stage.SetActive(true);
    }

    public override void Play_Level()
    {
    }

    public override void Update_Level()
    {
        m_levels.Update_Level();
    }

    public override void LateUpdate_Level()
    {
    }

    public override void Exit_Level()
    {
        m_stage.SetActive(false);
    }

    public override void OnDrawGizmos()
    {
    }
}
