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
            // 회전
            transform.rotation = Camera.main.transform.rotation;

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

            #region 기존 이동 코드
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
