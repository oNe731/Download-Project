using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class CriminalGun : MonoBehaviour
    {
        public IEnumerator Start_Up()
        {
            Vector3 startPosition = transform.localPosition;
            Vector3 endPosition   = transform.localPosition;
            endPosition.y = 0f;

            float duration = 1f;
            float elapsed  = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
                yield return null;
            }

            transform.localPosition = endPosition;
            yield break;
        }
    }
}