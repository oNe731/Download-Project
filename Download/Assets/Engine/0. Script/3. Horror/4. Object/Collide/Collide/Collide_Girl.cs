using System.Collections;
using UnityEngine;

public class Collide_Girl : Collide
{
    [SerializeField] private GameObject m_girl;

    public override void Trigger_Event()
    {
        m_girl.SetActive(true);
        StartCoroutine(Move_Girl());
    }

    private IEnumerator Move_Girl()
    {
        Horror_Base level = GameManager.Ins.Horror.LevelController.Get_CurrentLevel<Horror_Base>();
        HorrorLight light1 = level.Light.transform.GetChild(1).GetComponent<HorrorLight>();
        HorrorLight light2 = level.Light.transform.GetChild(2).GetComponent<HorrorLight>();

        // 대기 ---
        float time = 0f;
        while(true)
        {
            time += Time.deltaTime;
            if (time >= 0.3f) // 0.3초 정도
                break;
            yield return null;
        }

        light1.Start_Blink(true, 0.3f, 0.4f);
        light2.Start_Blink(true, 0.4f, 0.5f);

        // 걷기 ---
        float speed = 2.5f;
        while (true)
        {
            m_girl.transform.localPosition = new Vector3(m_girl.transform.localPosition.x, m_girl.transform.localPosition.y, m_girl.transform.localPosition.z + speed * Time.deltaTime);
            if (m_girl.transform.localPosition.z >= 3.2f)
                break;
            yield return null;
        }

        light1.Finish_Event();
        light2.Finish_Event();

        // 여자아이 삭제 ---
        Destroy(m_girl);
        yield break;
    }
}
