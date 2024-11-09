using UnityEngine;

namespace Western
{
    public class Citizen : Person
    {


        public Citizen() : base()
        {
        }

        public override void Initialize(int groupIndex, int personIndex, Groups groups, Group group)
        {
            base.Initialize(groupIndex, personIndex, groups, group);
            m_personType = PERSONTYPE.PT_CITIZEN;

            // 랜덤 조합 요소 배치
            Combine_Round1();
        }

        private void Start()
        {
        }

        private void Update()
        {
        }

        // 범인 요소와 시민 요소를 섞어서 조합 가능.
        // 시민 요소가 최소 1개 이상 들어가야 함.
        // 같은 그룹 내 완전한 중복 금지.
        public void Combine_Round1()
        {
            string defaultAnimatorPath = "6. Animation/2. Western/Character/Round1/Person/Element/";
            ElementType1 elementStruct = new ElementType1();

            int citizenCombineIndex = 0;
            string eyeName = "";
            string scarfName = "";

            while (true)
            {
                citizenCombineIndex = Random.Range(0, 3);
                switch(citizenCombineIndex)
                {
                    case 0: // 눈색을 시민요소 사용
                        Get_RandomRound1_Eye(ref elementStruct, ref eyeName, 0, 3);
                        Get_RandomRound1_Blindfold(ref elementStruct);
                        Get_RandomRound1_Scarf(ref elementStruct, ref scarfName);
                        break;

                    case 1: // 안대를 시민요소 사용
                        Get_RandomRound1_Eye(ref elementStruct, ref eyeName);
                        elementStruct.blindfold = BLINDFOLD.BLINDFOLD_NON;
                        Get_RandomRound1_Scarf(ref elementStruct, ref scarfName);
                        break;

                    case 2: // 스카프를 시민요소 사용
                        Get_RandomRound1_Eye(ref elementStruct, ref eyeName);
                        Get_RandomRound1_Blindfold(ref elementStruct);
                        elementStruct.scarf = SCARF.SCARF_SOLID;
                        scarfName = "Scarf_Solid/AC_Scarf_Solid";
                        break;
                }

                // 같은 그룹 내 중복 검사
                if (m_groups.Check_ElementCitizen(m_groupIndex, m_personIndex, elementStruct) == false)
                    break;
            }

            // 요소 생성
            Create_Element(new Vector3(0f, 0f, -0.01f), defaultAnimatorPath + "Eye/" + eyeName);
            if (elementStruct.blindfold == BLINDFOLD.BLINDFOLD_USE)
                Create_Element(new Vector3(0f, 0f, -0.005f), defaultAnimatorPath + "Blindfold/Blindfold/AC_Blindfold");
            Create_Element(new Vector3(0f, 0f, -0.005f), defaultAnimatorPath + "Scarf/" + scarfName);

            m_element = elementStruct;
        }
    }
}

