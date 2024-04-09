using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject MainCamera;   // 메인 카메라
    [SerializeField] private float MoveSpeed = 5.0f;
    [SerializeField] private float TurnSpeed = 600.0f;

    private Rigidbody RigidbodyCom;

    void Start()
    {
        RigidbodyCom = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

    }

    void Update()
    {
        Input_Player();
    }

    private void Input_Player()
    {
        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");

        Vector3 Velocity = (transform.forward * InputZ + transform.right * InputX).normalized;
        if (InputX != 0.0f || InputZ != 0.0f)
        {
            // 카메라가 바라보는 방향으로 회전
            //Vector3 CameraDirection = MainCamera.transform.forward;
            //CameraDirection.y = 0f;
            //Quaternion Rotation = Quaternion.LookRotation(CameraDirection, Vector3.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Rotation, Time.deltaTime * TurnSpeed);
            //transform.rotation = MainCamera.transform.rotation;
            // 회전이 완료되면 이동
            //if (Quaternion.Angle(transform.rotation, Rotation) < 0.1f)
            //{

                RigidbodyCom.MovePosition(transform.position + Velocity * MoveSpeed * Time.deltaTime); // MovePosition : 지속적인 움직임 표현할 때 사용


            //}
        }

        transform.forward = MainCamera.transform.forward;
    }
}