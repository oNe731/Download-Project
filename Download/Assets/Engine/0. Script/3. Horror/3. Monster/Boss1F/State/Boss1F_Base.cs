using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Base : State<Monster>
{
    protected Boss1F m_owner = null;

    protected Animator m_animator = null;
    protected AudioSource m_audioSource = null;

    public Boss1F_Base(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_owner = m_stateMachine.Owner.GetComponent<Boss1F>();

        m_animator = m_stateMachine.Owner.transform.GetChild(0).GetComponent<Animator>();;
        m_audioSource = m_stateMachine.Owner.GetComponent<AudioSource>();
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

    protected void Look_Player()
    {
        Vector3 direction = GameManager.Ins.Horror.Player.transform.position - m_owner.transform.position;
        direction.y = 0f;
        direction   = direction.normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            float angleDifference = Quaternion.Angle(m_owner.transform.rotation, targetRotation);

            if (angleDifference > 10f)
            {
                // 현재 회전의 forward 벡터와 목표 방향의 각도를 계산하여 좌우 판별
                Vector3 cross = Vector3.Cross(m_owner.transform.forward, direction);
                float dot = Vector3.Dot(cross, Vector3.up);

                AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0); // 0은 기본 레이어
                if (dot > 0)
                {
                    Debug.Log("오른쪽으로 회전");
                    if (stateInfo.IsName("IsIdle") == true || stateInfo.IsName("IsRight") == true)
                    {
                        m_animator.SetBool("IsRight", false);
                        m_animator.SetBool("IsLeft", true);
                    }
                }
                else if (dot < 0)
                {
                    Debug.Log("왼쪽으로 회전");
                    if (stateInfo.IsName("IsIdle") == true || stateInfo.IsName("IsLeft") == true)
                    {
                        m_animator.SetBool("IsLeft", false);
                        m_animator.SetBool("IsRight", true);
                    }
                }

                m_owner.transform.rotation = Quaternion.Slerp(m_owner.transform.rotation, targetRotation, Time.deltaTime * 5f);
                return;
            }
        }
    }
}
