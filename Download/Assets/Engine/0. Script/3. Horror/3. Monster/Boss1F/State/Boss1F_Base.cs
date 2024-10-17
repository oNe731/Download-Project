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
        Boss1F.State preState = (Boss1F.State)m_owner.StateMachine.LasState;
        if (m_owner.Pattern == 1)
        {
            if (preState == Boss1F.State.ST_TENTACLE)
            {
                float randomValue = Random.Range(0f, 80f);
                if (randomValue <= 40) // 40%
                    m_stateMachine.Change_State((int)Boss1F.State.ST_TENTACLES);
                else // 40%
                    m_stateMachine.Change_State((int)Boss1F.State.ST_SPHERE);
            }
            else if (preState == Boss1F.State.ST_TENTACLES)
            {
                float randomValue = Random.Range(0f, 60f);
                if (randomValue <= 20) // 20%
                    m_stateMachine.Change_State((int)Boss1F.State.ST_TENTACLE);
                else // 40%
                    m_stateMachine.Change_State((int)Boss1F.State.ST_SPHERE);
            }
            else if(preState == Boss1F.State.ST_SPHERE)
            {
                float randomValue = Random.Range(0f, 60f);
                if (randomValue <= 20) // 20%
                    m_stateMachine.Change_State((int)Boss1F.State.ST_TENTACLE);
                else // 40%
                    m_stateMachine.Change_State((int)Boss1F.State.ST_TENTACLES);
            }
            else 
            {
                float randomValue = Random.Range(0f, 100f);
                if(randomValue <= 20) // 20%
                    m_stateMachine.Change_State((int)Boss1F.State.ST_TENTACLE);
                else if(randomValue <= 60) // 40%
                    m_stateMachine.Change_State((int)Boss1F.State.ST_TENTACLES);
                else // 40%
                    m_stateMachine.Change_State((int)Boss1F.State.ST_SPHERE);
            }
        }
        else if(m_owner.Pattern == 2)
        {
            if (preState == Boss1F.State.ST_TENTACLES)
            {
                m_stateMachine.Change_State((int)Boss1F.State.ST_SPHERE);
            }
            else if (preState == Boss1F.State.ST_SPHERE)
            {
                m_stateMachine.Change_State((int)Boss1F.State.ST_TENTACLES);
            }
            else
            {
                float randomValue = Random.Range(0f, 100f);
                if(randomValue <= 50)
                    m_stateMachine.Change_State((int)Boss1F.State.ST_TENTACLES);
                else
                    m_stateMachine.Change_State((int)Boss1F.State.ST_SPHERE);
            }
        }
    }

    protected bool Change_Recall()
    {
        float probability = 0f;
        if (m_owner.Pattern == 1)
            probability = 40f;
        else if (m_owner.Pattern == 2)
            probability = 65f;

        float randomValue = Random.Range(0f, 100f);
        if (randomValue <= probability)
        {
            m_stateMachine.Change_State((int)Boss1F.State.ST_RECALL);
            return true;
        }

        return false;
    }

    protected void Create_Tentacle(float symptomTime = 1.5f, float idleTime = 1.5f)
    {
        int randomIndex = Random.Range(1, 7); // 6가지
        GameObject gameObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/Monster/Etc/Tentacles/Patterns Variant_" + randomIndex.ToString());
        if (gameObject != null)
        {
            Vector3 playerPosition = GameManager.Ins.Horror.Player.transform.position;
            gameObject.transform.position = new Vector3(playerPosition.x, 0f, playerPosition.z);

            Tentacles tentacles = gameObject.GetComponent<Tentacles>();
            if (tentacles != null)
                tentacles.Start_Tentacles(symptomTime, idleTime);
        }
    }
}
