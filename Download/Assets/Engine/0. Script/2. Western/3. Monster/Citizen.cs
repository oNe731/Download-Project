using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Citizen : Person
    {


        public Citizen() : base()
        {
        }

        public override void Initialize(int groupIndex, int personIndex, Groups groups, int roundIndex)
        {
            base.Initialize(groupIndex, personIndex, groups, roundIndex);
            m_personType = PERSONTYPE.PT_CITIZEN;

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
        }

        public void Combine_Round1()
        {
            // 범인 요소와 시민 요소를 섞어서 조합 가능 / 시민 요소가 최소 1개 이상 들어가야 함
            // 같은 그룹 내 완전한 중복 금지

            Person.ElementType1 elementStruct = new Person.ElementType1();

            int citizenIndex = 0;
            int index = 0;
            string name = "";
            GameObject eye = null;
            GameObject blindfold = null;
            GameObject scarf = null;

            while (true)
            {
                citizenIndex = Random.Range(0, 3);
                switch(citizenIndex)
                {
                    case 0:
                        // 눈색 시민요소 사용
                        index = Random.Range(0, 3);
                        switch (index)
                        {
                            case 0:
                                elementStruct.eye = Person.EYE.EYE_PINK;
                                name = "Eye_Pink/AC_Eye_Pink";
                                break;
                            case 1:
                                elementStruct.eye = Person.EYE.EYE_BLUE;
                                name = "Eye_Blue/AC_Eye_Blue";
                                break;
                            case 2:
                                elementStruct.eye = Person.EYE.EYE_WHITE;
                                name = "Eye_White/AC_Eye_White";
                                break;
                        }
                        eye = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
                        eye.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.002f); // 베이스 판넬보다 앞으로 배치
                        eye.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                        eye.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                        eye.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Eye/" + name);

                        // 안대 랜덤
                        index = Random.Range(0, 2);
                        switch(index)
                        {
                            case 0:
                                elementStruct.blindfold = Person.BLINDFOLD.BLINDFOLD_NON;
                                break;

                            case 1:
                                elementStruct.blindfold = Person.BLINDFOLD.BLINDFOLD_USE;
                                blindfold = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
                                blindfold.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.001f); // 베이스 판넬보다 앞으로 배치
                                blindfold.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                                blindfold.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                                blindfold.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Blindfold/Blindfold/AC_Blindfold");
                                break;
                        }

                        // 스카프 랜덤
                        index = Random.Range(0, 5);
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
                            case 4:
                                elementStruct.scarf = Person.SCARF.SCARF_SOLID;
                                name = "Scarf_Solid/AC_Scarf_Solid";
                                break;
                        }
                        scarf = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
                        scarf.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.001f); // 베이스 판넬보다 앞으로 배치
                        scarf.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                        scarf.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                        scarf.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Scarf/" + name);
                        break;

                    case 1:
                        // 안대 시민요소 사용
                        elementStruct.blindfold = Person.BLINDFOLD.BLINDFOLD_NON;

                        // 눈 랜덤
                        index = Random.Range(0, 4);
                        switch (index)
                        {
                            case 0:
                                elementStruct.eye = Person.EYE.EYE_BLUE;
                                name = "Eye_Blue/AC_Eye_Blue";
                                break;
                            case 1:
                                elementStruct.eye = Person.EYE.EYE_GREEN;
                                name = "Eye_Green/AC_Eye_Green";
                                break;
                            case 2:
                                elementStruct.eye = Person.EYE.EYE_PINK;
                                name = "Eye_Pink/AC_Eye_Pink";
                                break;
                            case 3:
                                elementStruct.eye = Person.EYE.EYE_WHITE;
                                name = "Eye_White/AC_Eye_White";
                                break;
                        }
                        scarf = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
                        scarf.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.001f); // 베이스 판넬보다 앞으로 배치
                        scarf.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                        scarf.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                        scarf.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Eye/" + name);

                        // 스카프 랜덤
                        index = Random.Range(0, 5);
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
                            case 4:
                                elementStruct.scarf = Person.SCARF.SCARF_SOLID;
                                name = "Scarf_Solid/AC_Scarf_Solid";
                                break;
                        }
                        scarf = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
                        scarf.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.001f); // 베이스 판넬보다 앞으로 배치
                        scarf.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                        scarf.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                        scarf.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Scarf/" + name);
                        break;

                    case 2:
                        // 스카프 시민요소 사용
                        elementStruct.scarf = Person.SCARF.SCARF_SOLID;
                        scarf = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
                        scarf.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.001f); // 베이스 판넬보다 앞으로 배치
                        scarf.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                        scarf.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                        scarf.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Scarf/Scarf_Solid/AC_Scarf_Solid");

                        // 눈 랜덤
                        index = Random.Range(0, 4);
                        switch (index)
                        {
                            case 0:
                                elementStruct.eye = Person.EYE.EYE_BLUE;
                                name = "Eye_Blue/AC_Eye_Blue";
                                break;
                            case 1:
                                elementStruct.eye = Person.EYE.EYE_GREEN;
                                name = "Eye_Green/AC_Eye_Green";
                                break;
                            case 2:
                                elementStruct.eye = Person.EYE.EYE_PINK;
                                name = "Eye_Pink/AC_Eye_Pink";
                                break;
                            case 3:
                                elementStruct.eye = Person.EYE.EYE_WHITE;
                                name = "Eye_White/AC_Eye_White";
                                break;
                        }
                        scarf = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
                        scarf.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.002f); // 베이스 판넬보다 앞으로 배치
                        scarf.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                        scarf.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                        scarf.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Eye/" + name);

                        // 안대 랜덤
                        index = Random.Range(0, 2);
                        switch (index)
                        {
                            case 0:
                                elementStruct.blindfold = Person.BLINDFOLD.BLINDFOLD_NON;
                                break;

                            case 1:
                                elementStruct.blindfold = Person.BLINDFOLD.BLINDFOLD_USE;
                                GameObject element = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
                                element.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.001f); // 베이스 판넬보다 앞으로 배치
                                element.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                                element.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                                element.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Blindfold/Blindfold/AC_Blindfold");
                                break;
                        }
                        break;
                }

                // 같은 그룹 내 중복 검사
                if (m_groups.Check_ElementCitizen(m_groupIndex, m_personIndex, elementStruct) == false)
                    break;
                else
                {
                    Destroy(eye);
                    Destroy(blindfold);
                    Destroy(scarf);
                }
            }

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

