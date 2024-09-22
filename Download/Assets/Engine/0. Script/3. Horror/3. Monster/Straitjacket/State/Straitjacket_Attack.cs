using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket_Attack : Straitjacket_Base
{
    private bool isAttak = false;

    public Straitjacket_Attack(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        isAttak = false;
        Change_Animation("IsAttack");
    }

    public override void Update_State()
    {
        // 현재 애니메이션 상태 확인
        if (m_animator.IsInTransition(0) == true) return;
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_triggerName) == true)
        {
            Reset_Animation();

            float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animTime >= 1.0f) // 애니메이션 종료
                m_stateMachine.Change_State((int)Straitjacket.State.ST_ATTACKWAIT);
            else
            {
                if (isAttak == false)
                {
                    if (animTime >= 0.35f)
                    {
                        isAttak = true;

                        float distanceToPlayer = Vector3.Distance(m_stateMachine.Owner.transform.position, HorrorManager.Instance.Player.transform.position);
                        if (distanceToPlayer <= m_attackDist)
                        {
                            HorrorManager.Instance.Player.Damage_Player(m_owner.Attack);
                            GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Horror_Straitjacket_Attack", false, 1f);
                        }
                    }
                }
            }
        }
    }

    public override void Exit_State()
    {
        Reset_Animation();
    }
}
