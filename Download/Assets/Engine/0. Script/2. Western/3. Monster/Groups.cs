using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groups : MonoBehaviour
{
    [SerializeField] private Group[] m_groups;

    private int m_currentIndex = -1;

    private void Start()
    {
    }

    public void WakeUp_Next(bool isCount = true)
    {
        m_currentIndex++;
        m_groups[m_currentIndex].WakeUp_Group(isCount);
    }
}
