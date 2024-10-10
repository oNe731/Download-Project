using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatingTable : MonoBehaviour
{
    private bool m_down = false;
    private Vector3 m_targetPosition;
    private float m_speed = 3f;

    private void Start()
    {
        m_targetPosition = new Vector3(transform.position.x, -0.86f, transform.position.z);
    }

    public void Start_Down()
    {
        m_down = true;
    }

    private void Update()
    {
        if (m_down == false)
            return;

        transform.position = Vector3.Lerp(transform.position, m_targetPosition, Time.deltaTime * m_speed);
        if (Vector3.Distance(transform.position, m_targetPosition) < 0.01f)
        {
            m_down = false;
            transform.position = m_targetPosition;
        }
    }
}
