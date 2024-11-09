using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarPeoples : MonoBehaviour
{
    private bool m_isUp = false;
    public bool IsUp => m_isUp;

    private void Start()
    {
        
    }

    public IEnumerator Start_Up()
    {
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = transform.localPosition;
        endPosition.y = 0f;

        float duration = 2.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            yield return null;
        }

        transform.localPosition = endPosition;
        m_isUp = true;
        yield break;
    }

    public void Dance_Peoples()
    {
        Animator[] animators = GetComponentsInChildren<Animator>();
        foreach (Animator animator in animators)
        {
            animator.SetBool("isDance", true);
        }
    }

    public void Finish_Peoples()
    {
        Animator[] animators = GetComponentsInChildren<Animator>();
        foreach (Animator animator in animators)
        {
            animator.SetBool("isFinish", true);
        }
    }
}
