using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Weakness : Boss1F_Base // 내려가기, 올라가기
{
    private bool m_isUsed = false;
    private bool m_isDown = false;

    private bool m_isWeakness = false;
    private float m_stateTime = 0f;

    public Boss1F_Weakness(StateMachine<Monster> stateMachine) : base(stateMachine)
    {

    }

    public override void Enter_State()
    {
        Debug.Log("약점상태");

        if (m_isUsed == false)
        {
            m_isUsed = true;

            m_isWeakness = false;
            m_stateTime = 0f;

            m_isDown = true;
            m_animator.speed = 1f;
            m_animator.SetLayerWeight(1, 0f);
            m_animator.SetLayerWeight(2, 0f);
            m_animator.SetBool("IsWeakDown", true);

            m_owner.IsInvincible = true;

            Debug.Log("약점 내려가기 상태");
        }
        else
        {
            Debug.Log("약점 유지 상태");
        }
    }

    public override void Update_State()
    {
        if (m_isDown == true)
        {
            if (m_animator.IsInTransition(0) == true)
                return;

            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("IsWeakDown") == true)
            {
                m_animator.SetBool("IsWeakDown", false);

                float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (animTime >= 1f) // 애니메이션 종료
                {
                    m_isDown = false;
                    m_isWeakness = true;

                    Set_Weakness(true); // 콜라이더 활성화
                    m_owner.IsInvincible = false;

                    Debug.Log("약점 유지 상태");
                }
            }
        }
        else if (m_isWeakness == true)
        {
            m_stateTime += Time.deltaTime;
            if (m_stateTime > 5f) // 5초 유지
            {
                m_isWeakness = false;

                Set_Weakness(false); // 콜라이더 비활성화
                m_owner.IsInvincible = true;

                m_animator.speed = 1f;
                m_animator.SetLayerWeight(1, 0f);
                m_animator.SetLayerWeight(2, 0f);
                m_animator.SetBool("IsWeakUp", true);

                Debug.Log("약점 올라가기 상태");
            }
        }
        else
        {
            if (m_animator.IsInTransition(0) == true)
                return;

            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("IsWeakUp") == true)
            {
                m_animator.SetBool("IsWeakUp", false);

                float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (animTime >= 1f) // 애니메이션 종료
                {
                    m_isUsed = false;

                    m_owner.CumulativeDamage = 0;
                    m_owner.IsInvincible = false;

                    Change_Patterns();

                    Debug.Log("약점 완료 상태");
                }
            }
        }
    }

    public override void Exit_State()
    {
        m_animator.SetBool("IsWeakDown", false);
        m_animator.SetBool("IsWeakUp", false);
    }

    private void Set_Weakness(bool active)
    {
        //ㄴ이때 눈을 제외한 부분을 공격하면 기본 데미지가 들어가고, 눈을 공격하면 데미지가 2.5배로 들어간다.
        if(active == true)
        {

        }
        else
        {

        }
    }
}
