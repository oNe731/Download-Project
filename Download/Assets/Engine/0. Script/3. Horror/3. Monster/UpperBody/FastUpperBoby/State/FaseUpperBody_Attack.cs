using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseUpperBody_Attack : FaseUpperBody_Base
{
    public FaseUpperBody_Attack(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        // 폭발 이펙트 생성
        // 

        if(Get_Attacked() == true)
            HorrorManager.Instance.Player.Damage_Player(m_owner.Attack);
    }

    public override void Update_State()
    {
        m_owner.StateMachine.Change_State((int)FaseUpperBody.State.ST_DIE); // 한번의 데미지만 안겨줌
    }

    public override void Exit_State()
    {
    }

    private bool Get_Attacked()
    {
        float distanceToPlayer = Vector3.Distance(m_stateMachine.Owner.transform.position, HorrorManager.Instance.Player.transform.position);
        if (distanceToPlayer <= 2f)
            return true;

        return false;
    }
}
