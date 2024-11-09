using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Person : MonoBehaviour
    {
        public enum PERSONTYPE { PT_CITIZEN, PT_CRIMINAL, PT_END }
        public enum EYE { EYE_BLUE, EYE_GREEN, EYE_PINK, EYE_WHITE, EYE_END }
        public enum BLINDFOLD { BLINDFOLD_NON, BLINDFOLD_USE, BLINDFOLD_END }
        public enum SCARF { SCARF_PAINTING, SCARF_SOLID, SCARF_SPRITE, SCARF_WATERDROP, SCARF_WAVE, SCARF_END }

        public interface ElementType
        {
        }

        public struct ElementType1 : ElementType
        {
            public EYE       eye { get; set; }
            public BLINDFOLD blindfold { get; set; }
            public SCARF     scarf { get; set; }
        }

        protected PERSONTYPE   m_personType = PERSONTYPE.PT_END;
        protected ElementType  m_element;
        protected Groups       m_groups = null;
        protected Group        m_group  = null;
        protected MeshRenderer m_meshRenderer;
        protected Animator     m_animator;
        protected AudioSource  m_audioSource;

        protected int m_groupIndex;
        protected int m_personIndex;

        private Vector3 m_StartPosition;
        private float m_shakeTime   = 0.3f;
        private float m_shakeAmount = 1f; // 세기

        public PERSONTYPE PersonType => m_personType;
        public Person.ElementType Element => m_element;
        public int PersonIndex => m_personIndex;

        protected Person()
        {
        }

        public virtual void Initialize(int groupIndex, int personIndex, Groups groups, Group group)
        {
            m_groupIndex  = groupIndex;
            m_personIndex = personIndex;
            m_groups = groups;
            m_group = group;

            m_meshRenderer  = GetComponent<MeshRenderer>();
            m_animator      = GetComponent<Animator>();
            m_audioSource   = GetComponent<AudioSource>();
            m_StartPosition = transform.localPosition;

            // 해당하는 애니메이션 컨트롤러 추가
            m_animator.runtimeAnimatorController = GameManager.Ins.Resource.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Person/AC_Person");
        }

        protected void Create_Element(Vector3 position, string animatorPath)
        {
            GameObject element = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/Common/PersonElement", gameObject.transform);
            element.GetComponent<Transform>().localPosition = position;
            element.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            element.GetComponent<Transform>().localScale    = new Vector3(1f, 1f, 1f);
            element.GetComponent<Animator>().runtimeAnimatorController = GameManager.Ins.Resource.Load<RuntimeAnimatorController>(animatorPath);
        }

        protected void Get_RandomRound1_Eye(ref ElementType1 elementStruct, ref string name, int startIndex = 0, int maxIndex = 4)
        {
            int index = Random.Range(startIndex, maxIndex);
            switch (index)
            {
                case 0:
                    elementStruct.eye = EYE.EYE_PINK;
                    name = "Eye_Pink/AC_Eye_Pink";
                    break;
                case 1:
                    elementStruct.eye = EYE.EYE_BLUE;
                    name = "Eye_Blue/AC_Eye_Blue";
                    break;
                case 2:
                    elementStruct.eye = EYE.EYE_WHITE;
                    name = "Eye_White/AC_Eye_White";
                    break;

                case 3: // 범인 요소
                    elementStruct.eye = EYE.EYE_GREEN;
                    name = "Eye_Green/AC_Eye_Green";
                    break;
            }
        }

        protected void Get_RandomRound1_Blindfold(ref ElementType1 elementStruct, int startIndex = 0, int maxIndex = 2)
        {
            int index = Random.Range(startIndex, maxIndex);
            switch (index)
            {
                case 0:
                    elementStruct.blindfold = BLINDFOLD.BLINDFOLD_NON;
                    break;

                case 1: // 범인 요소
                    elementStruct.blindfold = BLINDFOLD.BLINDFOLD_USE;
                    break;
            }
        }

        protected void Get_RandomRound1_Scarf(ref ElementType1 elementStruct, ref string name, int startIndex = 0, int maxIndex = 5)
        {
            int index = Random.Range(startIndex, maxIndex);
            switch (index)
            {
                case 0:
                    elementStruct.scarf = SCARF.SCARF_SOLID;
                    name = "Scarf_Solid/AC_Scarf_Solid";
                    break;

                case 1: // 범인 요소
                    elementStruct.scarf = SCARF.SCARF_SPRITE;
                    name = "Scarf_Sprite/AC_Scarf_Sprite";
                    break;
                case 2: // 범인 요소
                    elementStruct.scarf = SCARF.SCARF_WAVE;
                    name = "Scarf_Wave/AC_Scarf_Wave";
                    break;
                case 3: // 범인 요소
                    elementStruct.scarf = SCARF.SCARF_WATERDROP;
                    name = "Scarf_Waterdrop/AC_Scarf_Waterdrop";
                    break;
                case 4: // 범인 요소
                    elementStruct.scarf = SCARF.SCARF_PAINTING;
                    name = "Scarf_Painting/AC_Scarf_Painting";
                    break;
            }
        }


        public void Start_Shake()
        {
            StartCoroutine(Shake(m_shakeAmount, m_shakeTime));
        }

        IEnumerator Shake(float ShakeAmount, float ShakeTime)
        {
            float timer = 0;
            while (timer <= ShakeTime)
            {
                Vector3 randomPoint = m_StartPosition + new Vector3(Random.insideUnitSphere.x, Random.insideUnitSphere.y, 0) * ShakeAmount;//m_StartPosition + Random.insideUnitSphere * ShakeAmount;
                transform.localPosition = Vector3.Lerp(m_StartPosition, randomPoint, Time.deltaTime);
                yield return null;

                timer += Time.deltaTime;
            }
            transform.localPosition = m_StartPosition;
            yield break;
        }

        public float Get_GroupZ()
        {
            return m_group.transform.position.z;
        }

        public void Stop_Animation()
        {
            if (m_animator.speed == 0f)
                return;

            m_animator.speed = 0f;
            foreach (Transform child in transform)
            {
                Animator childAnimator = child.GetComponent<Animator>();
                if (childAnimator != null) { childAnimator.speed = 0f; }
            }
        }
    }
}

