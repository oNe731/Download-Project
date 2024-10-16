using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Idle : Boss1F_Base
{
    public Boss1F_Idle(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        Debug.Log("아이들상태");
    }

    public override void Update_State()
    {
        if (Change_Weakness() == false)
        {
            //Look_Player();
        }
    }

    public override void Exit_State()
    {
    }
}
