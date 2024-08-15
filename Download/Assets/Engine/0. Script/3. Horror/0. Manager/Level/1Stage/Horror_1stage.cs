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
    }

    public override bool Check_Clear(ref string text)
    {
        return true;
    }

    public override void Enter_Level()
    {
        // 스테이지 생성
        m_stage = Instantiate(Resources.Load<GameObject>("5. Prefab/3. Horror/Map/Stage1"));

        // 레벨 초기화
        GameObject collider_Area = m_stage.transform.GetChild(1).GetChild(1).GetChild(0).gameObject; // etc -> Collider -> Collider_Area

        m_levels = gameObject.AddComponent<LevelController>();
        List<Level> levels = new List<Level>();
        foreach (Transform child in collider_Area.transform)
            levels.Add(null);
        foreach (Transform child in collider_Area.transform)
        {
            Horror_Base stage = child.gameObject.GetComponent<Horror_Base>();
            stage.Initialize_Level(m_levels);

            levels[stage.LevelIndex] = stage;
        }

        m_levels.Initialize_Level(levels);
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
    }

    public override void OnDrawGizmos()
    {
    }
}
