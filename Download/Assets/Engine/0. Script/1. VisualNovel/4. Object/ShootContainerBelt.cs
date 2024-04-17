using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootContainerBelt : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject[] m_Dolls;

    [Header("Resource")]
    [SerializeField] private GameObject m_Hpbar;

    private bool m_startBelt = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void Start_Belt()
    {
        Transform CanvasTransform = GameObject.Find("Canvas").transform;

        for (int i = 0; i < m_Dolls.Length; ++i)
        {
            GameObject clone = Instantiate(m_Hpbar, Vector2.zero, Quaternion.identity, CanvasTransform);
            clone.GetComponent<ShootDollHpbar>().Owner = m_Dolls[i];
        }

        m_startBelt = true;
    }
}
