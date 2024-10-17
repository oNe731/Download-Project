using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle_Symptom : MonoBehaviour
{
    private Tentacle m_tentacle;

    private bool m_isUpdate = false;
    private float m_symptomTime = 0f;
    private float m_time = 0f;

    public void Start_Symptom(Tentacle owner, float symptomTime)
    {
        m_tentacle = owner;
        m_symptomTime = symptomTime;
        m_isUpdate = true;
    }

    void Update()
    {
        if (m_isUpdate == false)
            return;

        m_time += Time.deltaTime;
        if (m_time >= m_symptomTime)
        {
            m_tentacle.Up_Tentacle();
            Destroy(gameObject);
        }
    }
}
