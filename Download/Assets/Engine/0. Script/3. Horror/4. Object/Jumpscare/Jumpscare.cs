using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Jumpscare : MonoBehaviour
{
    protected bool m_isTrigger = false;

    public bool IsTriger => m_isTrigger;
    public abstract void Active_Jumpscare();

    private void OnTriggerEnter(Collider other)
    {
        if (m_isTrigger == true || other.gameObject.CompareTag("Player") == false)
            return;

        Active_Jumpscare();
    }
}
