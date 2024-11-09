using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Criminal : Person
    {
        private bool m_attack = false;

        public Criminal() : base()
        {
        }

        public override void Initialize(int groupIndex, int personIndex, Groups groups, Group group)
        {
            base.Initialize(groupIndex, personIndex, groups, group);
            m_personType = PERSONTYPE.PT_CRIMINAL;

            Combine_Round1();
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (m_animator == null || m_attack == false)
                return;

            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("AN_Person_Attack") == true)
            {
                float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (animTime >= 1.0f) // 애니메이션 종료
                {
                    // 손이 다 올라오면 HP가 깎인다.
                    m_attack = false;
                    GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>().Attacked_Player();

                    // 하얀색 화면으로 번쩍 효과 적용 (등장은 한번에 사라지는건 서서히 빠르게)
                    GameManager.Ins.UI.Start_FadeIn(0.3f, Color.white);

                    // 연기 이펙트 생성
                    GameObject smoke = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/1Stage/Effect/Effect_GunSmoke"); // 스케일 0.1
                    smoke.transform.position = new Vector3(transform.position.x - 0.518f, transform.position.y - 0.3729f, transform.position.z - 0.018f);
                }
            }
        }

        public void Change_Attack()
        {
            // 총 쏘는 애니메이션 재생
            m_attack = true;
            m_animator.SetBool("isAttack", true);

            // 자식들 애니메이션 변경
            foreach (Transform child in transform)
            {
                Animator childAnimator = child.GetComponent<Animator>();
                if (childAnimator != null) { childAnimator.SetBool("isAttack", true); }
            }

            // 1라운드 바닥에서 손이 올라온다.
            GameObject element = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/Common/PersonElement", gameObject.transform); // -0.4 -> 0
            element.GetComponent<Transform>().localPosition = new Vector3(0f, -0.4f, -0.01f); // 3
            element.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            element.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            element.GetComponent<MeshRenderer>().materials[0].SetTexture("_BaseMap", GameManager.Ins.Resource.Load<Texture2D>("1. Graphic/3D/2. Western/Character/Round1/Person/Person/Texture/Attack/1_PANNEL_Gun1"));
            StartCoroutine(element.AddComponent<CriminalGun>().Start_Up());
        }

        // 범인요소 내에서만 랜덤으로 조합.
        // 스카프는 이전 2개의 그룹과 중복이 아닐 시 허용.
        public void Combine_Round1()
        {
            string defaultAnimatorPath = "6. Animation/2. Western/Character/Round1/Person/Element/";
            ElementType1 elementStruct = new ElementType1();

            // 안대 생성
            elementStruct.blindfold = BLINDFOLD.BLINDFOLD_USE;
            Create_Element(new Vector3(0f, 0f, -0.005f), defaultAnimatorPath + "Blindfold/Blindfold/AC_Blindfold");

            // 초록색 눈 생성
            elementStruct.eye = EYE.EYE_GREEN;
            Create_Element(new Vector3(0f, 0f, -0.01f), defaultAnimatorPath + "Eye/Eye_Green/AC_Eye_Green");

            // 무늬 스카프 4종에서 랜덤 생성
            string name = "";
            while (true)
            {
                Get_RandomRound1_Scarf(ref elementStruct, ref name, 1, 5);

                // 이전 그룹이 존재하면 중복 검사
                if (m_groups.Check_ElementCriminal(m_groupIndex, elementStruct) == false)
                    break;
            }
            Create_Element(new Vector3(0f, 0f, -0.005f), defaultAnimatorPath + "Scarf/" + name);

            m_element = elementStruct;
        }
    }
}