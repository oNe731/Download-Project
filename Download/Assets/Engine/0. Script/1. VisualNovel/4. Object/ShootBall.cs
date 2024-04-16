using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBall : MonoBehaviour
{
    private Vector3 m_startPosition  = new Vector3(0f, 0f, 0f);
    private Vector3 m_targetPosition = new Vector3(0f, 0f, 0f);
    private float m_heightArc = 5.0f;
    private float m_speed = 2.0f;

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
    }

    private void Update()
    {
        float x0 = m_startPosition.x;
        float x1 = m_targetPosition.x;
        float distance = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, m_speed * Time.deltaTime);
        float baseY = Mathf.Lerp(m_startPosition.y, m_targetPosition.y, (nextX - x0) / distance);
        float arc = m_heightArc * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);

        Vector3 nextPosition = new Vector3(nextX, baseY + arc, transform.position.z);
        transform.rotation = LookAt2D(nextPosition - transform.position);
        transform.position = nextPosition;

        if (nextPosition == m_targetPosition)
            Arrived();
    }

    private Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    private void Arrived()
    {
        Destroy(gameObject);
    }
}
