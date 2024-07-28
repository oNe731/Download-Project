using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Base : State<HorrorPlayer>
    {
        protected HorrorPlayer m_player = null;

        protected float m_moveSpeed = 400.0f;
        protected float m_lerpSpeed = 5.0f;
        protected float m_rotationSpeed = 100.0f;
        protected Vector2 m_rotationLimit = new Vector2(-45f, 45f);

        protected bool  m_isLock = false;
        protected float m_xRotate = 0.0f;

        protected Transform m_transform;
        protected Rigidbody m_rigidbody;

        public HorrorPlayer_Base(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
            m_transform = m_stateMachine.Owner.GetComponent<Transform>();
            m_rigidbody = m_stateMachine.Owner.GetComponent<Rigidbody>();

            m_player = m_stateMachine.Owner.GetComponent<HorrorPlayer>();
        }

        public override void Enter_State()
        {
        }

        public override void Update_State()
        {
        }

        public override void Exit_State()
        {
        }

        protected bool Input_Move()
        {
            float yRotate = Input_Rotation();

            Vector3 forwardDir = Quaternion.Euler(0, yRotate, 0) * Vector3.forward;
            Vector3 rightDir   = Quaternion.Euler(0, yRotate, 0) * Vector3.right;

            Vector3 velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                velocity += forwardDir * m_moveSpeed * Time.deltaTime;
            else if (Input.GetKey(KeyCode.S))
                velocity += -forwardDir * m_moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
                velocity += rightDir * m_moveSpeed * Time.deltaTime;
            else if (Input.GetKey(KeyCode.A))
                velocity += -rightDir * m_moveSpeed * Time.deltaTime;

            if (velocity == Vector3.zero)
            {
                m_rigidbody.isKinematic = true;
            }
            else
            {
                m_rigidbody.isKinematic = false;
                m_rigidbody.velocity = Vector3.Lerp(m_rigidbody.velocity, velocity, Time.deltaTime * m_lerpSpeed);
                return true;
            }

            return false;
        }

        protected float Input_Rotation()
        {
            float xRotateSize = -Input.GetAxis("Mouse Y") * m_rotationSpeed * Time.deltaTime;
            float yRotateSize = Input.GetAxis("Mouse X") * m_rotationSpeed * Time.deltaTime;

            float yRotate = m_transform.eulerAngles.y + yRotateSize;
            m_xRotate = Mathf.Clamp(m_xRotate + xRotateSize, m_rotationLimit.x, m_rotationLimit.y); // 각도 제한(-45, 80)

            m_transform.eulerAngles = new Vector3(m_xRotate, yRotate, 0);

            return yRotate;
        }

        protected bool Input_Weapon()
        {
            // 키보드 ctrl
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                m_player.WeaponManagement.Next_Weapon(1);
                return true;
            }
            else
            {
                // 마우스 휠
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll > 0f)
                {
                    m_player.WeaponManagement.Next_Weapon(1);
                    return true;
                }
                else if (scroll < 0f)
                {
                    m_player.WeaponManagement.Next_Weapon(-1);
                    return true;
                }
            }

            return false;
        }

        protected void Input_Interaction()
        {
            // 상호작용
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit = GameManager.Instance.Start_Raycast(Camera.main.transform.position, Camera.main.transform.forward, 10f, LayerMask.GetMask("Interaction"));

                if (hit.collider != null)
                    Debug.Log($"상호작용 {hit.collider.gameObject.transform.parent.gameObject.name}");
            }
        }

        public void Set_Lock(bool isLock)
        {
            m_isLock = isLock;
            if (isLock)
                m_rigidbody.isKinematic = true;
        }
    }
}

