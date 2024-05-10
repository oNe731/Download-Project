using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    [SerializeField] private GameObject[] m_person;

    private Transform m_grouptransform;
    private Quaternion m_targetQuaternion;

    private float m_rotationSpeed = 3f;

    private void Start()
    {
        m_grouptransform = GetComponent<Transform>();
        m_targetQuaternion = Quaternion.Euler(new Vector3(0f, 0f, 0f));


        int randomIndex = Random.Range(0, 3);
        for (int i = 0; i < m_person.Length; ++i)
        {
            if (i == randomIndex)
            {
                m_person[i].AddComponent<Criminal>();
                m_person[i].GetComponent<Criminal>().Initialize();
            }
            else
            {
                m_person[i].AddComponent<Citizen>();
                m_person[i].GetComponent<Citizen>().Initialize();
            }
        }
    }

    private void Update()
    {
    }

    public void WakeUp_Group(bool isCount)
    {
        StartCoroutine(WakeUp(isCount));
    }

    private IEnumerator WakeUp(bool isCount)
    {
        while (m_grouptransform.rotation != m_targetQuaternion)
        {
            m_grouptransform.rotation = Quaternion.Slerp(m_grouptransform.rotation, m_targetQuaternion, m_rotationSpeed * Time.deltaTime);
            yield return null;
        }

        if (isCount) // 카운트 시작
            StartCoroutine(Count());
        else
            WesternManager.Instance.IsShoot = true;

        yield break;
    }

    private IEnumerator Count()
    {
        yield break;
    }
}
