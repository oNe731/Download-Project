using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Sphere : Boss1F_Base
{
    public enum STATE { ST_SYMPTOMS, ST_ATTACK, ST_END }

    private bool m_recall = false;
    private float m_time = 0f;
    private float m_symptomsTime = 0f;
    private float m_rotationSpeed = 0f;
    private float m_attackTime = 0f;
    private STATE m_state = STATE.ST_END;

    public Boss1F_Sphere(StateMachine<Monster> stateMachine) : base(stateMachine)
    {

    }

    public override void Enter_State()
    {
        if (m_recall == false)
        {
            m_recall = true;

            if (Change_Recall() == true)
                return;
        }

        Debug.Log("구체 생성");

        m_rotationSpeed = 2f; // 기본 스피드 : 회전하는 속도는 약간 빠르게 해서 주인공이 스테미나가 없거나, 미리 피하지 않으면 물질에 닿아 데미지를 입도록 유도.
        if (m_owner.Pattern == 1)
        {
            m_symptomsTime = 1.5f;
        }
        else if (m_owner.Pattern == 1)
        {
            m_symptomsTime = 1f;
            m_rotationSpeed *= 1.25f; // 25% 가속
        }

        // 전조증상
        m_state = STATE.ST_SYMPTOMS;
        m_time = 0f;
        // 입 열고 포효 애니메이션 재생
        // 카메라 쉐이킹
        // 동시에 천장에서 바닥으로 랜덤한 위치에 상자가 떨어짐. 갯수 2개
    }

    public override void Update_State()
    {
        switch(m_state)
        {
            case STATE.ST_SYMPTOMS:
                m_time += Time.deltaTime;
                if(m_time >= m_symptomsTime)
                {
                    m_time = 0f;
                    m_state = STATE.ST_ATTACK;
                    Attack_Sphere();
                }
                break;

            case STATE.ST_ATTACK:
                m_time += Time.deltaTime;
                if (m_time >= 5f)
                {
                    m_stateMachine.Change_State((int)Boss1F.State.ST_REST);
                }
                else
                {
                    m_attackTime += Time.deltaTime;
                    if(m_attackTime >= 1.5f)
                    {
                        m_attackTime = 0f;
                        Attack_Sphere();
                    }
                }
                break;
        }
    }

    public override void Exit_State()
    {
    }

    private void Attack_Sphere()
    {
        //입에서 녹색의 오염된 물질을 발사한다. 뱉으면서(부아악~빔처럼) 랜덤한 방향으로 회전한다.
        //회전 방향은 50 % 확률로 랜덤 / 50 % 확률로 주인공이 가까운 방향
    }
}
