using UnityEngine;

public enum WeaponId { WP_MELEE, WP_FLASHLIGHT, WP_GUN, WP_END }
public interface WeaponInfo
{
}

public abstract class Weapon<T> : MonoBehaviour where T : class
{
    protected float m_damage = 0f;

    protected UIWeapon m_uIWeapon;

    protected WeaponManagement<T> m_stateManagement;
    protected WeaponId m_weaponID = WeaponId.WP_END;
    protected WeaponInfo m_weaponInfo;

    public WeaponId WeaponID { get => m_weaponID; }
    public WeaponInfo WeaponInfo { get => m_weaponInfo; }

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

    public virtual void OnDrawGizmos()
    {
    }
}
