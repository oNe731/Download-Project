using UnityEngine;

namespace Horror
{
    public abstract class Weapon<T> : MonoBehaviour where T : class
    {
        protected float m_damage = 0f;

        protected UIWeapon m_uIWeapon;

        protected WeaponManagement<T> m_stateManagement;

        protected NoteItem m_itemInfo = new NoteItem();

        protected Animator m_animator;
        protected AudioSource m_audioSource;

        public NoteItem ItemInfo { get => m_itemInfo; }

        public abstract bool Attack_Weapon();

        public virtual void Initialize_Weapon(WeaponManagement<T> weaponManagement, NoteItem noteItem, UIWeapon uIWeapon)
        {
            m_stateManagement = weaponManagement;
            m_itemInfo = noteItem;
            m_uIWeapon = uIWeapon;

            m_animator = GameManager.Ins.Horror.Player.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetComponent<Animator>();
            gameObject.SetActive(false);

            m_audioSource = gameObject.GetComponent<AudioSource>();
        }

        public virtual void Enter_Weapon()
        {
            GameManager.Ins.Horror.Player.StateMachine.Change_State((int)HorrorPlayer.State.ST_CHANGE);
            //gameObject.SetActive(true);
        }

        public virtual void Update_Weapon()
        {
        }

        public virtual void Exit_Weapon()
        {
            //gameObject.SetActive(false);
        }

        public void Update_WeaponUI()
        {
            if (m_uIWeapon == null)
                return;

            m_uIWeapon.Update_Info(m_itemInfo.m_itemType, m_itemInfo.m_itemInfo);
        }

        public virtual void OnDrawGizmos()
        {
        }
    }
}

