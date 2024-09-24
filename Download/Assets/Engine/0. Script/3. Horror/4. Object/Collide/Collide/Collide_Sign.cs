using System.Collections;
using UnityEngine;

public class Collide_Sign : Collide
{
    public override void Trigger_Event()
    {
        GetComponent<AudioSource>().Play();
        StartCoroutine(Move_Sign());
    }

    private IEnumerator Move_Sign()
    {
        float duration = 0.2f;

        Quaternion startRotation = transform.GetChild(0).localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(-90, 0f, 0f);

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            transform.GetChild(0).localRotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.GetChild(0).localRotation = endRotation;
        yield break;
    }
}
