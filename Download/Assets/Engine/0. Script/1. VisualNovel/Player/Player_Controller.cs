using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 5.0f;
    [SerializeField] private float m_turnSpeed = 500.0f;

    private Rigidbody m_rigidbodyCom;

    void Awake()
    {
        m_rigidbodyCom = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Input_Player();
    }

    private void Input_Player()
    {
        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");
        float MouseX = Input.GetAxis("Mouse X");

        // 플레이어 이동
        if (InputX != 0.0f || InputZ != 0.0f)
        {
            Vector3 Dir = (transform.forward * InputZ) + (transform.right * InputX);
            // transform.Translate(Dir.normalized * m_moveSpeed * Time.deltaTime);
            m_rigidbodyCom.MovePosition(transform.position + Dir.normalized * m_moveSpeed * Time.deltaTime); // MovePosition : 지속적인 움직임 표현할 때 사용
        }

        // 플레이어 회전
        transform.Rotate(Vector3.up * m_turnSpeed * Time.deltaTime * MouseX);
    }
}