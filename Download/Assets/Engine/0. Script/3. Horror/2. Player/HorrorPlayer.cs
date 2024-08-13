using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Horror
{
    public class HorrorPlayer : Character
    {
        public enum State { ST_IDLE, ST_WALK, ST_RUN, ST_ATTACK, ST_END }

        [SerializeField] private Slider m_hpSlider;
        [SerializeField] private Slider m_staminaSlider;
        [SerializeField] private UIBlood m_blood;

        private float m_hp = 0f;
        private float m_hpMax = 10f;
        private float m_stamina = 0f;
        private float m_staminaMax = 10f;
        private float m_xRotate = 0.0f;

        private StateMachine<HorrorPlayer> m_stateMachine;
        private WeaponManagement<HorrorPlayer> m_weaponManagement;
        private Note m_note;

        public float Hp => m_hp;
        public float HpMax => m_hpMax;
        public float Stamina => m_stamina;
        public float StaminaMax => m_staminaMax;
        public float XRotate { get => m_xRotate; set => m_xRotate = value; }

        public StateMachine<HorrorPlayer> StateMachine => m_stateMachine;
        public WeaponManagement<HorrorPlayer> WeaponManagement => m_weaponManagement;
        public Note Note => m_note;

        public void Damage_Player(float damage)
        {
            m_hp -= damage;

            // Temp
            if (m_hp < 1)
                m_hp = 1;

            if (m_hp <= 0)
            {
                m_hp = 0f;
                Debug.Log("플레이어 사망");
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
            m_hp = m_hpMax;
            m_hpSlider.maxValue = m_hpMax;
            Damage_Player(0);

            m_stamina = m_staminaMax;
            m_staminaSlider.maxValue = m_staminaMax;
            Set_Stamina(0);

            // State
            m_stateMachine = new StateMachine<HorrorPlayer>(gameObject);

            List<State<HorrorPlayer>> states = new List<State<HorrorPlayer>>();
            states.Add(new HorrorPlayer_Idle(m_stateMachine));   // 0
            states.Add(new HorrorPlayer_Walk(m_stateMachine));   // 1
            states.Add(new HorrorPlayer_Run(m_stateMachine));    // 2
            states.Add(new HorrorPlayer_Attack(m_stateMachine)); // 3

            m_stateMachine.Initialize_State(states, (int)State.ST_IDLE);

            // Weapon
            m_weaponManagement = new WeaponManagement<HorrorPlayer>(gameObject);
        }

        public void Update()
        {
            if (HorrorManager.Instance.IsGame == false)
                return;

            m_stateMachine.Update_State();
            m_weaponManagement.Update_Weapon();
        }

        public void Acquire_Note()
        {
            if (m_note != null)
                return;

            GameObject ui = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Note", GameObject.Find("Canvas").transform.GetChild(1));
            m_note = ui.GetComponent<Note>();
        }
    }
}
