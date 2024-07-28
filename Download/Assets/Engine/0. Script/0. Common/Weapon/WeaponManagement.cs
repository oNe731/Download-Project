using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManagement<T> where T : class
{
    private GameObject m_owner;
    private int m_curWeapon = -1;
    private int m_preWeapon = -1;
    private List<Weapon<T>> m_weapons = new List<Weapon<T>>();

    public GameObject Owner { get { return m_owner; } }
    public int CurWeapon { get { return m_curWeapon; } }
    public int PreWeapon { get { return m_preWeapon; } }
    public List<Weapon<T>> Weapons { get { return m_weapons; } }

    public WeaponManagement(GameObject owner)
    {
        m_owner = owner;
    }

    public void Add_Weapon(Weapon<T> weapons)
    {
        weapons.Initialize_Weapon(this);

        m_weapons.Add(weapons);
    }

    public void Update_Weapon()
    {
        if (m_curWeapon == -1)
            return;

        m_weapons[(int)m_curWeapon].Update_Weapon();
    }

    public void Change_Weapon(int weaponIndex)
    {
        if (m_curWeapon != -1)
            m_weapons[(int)m_curWeapon].Exit_Weapon();

        m_preWeapon = m_curWeapon;
        m_curWeapon = weaponIndex;

        m_weapons[(int)m_curWeapon].Enter_Weapon();
    }

    public void Next_Weapon(int value)
    {
        int index = m_curWeapon + value;
        if (index >= m_weapons.Count)
            index = 0;
        else if (index < 0)
            index = m_weapons.Count - 1;

        Change_Weapon(index);
    }

    public void Attack_Weapon()
    {
        if (m_curWeapon == -1 || m_curWeapon >= m_weapons.Count)
            return;

        m_weapons[(int)m_curWeapon].Attack_Weapon();
    }

    public int Get_WeaponIndex(int index)
    {
        for(int i = 0; i < m_weapons.Count; ++i)
        {
            if(m_weapons[i].WeaponIndex == index)
            {
                return i;
            }
        }

        return -1;
    }

    public void OnDrawGizmos()
    {
        m_weapons[(int)m_curWeapon].OnDrawGizmos();
    }
}