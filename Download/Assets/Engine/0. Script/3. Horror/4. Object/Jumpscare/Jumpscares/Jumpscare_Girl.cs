using System.Collections;
using UnityEngine;

public class Jumpscare_Girl : Jumpscare
{
    [SerializeField] private GameObject m_girl;

    public override void Active_Jumpscare()
    {
        m_isTrigger = true;

        // 웃음소리 사운드 재생
        //

        m_girl.SetActive(true);
        StartCoroutine(Move_Girl());
    }

    private IEnumerator Move_Girl()
    {
        // 대기 ---
        float time = 0f;
        while(true)
        {
            time += Time.deltaTime;
            if (time >= 0.3f) // 0.3초 정도
                break;
            yield return null;
        }

        // 걷기 ---
        float speed = 2f;
        while (true)
        {
            m_girl.transform.localPosition = new Vector3(m_girl.transform.localPosition.x, m_girl.transform.localPosition.y, m_girl.transform.localPosition.z + speed * Time.deltaTime);
            if (m_girl.transform.localPosition.z >= 3.2f)
                break;
            yield return null;
        }

        // 여자아이 삭제 ---
        Destroy(m_girl);
        yield break;
    }
}
