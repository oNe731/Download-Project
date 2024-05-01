using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yandere_Appear : State<HallwayYandere>
{
    private Animator m_animator;

    public Yandere_Appear(StateMachine<HallwayYandere> stateMachine) : base(stateMachine)
    {
        m_animator = m_stateMachine.Owner.GetComponentInChildren<Animator>();
    }

    public override void Enter_State()
    {
    }

    public override void Update_State()
    {
        // 0일 시 플레이 중이 아니며, 1이거나 1보다 클시 애니메이션 종료
        if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)  // 애니메이션 종료일 시
            m_stateMachine.Change_State((int)HallwayYandere.YandereState.ST_WAIT);
    }

    public override void Exit_State()
    {
    }
}
