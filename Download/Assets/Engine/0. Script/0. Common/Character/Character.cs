using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Collider[] m_Colliders;

    private void Awake()
    {
        // 모든 콜라이더 간의 충돌을 무시
        for (int i = 0; i < m_Colliders.Length; i++)
        {
            for (int j = i + 1; j < m_Colliders.Length; j++)
                Physics.IgnoreCollision(m_Colliders[i], m_Colliders[j], true);
        }
    }
}
