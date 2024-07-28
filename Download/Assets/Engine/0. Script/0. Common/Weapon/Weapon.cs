using UnityEngine;

public abstract class Weapon<T> : MonoBehaviour where T : class
{
    protected WeaponManagement<T> m_stateManagement;
    protected int m_weaponIndex = -1;

    public int WeaponIndex { get => m_weaponIndex; }

    public abstract void Attack_Weapon();

    public virtual void Initialize_Weapon(WeaponManagement<T> weaponManagement)
    {
        m_stateManagement = weaponManagement;

        gameObject.SetActive(false);
    }

    public virtual void Enter_Weapon()
    {
        gameObject.SetActive(true);

        // UI 업데이트
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
