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
                // 좌우 판별
                Vector3 cross = Vector3.Cross(m_owner.transform.forward, direction);
                float dot = Vector3.Dot(cross, Vector3.up);

                AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(1); // 2레이어
                if (dot > 0)
                {
                    if (stateInfo.IsName("IsIdle") == true || stateInfo.IsName("IsRight") == true)
                    {
                        m_animator.speed = 1f;
                        m_animator.SetBool("IsRight", false);
                        m_animator.SetBool("IsLeft", true);
                    }
                }
                else if (dot < 0)
                {
                    if (stateInfo.IsName("IsIdle") == true || stateInfo.IsName("IsLeft") == true)
                    {
                        m_animator.speed = 1f;
                        m_animator.SetBool("IsLeft", false);
                        m_animator.SetBool("IsRight", true);
                    }
                }
                m_owner.transform.rotation = Quaternion.Slerp(m_owner.transform.rotation, targetRotation, Time.deltaTime * m_owner.RotationSpeed);
            }
            else
            {
                m_animator.speed = 0f; // 애니메이션 정지
            }
        }
        else
        {
            m_animator.speed = 0f; // 애니메이션 정지
        }
    }

    protected bool Change_Weakness()
    {
        if(m_owner.CumulativeDamage >= m_owner.CumulativeMaxDamage)
        {
            m_owner.StateMachine.Change_State((int)Boss1F.State.ST_WEAKNESS);
            return true;
        }

        return false;
    }

    protected void Change_Patterns()
    {
        // Test
        m_stateMachine.Change_State((int)Boss1F.State.ST_IDLE);

        Boss1F.State preState = (Boss1F.State)m_owner.StateMachine.PreState;
        if (m_owner.Pattern == 1)
        {
            if (preState == Boss1F.State.ST_TENTACLE)
            {
                // 40%
                // 촉수 N번 생성

                // 40%
                // 오염 물질 발사
            }
            else if (preState == Boss1F.State.ST_TENTACLES)
            {
                // 20%
                // 촉수 1개 패턴

                // 40%
                // 오염 물질 발사
            }
            else if(preState == Boss1F.State.ST_SPHERE)
            {
                // 20%
                // 촉수 1개 패턴

                // 40%
                // 촉수 N번 생성
            }
            else 
            {
                // 20%
                // 촉수 1개 패턴

                // 40%
                // 촉수 N번 생성

                // 40%
                // 오염 물질 발사
            }
        }
        else if(m_owner.Pattern == 2)
        {
            if (preState == Boss1F.State.ST_TENTACLES)
            {
                // 오염 물질 발사
            }
            else if (preState == Boss1F.State.ST_SPHERE)
            {
                // 촉수 N번 샹송
            }
            else
            {
                // 촉수 N번 샹송

                // 오염 물질 발사
            }
        }
    }
}
