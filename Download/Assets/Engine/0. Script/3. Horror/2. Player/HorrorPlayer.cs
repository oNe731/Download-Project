using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer : MonoBehaviour
    {
        public enum State { ST_IDLE, ST_WALK, ST_RUN, ST_ATTACK, ST_END }
        public enum WeaponId { WP_MELEE, WP_FLASHLIGHT, WP_GUN, WP_END }

        private StateMachine<HorrorPlayer> m_stateMachine;
        private WeaponManagement<HorrorPlayer> m_weaponManagement;

        public StateMachine<HorrorPlayer> StateMachine => m_stateMachine;
        public WeaponManagement<HorrorPlayer> WeaponManagement => m_weaponManagement;

        private void Awake()
        {
        }

        private void Start()
        {
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
            m_weaponManagement.Add_Weapon(GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Character/Weapon/Melee", transform).GetComponent<Weapon<HorrorPlayer>>());
            m_weaponManagement.Add_Weapon(GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Character/Weapon/Gun", transform).GetComponent<Weapon<HorrorPlayer>>());
            m_weaponManagement.Add_Weapon(GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Character/Weapon/Flashlight", transform).GetComponent<Weapon<HorrorPlayer>>());
        }

        public void Update()
        {
            m_stateMachine.Update_State();
            m_weaponManagement.Update_Weapon();
        }
    }
}
