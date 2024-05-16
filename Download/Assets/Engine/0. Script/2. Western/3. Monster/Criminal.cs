using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Criminal : Person
{
    private bool m_attack = false;

    public Criminal() : base()
    {
    }

    public void Initialize(int roundIndex)
    {
        base.Initialize();
        m_personType = PERSONTYPE.PT_CRIMINAL;
        //m_meshRenderer.materials[0].SetTexture("_BaseMap", Resources.Load<Texture2D>("1. Graphic/3D/2. Western/Character/Texture/Person_01"));
        string path = "";
        switch(roundIndex)
        {
            case (int)WesternManager.LEVELSTATE.LS_PlayLv1:
                path = "6. Animation/2. Western/Character/Round1/Criminal/AC_Criminal";
                break;
            case (int)WesternManager.LEVELSTATE.LS_PlayLv2:
                path = "6. Animation/2. Western/Character/Round2/Criminal/AC_Criminal";
                break;
            case (int)WesternManager.LEVELSTATE.LS_PlayLv3:
                path = "6. Animation/2. Western/Character/Round3/Criminal/AC_Criminal";
                break;
        }
        m_animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(path);

        gameObject.SetActive(false);
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (m_attack) 
            return;

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("AN_Criminal_Attack") == true)
        {
            float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animTime >= 1.0f) // 애니메이션 종료
            {
                m_attack = true;
                WesternManager.Instance.LevelController.Get_CurrentLevel<Western_Play>().Attacked_Player();
            }
        }
    }

    public void Change_Attack()
    {
        // 총 쏘는 애니메이션 재생
        m_attack = false;
        m_animator.SetBool("isAttack", true);
    }
}
