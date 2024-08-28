using System.Collections;
using UnityEngine;

public class Jumpscare_Sign : Jumpscare
{
    public override void Active_Jumpscare()
    {
        /*
        [A]에 등신대(서 있는 안내판같은 간판)가 서있는데
        주인공이 일정 범위 내로 가까워지면 빠르게 팍 앞으로 엎어진다.
        콜라이더 적용하지 않아도 될 것 같습니다.* 넘어지는 소리 사운드 O
        */
        //x축 0 -> -90

        m_isTrigger = true;

        StartCoroutine(Move_Sign());
        GetComponent<AudioSource>().Play();
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
