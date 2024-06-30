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

        public override void Initialize(int groupIndex, int personIndex, Groups groups, Group group, int roundIndex)
        {
            base.Initialize(groupIndex, personIndex, groups, group, roundIndex);
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
            if (m_animator == null || m_attack == false)
                return;

            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("AN_Person_Attack") == true)
            {
                float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (animTime >= 1.0f) // 애니메이션 종료
                {
                    // 손이 다 올라오면 HP가 깎인다.
                    m_attack = false;
                    WesternManager.Instance.LevelController.Get_CurrentLevel<Western_Play>().Attacked_Player();
                }
            }
        }

        public void Change_Attack()
        {
            //m_audioSource.Stop();
            //m_audioSource.clip = Resources.Load<AudioClip>("2. Sound/2. Western/Effect/UI/범인 잡았을 때");
            //m_audioSource.Play();

            // 총 쏘는 애니메이션 재생
            m_attack = true;
            m_animator.SetBool("isAttack", true);

            // 자식들 애니메이션 변경
            foreach (Transform child in transform)
            {
                Animator childAnimator = child.GetComponent<Animator>();
                if (childAnimator != null) { childAnimator.SetBool("isAttack", true); }
            }

            if (m_roundIndex == (int)WesternManager.LEVELSTATE.LS_PlayLv1 || m_roundIndex == (int)WesternManager.LEVELSTATE.LS_PlayLv2 || m_roundIndex == (int)WesternManager.LEVELSTATE.LS_PlayLv3)
            {
                // 1라운드 바닥에서 손이 올라온다.
                GameObject element = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform); // -0.4 -> 0
                element.GetComponent<Transform>().localPosition = new Vector3(0f, -0.4f, -0.01f); // 3
                element.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                element.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                element.GetComponent<MeshRenderer>().materials[0].SetTexture("_BaseMap", Resources.Load<Texture2D>("1. Graphic/3D/2. Western/Character/Round1/Person/Person/Texture/Attack/1_PANNEL_Gun1"));
                StartCoroutine(element.AddComponent<CriminalGun>().Start_Up());
            }
        }

        public void Combine_Round1()
        {
            // 범인요소 내에서만 랜덤으로 조합.
            // 스카프만 이전 2개의 그룹과 중복이 아닐 시 허용.

            Person.ElementType1 elementStruct = new Person.ElementType1();

            // 안대 생성
            elementStruct.blindfold = Person.BLINDFOLD.BLINDFOLD_USE;
            GameObject element = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
            element.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.005f); // 2
            element.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            element.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            element.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Blindfold/Blindfold/AC_Blindfold");

            // 초록색 눈 생성
            elementStruct.eye = Person.EYE.EYE_GREEN;
            GameObject eye = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
            eye.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.01f); // 3
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
            scarf.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.005f); // 2
            scarf.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            scarf.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            scarf.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Scarf/" + name);

            m_element = elementStruct;
        }

        public void Combine_Round2()
        {
            Combine_Round1();
        }

        public void Combine_Round3()
        {

        }
    }
}

