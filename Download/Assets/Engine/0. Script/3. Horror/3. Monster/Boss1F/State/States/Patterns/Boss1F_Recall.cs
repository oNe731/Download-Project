using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Recall : Boss1F_Base
{
    private bool m_create = false;
    private float m_time = 0f;

    public Boss1F_Recall(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_create = false;
        m_time = 0f;

        m_animator.speed = 1f;
        m_animator.SetLayerWeight(1, 0f);
        m_animator.SetLayerWeight(2, 0f);
        m_animator.SetBool("IsWorm", true);

        Debug.Log("벌레 생성 상태");
    }

    public override void Update_State()
    {
        if (m_animator.IsInTransition(0) == true)
            return;

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("IsWorm") == true)
        {
            m_animator.SetBool("IsWorm", false);

            float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if(m_create == false && animTime >= 0.5f)
            {
                Create_Monster();
            }
            else if (animTime >= 1f) // 애니메이션 종료
            {
                m_time += Time.deltaTime;
                if(m_time >= 1f)
                {
                    m_stateMachine.Change_State((int)Boss1F.State.ST_REST); // 이전 상태로 돌아가 해당 공격 실행
                }
            }
        }
    }

    public override void Exit_State()
    {
        m_animator.SetBool("IsWorm", false);
    }

    private void Create_Monster()
    {
        if (m_create == true)
            return;
        m_create = true;

        int createCount;

        float randomValue = Random.Range(0f, 100f);
        if (randomValue <= 35)
            createCount = 2;
        else //if (randomValue <= 65)
            createCount = 1;

        for(int i = 0; i < createCount; ++i)
        {
            GameObject gameObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/Monster/Bug");
            if(gameObject != null)
            {
                // 보스가 매달린 쪽 천장에서 소환
                gameObject.transform.position = new Vector3(m_owner.transform.position.x + Random.Range(-0.5f, 0.5f), 2.45f, m_owner.transform.position.z + Random.Range(-0.5f, 0.5f));

                Bug bug = gameObject.GetComponent<Bug>();
                if(bug != null)
                {
                    bug.Initialize_Bug();

                    bug.StateMachine.Change_State((int)Bug.State.ST_FLY); // 처음부터 나는 상태
                    Bug_Fly bug_Fly = (Bug_Fly)bug.StateMachine.Get_CurrState();
                    if (bug_Fly != null)
                        bug_Fly.AtaackTime = 1.5f; // 1.5초 뒤부터 공격
                }
            }
        }
    }
}
