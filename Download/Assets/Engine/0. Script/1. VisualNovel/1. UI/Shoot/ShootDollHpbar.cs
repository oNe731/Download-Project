using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootDollHpbar : MonoBehaviour
{
    private GameObject m_Owner;
    public GameObject Owner
    {
        set { m_Owner = value; }
    }

    private Slider m_barSlider;
    private ShootDoll m_ownerDoll;

    private void Start()
    {
        m_barSlider = GetComponent<Slider>();
        m_ownerDoll = m_Owner.GetComponent<ShootDoll>();
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(m_Owner.transform.position + new Vector3(0, 1.5f, 0));
        m_barSlider.value = m_ownerDoll.Hp;
    }
}
