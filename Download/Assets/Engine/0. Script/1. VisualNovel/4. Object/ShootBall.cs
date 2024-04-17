using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBall : MonoBehaviour
{
    private ShootSlingshot m_Owner;
    private Vector3 m_startPosition  = new Vector3(0f, 0f, 0f);
    private Vector3 m_targetPosition = new Vector3(0f, 0f, 0f);
    private float m_heightArc = 5.0f;
    private float m_speed = 2.0f;
    private float m_time = 0;
    private bool m_arrived = false;
    private SphereCollider m_collider;

    public ShootSlingshot Owner
    {
        set { m_Owner = value; }
    }
    public Vector3 TargetPosition
    {
        set { m_targetPosition = value; }
    }
    public float Speed
    {
        set { m_speed = value; }
    }

    private void Start()
    {
        m_startPosition = transform.position;
        m_collider = GetComponent<SphereCollider>();

        // 각도에 따른 속도 조절
        float angle = Vector3.Angle((m_startPosition - m_targetPosition).normalized, Vector3.up);
        // Debug.Log("1 : " + angle.ToString());
        if (angle < 90)
            angle = 90 + (90 - angle);
        // Debug.Log("2 : " + angle.ToString());
        float result = (90 - Mathf.Abs(angle - 90));
        // Debug.Log("3 : " + result.ToString());
        m_speed *= result;
    }

    private void Update()
    {
        float nextX = Mathf.MoveTowards(transform.position.x, m_targetPosition.x, m_speed * Time.deltaTime);

        float distance = m_targetPosition.x - m_startPosition.x;
        float baseY = Mathf.Lerp(m_startPosition.y, m_targetPosition.y, (nextX - m_startPosition.x) / distance);
        float arc = m_heightArc * (nextX - m_startPosition.x) * (nextX - m_targetPosition.x) / (-0.25f * distance * distance);

        Vector3 nextPosition = new Vector3(nextX, baseY + arc, transform.position.z);
        transform.rotation = LookAt2D(nextPosition - transform.position);
        transform.position = nextPosition;

        if (!m_arrived)
        {
            if (nextPosition == m_targetPosition)
                Arrived();
        }
        else
        {
            m_time += Time.deltaTime;
            if (m_time > 0.1f)
                Destroy(gameObject);
        }
    }

    private Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    private void Arrived()
    {
        m_arrived = true;

        m_collider.enabled = true;
        m_Owner.Use = true;
    }
}
