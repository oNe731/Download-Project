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
                if (animTime >= 1.0f) // �ִϸ��̼� ����
                {
                    // ���� �� �ö���� HP�� ���δ�.
                    m_attack = false;
                    GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>().Attacked_Player();

                    // �Ͼ�� ȭ������ ��½ ȿ�� ���� (������ �ѹ��� ������°� ������ ������)
                    GameManager.Ins.UI.Start_FadeIn(0.3f, Color.white);

                    // ���� ����Ʈ ����
                    GameObject smoke = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/1Stage/Effect/Effect_GunSmoke"); // ������ 0.1
                    smoke.transform.position = new Vector3(transform.position.x - 0.518f, transform.position.y - 0.3729f, transform.position.z - 0.018f);
                }
            }
        }

        public void Change_Attack()
        {
            // �� ��� �ִϸ��̼� ���
            m_attack = true;
            m_animator.SetBool("isAttack", true);

            // �ڽĵ� �ִϸ��̼� ����
            foreach (Transform child in transform)
            {
                Animator childAnimator = child.GetComponent<Animator>();
                if (childAnimator != null) { childAnimator.SetBool("isAttack", true); }
            }

            // 1���� �ٴڿ��� ���� �ö�´�.
            GameObject element = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/Common/PersonElement", gameObject.transform); // -0.4 -> 0
            element.GetComponent<Transform>().localPosition = new Vector3(0f, -0.4f, -0.01f); // 3
            element.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            element.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            element.GetComponent<MeshRenderer>().materials[0].SetTexture("_BaseMap", GameManager.Ins.Resource.Load<Texture2D>("1. Graphic/3D/2. Western/Character/Round1/Person/Person/Texture/Attack/1_PANNEL_Gun1"));
            StartCoroutine(element.AddComponent<CriminalGun>().Start_Up());
        }

        // ���ο�� �������� �������� ����.
        // ��ī���� ���� 2���� �׷�� �ߺ��� �ƴ� �� ���.
        public void Combine_Round1()
        {
            string defaultAnimatorPath = "6. Animation/2. Western/Character/Round1/Person/Element/";
            ElementType1 elementStruct = new ElementType1();

            // �ȴ� ����
            elementStruct.blindfold = BLINDFOLD.BLINDFOLD_USE;
            Create_Element(new Vector3(0f, 0f, -0.005f), defaultAnimatorPath + "Blindfold/Blindfold/AC_Blindfold");

            // �ʷϻ� �� ����
            elementStruct.eye = EYE.EYE_GREEN;
            Create_Element(new Vector3(0f, 0f, -0.01f), defaultAnimatorPath + "Eye/Eye_Green/AC_Eye_Green");

            // ���� ��ī�� 4������ ���� ����
            string name = "";
            while (true)
            {
                Get_RandomRound1_Scarf(ref elementStruct, ref name, 1, 5);

                // ���� �׷��� �����ϸ� �ߺ� �˻�
                if (m_groups.Check_ElementCriminal(m_groupIndex, elementStruct) == false)
                    break;
            }
            Create_Element(new Vector3(0f, 0f, -0.005f), defaultAnimatorPath + "Scarf/" + name);

            m_element = elementStruct;
        }
    }
}