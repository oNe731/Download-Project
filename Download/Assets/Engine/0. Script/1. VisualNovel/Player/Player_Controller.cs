using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform m_mainCamera;
    [SerializeField] private float m_moveSpeed = 5.0f;
    [SerializeField] private float m_lerpSpeed = 5.0f;

    [SerializeField] private Minimap m_Minimap;

    private Rigidbody m_rigidbody;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Input_Player();
        m_Minimap.Update_Minimap(); // 미니맵 업데이트
    }

    private void Input_Player()
    {
        // 회전
        transform.rotation = m_mainCamera.rotation;

        // 이동
        Vector3 Velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            Velocity += transform.forward * m_moveSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            Velocity += -transform.forward * m_moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
            Velocity += transform.right * m_moveSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.A))
            Velocity += -transform.right * m_moveSpeed * Time.deltaTime;
        m_rigidbody.velocity = Vector3.Lerp(m_rigidbody.velocity, Velocity, Time.deltaTime * m_lerpSpeed);

        // float InputX = Input.GetAxis("Horizontal");
        // float InputZ = Input.GetAxis("Vertical");
        // if(InputX != 0.0f || InputZ != 0.0f)
        // {
        //     Vector3 localDirection = new Vector3(InputX, 0, InputZ);
        //     localDirection.Normalize();
        //     Vector3 worldDirection = transform.TransformDirection(localDirection); // 이동 방향을 플레이어의 로컬 좌표계에서 월드 좌표계로 변환
        //     worldDirection.Normalize();

        //     Vector3 rayOrigin = transform.position + new Vector3(0, 0.5f, 0);
        //#if UNITY_EDITOR
        //     Debug.DrawRay(rayOrigin, worldDirection * 0.4f, Color.red); // 레이 디버그 렌더
        //#endif
        //     if (!Physics.Raycast(rayOrigin, worldDirection, 0.4f, LayerMask.GetMask("Wall"))) // 벽이 없으면 해당 방향으로 이동.
        //          transform.Translate(localDirection * m_moveSpeed * Time.deltaTime);           // Translate은 해당 방향에 콜라이더 여부를 검사하지 않고 이동함.
        //     }
    }

    private void OnCollisionEnter(Collision collision)
    {
    }
}