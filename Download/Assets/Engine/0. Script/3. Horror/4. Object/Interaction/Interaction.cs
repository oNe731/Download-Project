using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interaction : MonoBehaviour
{
    [SerializeField] protected Vector3 m_uiOffset;

    protected GameObject m_interactionUI = null;

    protected bool m_interact = false;
    protected float m_InteractionDist = 3f;

    public abstract void Click_Interaction();

    protected void Update_InteractionUI()
    {
        if (m_interactionUI == null || m_interact == true)
            return;

        if (Check_PlayerDist() == true)
        {
            if (m_interactionUI.activeSelf == true)
                return;

            m_interactionUI.SetActive(true);
        }
        else
        {
            if (m_interactionUI.activeSelf == false)
                return;

            m_interactionUI.SetActive(false);
        }
    }

    protected bool Check_PlayerDist()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, HorrorManager.Instance.Player.transform.position);
        if (distanceToPlayer <= m_InteractionDist)
            return true;

        return false;
    }

    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_InteractionDist);
#endif
    }
}
