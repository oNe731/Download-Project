using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Western
{
    public class StatusBarUI : MonoBehaviour
    {
        private Slider      m_barSlider;
        private AudioSource m_audioSource;
        private Animator    m_horseAnimator;
        private Coroutine m_coroutine = null;

        private void Start()
        {
            m_barSlider     = GetComponent<Slider>();
            m_audioSource   = GetComponent<AudioSource>();
            m_horseAnimator = GetComponentInChildren<Animator>();
        }

        public void Start_UpdateValue(int currentIndex, int nextIndex, Vector3 currentPosition, Vector3 nextPosition)
        {
            if (m_coroutine != null)
                StopCoroutine(m_coroutine);
            m_coroutine = StartCoroutine(UpdateValue(currentIndex, nextIndex, currentPosition, nextPosition));
        }

        private IEnumerator UpdateValue(int currentIndex, int nextIndex, Vector3 currentPosition, Vector3 nextPosition)
        {
            m_horseAnimator.SetBool("isRun", true);
            m_audioSource.pitch  = 0.7f;
            m_audioSource.volume = GameManager.Ins.Sound.EffectSound;
            m_audioSource.Play();
            while (true)
            {
                Vector3 cameraPosition = Camera.main.transform.position;

                m_barSlider.value = Mathf.Lerp(currentIndex, nextIndex, 
                    Mathf.Clamp01(Vector3.Distance(currentPosition, cameraPosition) / Vector3.Distance(currentPosition, nextPosition)));

                if (cameraPosition.z == nextPosition.z)
                    break;

                yield return null;
            }

            m_barSlider.value = nextIndex;
            m_horseAnimator.SetBool("isRun", false);
            m_audioSource.Stop();
            yield break;
        }
    }
}
