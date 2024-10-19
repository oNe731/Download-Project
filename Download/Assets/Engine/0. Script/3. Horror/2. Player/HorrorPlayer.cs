using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Horror
{
    public class HorrorPlayer : Character
    {
        public enum State { ST_IDLE, ST_WALK, ST_RUN, ST_ATTACK, ST_CHANGE, ST_END }

        [SerializeField] private Slider m_hpSlider;
        [SerializeField] private Slider m_staminaSlider;
        [SerializeField] private UIBlood m_blood;

        private float m_hp = 0f;
        private float m_hpMax = 50f;
        private float m_stamina = 0f;
        private float m_staminaMax = 10f;

        private bool m_isLock = false;
        private float m_moveSpeed = 270f;
        private float m_lerpSpeed = 5.0f;
        private float m_rotationSpeed = 100.0f;
        private Vector2 m_rotationLimit = new Vector2(-80f, 80f);
        private float m_xRotate = 0.0f;

        private StateMachine<HorrorPlayer> m_stateMachine;
        private WeaponManagement<HorrorPlayer> m_weaponManagement;
        private Note m_note;

        protected Rigidbody m_rigidbody;
        protected Animator m_animator;

        public float Hp => m_hp;
        public float HpMax => m_hpMax;
        public float Stamina => m_stamina;
        public float StaminaMax => m_staminaMax;

        public float MoveSpeed { get => m_moveSpeed; set => m_moveSpeed = value; }
        public float LerpSpeed => m_lerpSpeed;
        public float RotationSpeed => m_rotationSpeed;
        public Vector2 RotationLimit => m_rotationLimit;
        public float XRotate { get => m_xRotate; set => m_xRotate = value; }

        public StateMachine<HorrorPlayer> StateMachine => m_stateMachine;
        public WeaponManagement<HorrorPlayer> WeaponManagement => m_weaponManagement;
        public Note Note => m_note;

        public void Damage_Player(float damage)
        {
            m_hp -= damage;

            // Temp
            //if (m_hp < 1)
            //    m_hp = 1;

            if (m_hp <= 0)
            {
                m_hp = 0f;
                GameManager.Ins.Horror.Over_Game();
            }
            else
            {
                if (damage > 0)
                    m_blood.Active_Blood();
            }

            m_hpSlider.value = m_hp;
        }

        public bool Set_Stamina(float useValue)
        {
            bool recover = true;

            m_stamina += useValue;
            if (m_stamina <= 0)
            {
                m_stamina = 0f;
                recover = false;
            }
            else if (m_stamina >= m_staminaMax)
            {
                m_stamina = m_staminaMax;
                recover = false;
            }
 
            m_staminaSlider.value = m_stamina;
            return recover;
        }

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_animator = transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetComponent<Animator>();

            m_hp = m_hpMax;
            m_hpSlider.maxValue = m_hpMax;
            m_hpSlider.transform.parent.GetChild(0).GetComponent<Slider>().maxValue = m_hpSlider.maxValue;
            Damage_Player(0);

            m_stamina = m_staminaMax;
            m_staminaSlider.maxValue = m_staminaMax;
            m_staminaSlider.transform.parent.GetChild(0).GetComponent<Slider>().maxValue = m_staminaSlider.maxValue;

            Set_Stamina(0);

            // Weapon
            m_weaponManagement = new WeaponManagement<HorrorPlayer>(this);
            m_weaponManagement.Initialize_WeaponManagement();

            // State
            m_stateMachine = new StateMachine<HorrorPlayer>(gameObject);
            List<State<HorrorPlayer>> states = new List<State<HorrorPlayer>>();
            states.Add(new HorrorPlayer_Idle(m_stateMachine));   // 0
            states.Add(new HorrorPlayer_Walk(m_stateMachine));   // 1
            states.Add(new HorrorPlayer_Run(m_stateMachine));    // 2
            states.Add(new HorrorPlayer_Attack(m_stateMachine)); // 3
            states.Add(new HorrorPlayer_Change(m_stateMachine)); // 4

            m_stateMachine.Initialize_State(states, (int)State.ST_IDLE);
        }

        public void Update()
        {
            if (GameManager.Ins.IsGame == false)
                return;

            m_stateMachine.Update_State();
            m_weaponManagement.Update_Weapon();
        }

        public void Acquire_Note()
        {
            if (m_note != null)
                return;

            GameObject ui = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/UI_Note", GameObject.Find("Canvas").transform.Find("Panel_Down"));
            m_note = ui.GetComponent<Note>();
            m_note.Initialize_Note();
        }

        public void Set_Lock(bool isLock)
        {
            m_isLock = isLock;
            m_stateMachine.Lock = isLock;
            m_rigidbody.isKinematic = isLock;
        }

        public void Stop_Player(bool stop)
        {
            Set_Lock(stop);

            if (stop == false)
                m_animator.StopPlayback();
            else
                m_animator.StartPlayback();
        }
    }
}
