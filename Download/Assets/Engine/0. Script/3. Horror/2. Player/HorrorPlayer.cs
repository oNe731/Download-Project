using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Horror
{
    public class HorrorPlayer : MonoBehaviour
    {
        public enum State { ST_IDLE, ST_WALK, ST_RUN, ST_ATTACK, ST_END }

        [SerializeField] private Slider m_hpSlider;
        [SerializeField] private Slider m_staminaSlider;

        private float m_hp = 0f;
        private float m_hpMax = 10f;
        private float m_stamina = 0f;
        private float m_staminaMax = 10f;

        private StateMachine<HorrorPlayer> m_stateMachine;
        private WeaponManagement<HorrorPlayer> m_weaponManagement;

        public float Hp => m_hp;
        public float HpMax => m_hpMax;
        public float Stamina => m_stamina;
        public float StaminaMax => m_staminaMax;

        public StateMachine<HorrorPlayer> StateMachine => m_stateMachine;
        public WeaponManagement<HorrorPlayer> WeaponManagement => m_weaponManagement;

        public void Damage_Player(float damage)
        {
            m_hp -= damage;
            if(m_hp <= 0)
            {
                m_hp = 0f;
                Debug.Log("플레이어 사망");
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

        private void Awake()
        {
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
    }
}
