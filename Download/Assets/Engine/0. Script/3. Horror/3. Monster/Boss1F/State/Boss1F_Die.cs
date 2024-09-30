using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Die : Boss1F_Base
{
    public Boss1F_Die(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        GameObject item = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/Object/Item/Research_1FKey");
        if (item != null)
            item.transform.position = new Vector3(12.314f, 1.265f, 7.82f);

        GameManager.Ins.Resource.Destroy(m_owner.gameObject);
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }
}
