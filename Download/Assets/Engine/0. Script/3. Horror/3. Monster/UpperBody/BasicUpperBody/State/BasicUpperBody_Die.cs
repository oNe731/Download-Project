using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUpperBody_Die : BasicUpperBody_Base
{
    public BasicUpperBody_Die(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        // ÅÍÁö´Â º¯½Å ÀÌÆÑÆ® »ý¼º
        // 

        // 2´Ü°è ¸÷ »ý¼º
        GameObject gameObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/Monster/FastUpperBody", m_owner.Spawner.transform);
        if (gameObject == null)
            return;
        Monster monster = gameObject.GetComponent<Monster>();
        if (monster == null)
            return;
        monster.Initialize_Monster(m_owner.Spawner);
        monster.gameObject.transform.localPosition = m_owner.gameObject.transform.localPosition;
        monster.gameObject.transform.localRotation = m_owner.gameObject.transform.localRotation;

        // 1´Ü°è ¸÷ ¼Ò¸ê
        GameManager.Ins.Resource.Destroy(m_owner.gameObject);
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }
}
