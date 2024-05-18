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

        public override void Initialize(int groupIndex, int personIndex, Groups groups, int roundIndex)
        {
            base.Initialize(groupIndex, personIndex, groups, roundIndex);
            m_personType = PERSONTYPE.PT_CRIMINAL;

            // 랜덤 조합 요소 배치
            if (roundIndex == (int)WesternManager.LEVELSTATE.LS_PlayLv3)
                Combine_Round3();
            else if (roundIndex == (int)WesternManager.LEVELSTATE.LS_PlayLv2)
                Combine_Round2();
            else
                Combine_Round1();
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (m_attack || m_animator == null)
                return;

            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("AN_Person_Attack") == true)
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

        public void Combine_Round1()
        {
            // 범인요소 내에서만 랜덤으로 조합.
            // 스카프만 이전 2개의 그룹과 중복이 아닐 시 허용.

            Person.ElementType1 elementStruct = new Person.ElementType1();

            // 안대 생성
            elementStruct.blindfold = Person.BLINDFOLD.BLINDFOLD_USE;
            GameObject element = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
            element.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.001f); // 베이스 판넬보다 앞으로 배치
            element.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            element.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            element.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Blindfold/Blindfold/AC_Blindfold");

            // 초록색 눈 생성
            elementStruct.eye = Person.EYE.EYE_GREEN;
            GameObject eye = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
            eye.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.002f); // 안대 판넬보다 앞으로 배치
            eye.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            eye.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            eye.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Eye/Eye_Green/AC_Eye_Green");

            // 스카프 4종에서 랜덤 생성
            string name = "";
            while (true)
            {
                int index = Random.Range(0, 4);
                switch (index)
                {
                    case 0:
                        elementStruct.scarf = Person.SCARF.SCARF_SPRITE;
                        name = "Scarf_Sprite/AC_Scarf_Sprite";
                        break;
                    case 1:
                        elementStruct.scarf = Person.SCARF.SCARF_WAVE;
                        name = "Scarf_Wave/AC_Scarf_Wave";
                        break;
                    case 2:
                        elementStruct.scarf = Person.SCARF.SCARF_WATERDROP;
                        name = "Scarf_Waterdrop/AC_Scarf_Waterdrop";
                        break;
                    case 3:
                        elementStruct.scarf = Person.SCARF.SCARF_PAINTING;
                        name = "Scarf_Painting/AC_Scarf_Painting";
                        break;
                }

                // 이전 그룹이 존재하면 중복 검사 후 중복이 아니라면 탈출
                if (m_groups.Check_ElementCriminal(m_groupIndex, elementStruct) == false)
                    break;
            }

            GameObject scarf = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
            scarf.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.001f); // 베이스 판넬보다 앞으로 배치
            scarf.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            scarf.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            scarf.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Scarf/" + name);

            m_element = elementStruct;
        }

        public void Combine_Round2()
        {

        }

        public void Combine_Round3()
        {

        }
    }
}

