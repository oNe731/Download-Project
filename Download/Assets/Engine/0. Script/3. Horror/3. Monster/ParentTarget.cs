using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentTarget : MonoBehaviour
{
    [SerializeField] private Transform m_parentTransform;
    public Transform ParentTransform => m_parentTransform;
}
