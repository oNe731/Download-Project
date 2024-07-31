using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManagement<T> where T : class
{
    private GameObject m_owner;
    private int m_curWeapon = -1;
    private int m_preWeapon = -1;
    private List<Weapon<T>> m_weapons = new List<Weapon<T>>();
    private GameObject m_uiParent;
    private List<UIWeapon> m_uis = new List<UIWeapon>();

    public GameObject Owner { get { return m_owner; } }
    public int CurWeapon { get { return m_curWeapon; } }
    public int PreWeapon { get { return m_preWeapon; } }
    public List<Weapon<T>> Weapons { get { return m_weapons; } }

    public WeaponManagement(GameObject owner)
    {
        m_owner = owner;

        m_uiParent = new GameObject("WeaponUIParent");
        m_uiParent.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    public void Add_Weapon(Weapon<T> weapons)
    {
        UIWeapon ui = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Weapon", m_uiParent.transform).GetComponent<UIWeapon>();
        if (ui == null)
            return; 

        weapons.Initialize_Weapon(this, ui);
        m_weapons.Add(weapons);

        ui.Initialize_UI(weapons.WeaponID, weapons.WeaponInfo);
        m_uis.Add(ui);


        // 현재 장착하고 있는 무기가 없다면 자동 장착
        if (m_curWeapon == -1)
            Change_Weapon(0);
        else
            Update_UIWeapons();
    }

    public void Update_Weapon()
    {
        if (m_curWeapon == -1)
            return;

        m_weapons[(int)m_curWeapon].Update_Weapon();
    }

    public void Change_Weapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= m_weapons.Count)
            return;

        if (m_curWeapon >= 0)
            m_weapons[(int)m_curWeapon].Exit_Weapon();

        m_preWeapon = m_curWeapon;
        m_curWeapon = weaponIndex;

        m_weapons[(int)m_curWeapon].Enter_Weapon();

        // UI 위치 업데이트
        Update_UIWeapons();
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

    public int Get_WeaponIndex(WeaponId weaponId)
    {
        for(int i = 0; i < m_weapons.Count; ++i)
        {
            if(m_weapons[i].WeaponID == weaponId)
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

    private void Update_UIWeapons()
    {
        if(m_weapons.Count == 1)
        {
            // 개수가 1개라면 제일 뒤
            m_uis[m_curWeapon].Update_UI(UIWeapon.POSITION.PT_BACK, true);
        }
        else if (m_weapons.Count == 2)
        {
            // 개수가 2개라면 중간
            m_uis[m_curWeapon].Update_UI(UIWeapon.POSITION.PT_MIDDLE, true);

            // 다른 한개는 맨뒤
            int NextIndex = (m_curWeapon == 0) ? 1 : 0;
            m_uis[NextIndex].Update_UI(UIWeapon.POSITION.PT_BACK, false);
        }
        else if (m_weapons.Count == 3)
        {
            // 개수가 3개라면 맨위
            m_uis[m_curWeapon].Update_UI(UIWeapon.POSITION.PT_FRONT, true);

            int NextIndex = m_curWeapon + 1;
            if (NextIndex >= m_weapons.Count) // 마지막 인덱스면 처음으로
                NextIndex = 0;
            m_uis[NextIndex].Update_UI(UIWeapon.POSITION.PT_MIDDLE, false);

            NextIndex += 1;
            if (NextIndex >= m_weapons.Count) // 마지막 인덱스면 처음으로
                NextIndex = 0;
            m_uis[NextIndex].Update_UI(UIWeapon.POSITION.PT_BACK, false);
        }
    }
}