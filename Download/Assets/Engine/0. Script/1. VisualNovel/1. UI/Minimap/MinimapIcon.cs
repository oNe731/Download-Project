using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    [SerializeField] private float m_lerpSpeed = 20.0f;
    private Transform m_playerTr;

    void Start()
    {
        m_playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void LateUpdate()
    {
        //transform.rotation = Quaternion.Euler(90.0f, m_playerTr.eulerAngles.y, 0.0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90.0f, m_playerTr.eulerAngles.y, 0.0f), Time.deltaTime * m_lerpSpeed);
    }
}
