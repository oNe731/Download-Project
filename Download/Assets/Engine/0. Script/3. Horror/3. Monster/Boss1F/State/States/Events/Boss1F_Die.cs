using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Die : Boss1F_Base
{
    private bool m_dissolve = false;
    private bool m_destroy = false;

    private Material[] m_materials = new Material[2];

    public Boss1F_Die(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_animator.speed = 1f;
        m_animator.SetLayerWeight(1, 0f);
        m_animator.SetLayerWeight(2, 0f);
        m_animator.SetBool("IsDie", true);

        GameManager.Ins.Resource.Destroy(m_owner.transform.GetChild(3).gameObject); // 조명 삭제
    }

    public override void Update_State()
    {
        if (m_animator.IsInTransition(0) == true) 
            return;

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("IsDie") == true)
        {
            m_animator.SetBool("IsDie", false);

            float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animTime >= 1f) // 애니메이션 종료 1.0f
                Update_Dissolve();
        }
    }

    public override void Exit_State()
    {
    }

    private void Update_Dissolve()
    {
        if(m_dissolve == false)
        {
            m_dissolve = true;

            // 메테리얼 교체
            List<Material> materials1 = new List<Material>();
            materials1.Add(GameManager.Ins.Resource.Load<Material>("1. Graphic/3D/3. Horror/Monster/1FBoss/Meterial/D_Boss_Main"));
            SkinnedMeshRenderer SkinnedMeshRenderer1 = m_owner.gameObject.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer1.SetMaterials(materials1);
            m_materials[0] = SkinnedMeshRenderer1.materials[0];
            m_materials[0].SetFloat("_Split_Value", 1.1f);

            List<Material> materials2 = new List<Material>();
            materials2.Add(GameManager.Ins.Resource.Load<Material>("1. Graphic/3D/3. Horror/Monster/1FBoss/Meterial/D_Boss_Sub"));
            SkinnedMeshRenderer SkinnedMeshRenderer2 = m_owner.gameObject.transform.GetChild(0).GetChild(2).GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer2.SetMaterials(materials2);
            m_materials[1] = SkinnedMeshRenderer2.materials[0];
            m_materials[1].SetFloat("_Split_Value", 1.1f);
        }
        else
        {
            if (m_destroy == true)
                return;

            for (int i = 0; i < m_materials.Length; ++i)
            {
                float value = m_materials[i].GetFloat("_Split_Value");
                value -= Time.deltaTime * 1.5f; // 속도 1.5
                if (value <= -1)
                {
                    value = -1f;
                    Delete_Monster();
                }

                m_materials[i].SetFloat("_Split_Value", value);
            }
        }        
    }

    private void Delete_Monster()
    {
        m_destroy = true;

        GameObject item = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/Object/Item/Research_1FKey");
        if (item != null)
            item.transform.position = new Vector3(12.314f, 0.5f, 7.82f);

        GameManager.Ins.Resource.Destroy(m_owner.gameObject);
    }
}
