using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseUpperBody_Base : State<Monster>
{
    protected FaseUpperBody m_owner = null;

    public FaseUpperBody_Base(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_owner = m_stateMachine.Owner.GetComponent<FaseUpperBody>();
    }


    public override void Enter_State()
    {
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
#if UNITY_EDITOR
#endif
    }

    protected bool Check_Collider(Vector3 dir, int layerIndex) // ~0
    {
        Vector3 startPosition = m_owner.transform.position + m_owner.transform.up * 0.3f;

        RaycastHit hit = GameManager.Ins.Start_Raycast(startPosition, dir, 1f, layerIndex);
        if (hit.collider != null)
            return true;

        return false;
    }
}
