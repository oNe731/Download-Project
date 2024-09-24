using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUpperBody_Attack : BasicUpperBody_Base
{
    private float m_change = 1.5f;
    private float m_time = 0f;

    private Transform m_mouseTransform;

    public BasicUpperBody_Attack(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_mouseTransform = m_owner.transform.GetChild(3);
    }

    public override void Enter_State()
    {
        m_time = 0;
        Attack_Bullet();
    }

    public override void Update_State()
    {
        // 공격 모션이 끝나면 다시 공격
        m_time += Time.deltaTime;
        if (m_time >= m_change)
        {
            if (Change_Attack() == false) // 거리가 일정 이상일 시 재 추격, 아닐 시 재공격
                m_stateMachine.Change_State((int)BasicUpperBody.State.ST_CHASE);
        }
    }

    public override void Exit_State()
    {
    }

    private void Attack_Bullet() // 원거리 공격 : 입에서 투사체 액체 발사
    {
        GameObject gameObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/Monster/Etc/BasicUpperBody_Bullet");
        if (gameObject == null)
            return;
        Bullet bullet = gameObject.GetComponent<Bullet>();
        if (bullet == null)
            return;
        Vector3 targetPosition = GameManager.Ins.Horror.Player.transform.position;
        targetPosition.y += 1f;
        bullet.Initialize_Bullet(m_mouseTransform.position, targetPosition, m_owner.Attack, 5f);
    }
}
