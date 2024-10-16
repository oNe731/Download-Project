using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Hit : Boss1F_Base
{
    private string m_name;

    public Boss1F_Hit(StateMachine<Monster> stateMachine) : base(stateMachine)
    {

    }

    public override void Enter_State()
    {
        if (m_owner.StateMachine.PreState == (int)Boss1F.State.ST_WEAKNESS)
            m_name = "IsWeakHit";
        else
            m_name = "IsHit";

        m_animator.speed = 1f;
        m_animator.SetLayerWeight(1, 0f);
        m_animator.SetLayerWeight(2, 0f);

        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(m_name) == true)
            m_animator.Play(m_name, 0, 0f); // 트랜지션 없이 변경
        else
            m_animator.SetBool(m_name, true);

        Debug.Log("맞음상태");
    }

    public override void Update_State()
    {
        if (Change_Weakness() == false)
        {
            if (m_animator.IsInTransition(0) == true)
                return;

            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_name) == true)
            {
                m_animator.SetBool(m_name, false);

                float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (animTime >= 1f) // 애니메이션 종료 1.0f
                {
                    if (m_owner.StateMachine.PreState == (int)Boss1F.State.ST_APPEAR)
                        m_stateMachine.Change_State((int)Boss1F.State.ST_APPEAR);
                    else if (m_owner.StateMachine.PreState == (int)Boss1F.State.ST_WAIT)
                        m_stateMachine.Change_State((int)Boss1F.State.ST_WAIT);
                    else if (m_owner.StateMachine.PreState == (int)Boss1F.State.ST_WEAKNESS)
                        m_stateMachine.Change_State((int)Boss1F.State.ST_WEAKNESS);
                    else
                        m_stateMachine.Change_State((int)Boss1F.State.ST_IDLE);
                }
            }
        }
    }

    public override void Exit_State()
    {

    }
}
