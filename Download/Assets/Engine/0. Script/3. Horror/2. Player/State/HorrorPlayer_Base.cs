using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Base : State<HorrorPlayer>
    {
        protected HorrorPlayer m_player = null;
        protected UIWorldHint m_interactionUI = null;

        protected float m_moveSpeed = 400.0f;
        protected float m_lerpSpeed = 5.0f;
        protected float m_rotationSpeed = 100.0f;
        protected Vector2 m_rotationLimit = new Vector2(-80f, 80f);

        protected bool m_isLock = false;
        protected bool m_recoverStamina = false;

        protected Transform m_transform;
        protected Transform m_rotationTransform;
        protected Rigidbody m_rigidbody;

        public HorrorPlayer_Base(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
            m_transform = m_stateMachine.Owner.GetComponent<Transform>();
            m_rotationTransform = m_transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).transform;
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
            if (m_player == null)
                return 0f;

            // Y축 좌우 회전
            float yRotateSize = Input.GetAxis("Mouse X") * m_rotationSpeed * Time.deltaTime;
            float yRotate = m_transform.eulerAngles.y + yRotateSize;
            m_transform.eulerAngles = new Vector3(m_transform.eulerAngles.x, yRotate, 0);

            // X축 상하 회전
            float xRotateSize = -Input.GetAxis("Mouse Y") * m_rotationSpeed * Time.deltaTime;
            m_player.XRotate = Mathf.Clamp(m_player.XRotate + xRotateSize, m_rotationLimit.x, m_rotationLimit.y); // 각도 제한(-45, 80)
            m_rotationTransform.eulerAngles = new Vector3(m_player.XRotate, m_rotationTransform.eulerAngles.y, 0);

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
            // 실시간으로 바라보고 있는 UI만 활성화
            RaycastHit hit = GameManager.Instance.Start_Raycast(Camera.main.transform.position, Camera.main.transform.forward, 5f, LayerMask.GetMask("Interaction"));
            if (hit.collider == null) // 제일 가까운 콜라이더 반환
            {
                Reset_Interaction();
                return;
            }

           Interaction interaction = hit.collider.gameObject.transform.GetComponent<Interaction>();
            if (interaction == null)
                interaction = hit.collider.gameObject.transform.parent.GetComponent<Interaction>();
            if (interaction == null)
            {
                Reset_Interaction();
                return;
            }

            if(m_interactionUI != null)
            {
                if(m_interactionUI != interaction.InteractionUI)
                    m_interactionUI.gameObject.SetActive(false);
            }

            m_interactionUI = interaction.InteractionUI;
            if(m_interactionUI != null)
            {
                m_interactionUI.Update_Transform();
                m_interactionUI.gameObject.SetActive(true);
            }

            // 상호작용
            if (Input.GetKeyDown(KeyCode.F))
                interaction.Click_Interaction();
        }

        private void Reset_Interaction()
        {
            if (m_interactionUI == null)
                return;

            m_interactionUI.gameObject.SetActive(false);
            m_interactionUI = null;
        }

        protected void Check_Stemina()
        {
            if (m_player.Stamina < m_player.StaminaMax)
                m_recoverStamina = true;
            else
                m_recoverStamina = false;
        }

        protected void Recover_Stemina()
        {
            if (m_recoverStamina == false)
                return;

            m_recoverStamina = m_player.Set_Stamina(0.5f * Time.deltaTime);
        }

        public void Set_Lock(bool isLock)
        {
            m_isLock = isLock;
            if (isLock)
                m_rigidbody.isKinematic = true;
        }
    }
}

