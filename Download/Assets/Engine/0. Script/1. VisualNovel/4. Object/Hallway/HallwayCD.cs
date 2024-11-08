using System.Collections;
using UnityEngine;

namespace VisualNovel
{
    public class HallwayCD : MonoBehaviour
    {
        private int m_positionIndex = -1;
        public int PositionIndex
        {
            get => m_positionIndex;
            set => m_positionIndex = value;
        }

        private float m_floatSpeed = 1f;
        private float m_height = 0.2f;
        private float m_startY = 0f;

        //private Quaternion m_initialRotation;

        private bool m_trigger = false;
        private AudioSource m_audioSource;

        private void Start()
        {
            m_audioSource = GetComponent<AudioSource>();

            m_startY = 0.5f + m_height;
            //m_initialRotation = transform.rotation;
        }

        private void Update()
        {
            float newY = m_startY + Mathf.Sin(Time.time * m_floatSpeed) * m_height;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            Vector3 directionToCamera = Camera.main.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);
            transform.rotation = lookRotation;// * m_initialRotation;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (m_trigger == true)
                return;

            if (other.gameObject.CompareTag("Player"))
            {
                m_trigger = true;

                GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Day3Chase>().Get_CD(m_positionIndex);
                StartCoroutine(Wait_PlaySound());
            }
        }

        IEnumerator Wait_PlaySound()
        {
            Destroy(transform.GetChild(0).gameObject); // 메시 삭제
            Destroy(transform.GetChild(1).gameObject); // UI 삭제

            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);

            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 10.0f);
#endif
        }
    }
}

