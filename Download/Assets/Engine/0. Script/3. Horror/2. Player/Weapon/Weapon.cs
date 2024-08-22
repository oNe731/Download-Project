using UnityEngine;

public abstract class Weapon<T> : MonoBehaviour where T : class
{
    protected float m_damage = 0f;

    protected UIWeapon m_uIWeapon;

    protected WeaponManagement<T> m_stateManagement;

    protected NoteItem m_itemInfo = new NoteItem();

    public NoteItem ItemInfo { get => m_itemInfo; }

    public abstract void Attack_Weapon();

    public virtual void Initialize_Weapon(WeaponManagement<T> weaponManagement, UIWeapon uIWeapon)
    {
        m_stateManagement = weaponManagement;
        m_uIWeapon = uIWeapon;

        gameObject.SetActive(false);
    }

    public virtual void Enter_Weapon()
    {
        gameObject.SetActive(true);
    }

    public virtual void Update_Weapon()
    {
    }

    public virtual void Exit_Weapon()   
    {
        gameObject.SetActive(false);
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
