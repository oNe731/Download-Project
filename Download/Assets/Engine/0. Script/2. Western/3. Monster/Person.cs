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

        protected int m_roundIndex;
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

        public virtual void Initialize(int groupIndex, int personIndex, Groups groups, Group group, int roundIndex)
        {
            m_roundIndex  = roundIndex;
            m_groupIndex  = groupIndex;
            m_personIndex = personIndex;
            m_groups = groups;
            m_group = group;

            m_meshRenderer  = GetComponent<MeshRenderer>();
            m_animator      = GetComponent<Animator>();
            m_audioSource   = GetComponent<AudioSource>();
            m_StartPosition = transform.localPosition;

            // 해당하는 애니메이션 컨트롤러 추가
            int level; //m_meshRenderer.materials[0].SetTexture("_BaseMap", Resources.Load<Texture2D>(""));
            if(roundIndex == (int)WesternManager.LEVELSTATE.LS_PlayLv3)
                level = 3;
            else if(roundIndex == (int)WesternManager.LEVELSTATE.LS_PlayLv2)
                level = 2;
            else
                level = 1;
            m_animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round" + level.ToString() + "/Person/Person/AC_Person");

            //gameObject.SetActive(false);
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
    }
}

