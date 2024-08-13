using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interaction : MonoBehaviour
{
    [SerializeField] protected Vector3 m_uiOffset;

    protected UIWorldHint m_interactionUI = null;

    protected bool m_interact = false;
    protected float m_InteractionDist = 2.2f;

    public abstract void Click_Interaction();

    protected void Update_InteractionUI()
    {
        if (m_interactionUI == null || m_interact == true)
            return;

        if (Check_PlayerDist() == true)
        {
            if (m_interactionUI.gameObject.activeSelf == true)
                return;

            m_interactionUI.Update_Transform();
            m_interactionUI.gameObject.SetActive(true);
        }
        else
        {
            if (m_interactionUI.gameObject.activeSelf == false)
                return;

            m_interactionUI.gameObject.SetActive(false);
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

    protected void Desttoy_Interaction()
    {
        m_interactionUI.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
