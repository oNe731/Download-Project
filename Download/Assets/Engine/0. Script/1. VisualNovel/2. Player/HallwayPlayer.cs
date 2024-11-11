using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class HallwayPlayer : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed = 400.0f;
        [SerializeField] private float m_lerpSpeed = 5.0f;

        private bool m_isLock = true;
        private Rigidbody   m_rigidbody;
        private AudioSource m_audioSource;

        private float m_soundTime = 0f;

        public float MoveSpeed { set => m_moveSpeed = value; }

        private void Awake()
        {
            m_rigidbody   = GetComponent<Rigidbody>();
            m_audioSource = GetComponent<AudioSource>();
        }

        public void Update()
        {
            if (GameManager.Ins.IsGame == false)
            {
                if(m_rigidbody.isKinematic == false)
                    m_rigidbody.velocity = Vector3.zero;
                return;
            }

            if (!m_isLock)
                Input_Player();
        }

        private void Input_Player()
        {
            // ȸ��
            transform.rotation = Camera.main.transform.rotation;

            // �̵�
            Vector3 Velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                Velocity += transform.forward * m_moveSpeed * Time.deltaTime;
            else if (Input.GetKey(KeyCode.S))
                Velocity += -transform.forward * m_moveSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.D))
                Velocity += transform.right * m_moveSpeed * Time.deltaTime;
            else if (Input.GetKey(KeyCode.A))
                Velocity += -transform.right * m_moveSpeed * Time.deltaTime;

            if (Velocity == Vector3.zero)
            {
                m_rigidbody.isKinematic = true;
            }
            else
            {
                Play_WalkSound();
                m_rigidbody.isKinematic = false;
                m_rigidbody.velocity = Vector3.Lerp(m_rigidbody.velocity, Velocity, Time.deltaTime * m_lerpSpeed);
            }

            #region ���� �̵� �ڵ�
            // float InputX = Input.GetAxis("Horizontal");
            // float InputZ = Input.GetAxis("Vertical");
            // if(InputX != 0.0f || InputZ != 0.0f)
            // {
            //     Vector3 localDirection = new Vector3(InputX, 0, InputZ);
            //     localDirection.Normalize();
            //     Vector3 worldDirection = transform.TransformDirection(localDirection); // �̵� ������ �÷��̾��� ���� ��ǥ�迡�� ���� ��ǥ��� ��ȯ
            //     worldDirection.Normalize();

            //     Vector3 rayOrigin = transform.position + new Vector3(0, 0.5f, 0);
            //#if UNITY_EDITOR
            //     Debug.DrawRay(rayOrigin, worldDirection * 0.4f, Color.red); // ���� ����� ����
            //#endif
            //     if (!Physics.Raycast(rayOrigin, worldDirection, 0.4f, LayerMask.GetMask("Wall"))) // ���� ������ �ش� �������� �̵�.
            //          transform.Translate(localDirection * m_moveSpeed * Time.deltaTime);           // Translate�� �ش� ���⿡ �ݶ��̴� ���θ� �˻����� �ʰ� �̵���.
            //     }
            #endregion
        }

        public void Set_Lock(bool isLock)
        {
            m_isLock = isLock;
            if (isLock)
                m_rigidbody.isKinematic = true;
        }

        public void Stop_Player(bool stop)
        {
            Set_Lock(stop);
        }

        protected void Play_WalkSound()
        {
            m_soundTime += Time.deltaTime;
            if (m_soundTime >= 0.45f)
            {
                m_soundTime = 0f;

                int Index = Random.Range(0, 5);
                GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "VisualNovel_Player_Steps_" + Index.ToString(), false, 1f);
            }
        }
    }
}
