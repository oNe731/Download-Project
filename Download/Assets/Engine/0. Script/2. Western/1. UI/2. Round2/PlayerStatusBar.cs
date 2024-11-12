using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Western
{
    public class PlayerStatusBar : MonoBehaviour
    {
        [SerializeField] private Transform[] m_positions;

        private Slider      m_barSlider;
        private AudioSource m_audioSource;
        private Animator    m_horseAnimator;
        private float       m_totalDistance;

        private void Start()
        {
            m_barSlider     = GetComponent<Slider>();
            m_audioSource   = GetComponent<AudioSource>();
            m_horseAnimator = GetComponentInChildren<Animator>();

            m_totalDistance = Vector3.Distance(m_positions[0].position, m_positions[1].position);
        }

        public void Update_Value(Vector3 startPosition, Vector3 currentPosition)
        {
            m_horseAnimator.SetBool("isRun", true);

            float currentDistance = Vector3.Distance(currentPosition, startPosition);
            m_barSlider.value = Mathf.Clamp01(currentDistance / m_totalDistance);
        }

        public void Stop_Value()
        {
            m_horseAnimator.SetBool("isRun", false);
        }
    }
}
